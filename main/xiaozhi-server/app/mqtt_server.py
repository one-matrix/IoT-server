import asyncio
import json
import logging
import uuid

import paho.mqtt.client as mqtt

from app.mqtt_gateway import MqttGateway, CustomSocket
from app.session_manager import session_manager
from app.udp_server import UdpServer
from config.settings import load_config

config = load_config()
logging.basicConfig(level=logging.INFO, format='%(asctime)s - %(levelname)s - %(message)s')
logger = logging.getLogger(__name__)

# MQTT Broker 配置 (请根据你的环境修改)
# 服务端监听的 MQTT 主题 (使用通配符+来匹配所有客户端)
# 假设客户端的 publish_topic 是 "devices/up/{client_id}"
# 假设客户端的 subscribe_topic 是 "devices/down/{client_id}"
MQTT_Endpoint = config.get("mqtt", {}).get("endpoint", "127.0.0.1")
MQTT_PORT = config.get("mqtt", {}).get("port", 1883)
MQTT_USERNAME = config.get("mqtt", {}).get("username", "")
MQTT_PASSWORD = config.get("mqtt", {}).get("password", "")
SERVER_CLIENT_ID = f'{config.get("mqtt", {}).get("server_client_id_prefix", "server-")}{uuid.uuid4()}'
MQTT_UPSTREAM_TOPIC = config.get("mqtt", {}).get("upstream_topic", "devices/up/+")
MQTT_DOWNSTREAM_TOPIC_TPL = config.get("mqtt", {}).get("downstream_topic_tpl", "devices/down/{}")

UDP_HOST = config.get("udp", {}).get("host", "0.0.0.0")
UDP_PORT = config.get("udp", {}).get("port", 12345)
SERVER_PUBLIC_IP = config.get("udp", {}).get("server_public_ip", "127.0.0.1")

class MqttServer:
    def __init__(self, loop):
        self.loop = loop
        self.session_manager = session_manager
        self.udp_server = UdpServer(UDP_HOST, UDP_PORT, self.session_manager, self.handle_udp_data)
        self.mqtt_client = None
        self.gateway_map = {}
        self.gateway = MqttGateway(config)

    def start(self):
        logger.info("正在启动小直服务端...")
        self.udp_server.start()
        self._start_mqtt_client()
        logger.info("服务端已成功启动。")

    def stop(self):
        logger.info("正在停止服务端...")
        self.udp_server.stop()
        if self.mqtt_client:
            self.mqtt_client.loop_stop()
            self.mqtt_client.disconnect()
        logger.info("服务端已停止。")

    def _start_mqtt_client(self):
        """初始化并连接到MQTT Broker"""
        self.mqtt_client = mqtt.Client(client_id=SERVER_CLIENT_ID,callback_api_version=mqtt.CallbackAPIVersion.VERSION2)
        self.mqtt_client.username_pw_set(MQTT_USERNAME, MQTT_PASSWORD)
        # 根据客户端代码，启用TLS
        # self.mqtt_client.tls_set(
        #      ca_certs=None,
        #      certfile=None,
        #      keyfile=None,
        #      cert_reqs=mqtt.ssl.CERT_REQUIRED,
        #      tls_version=mqtt.ssl.PROTOCOL_TLS,
        #  )
        self.mqtt_client.on_connect = self._on_mqtt_connect
        self.mqtt_client.on_message = self._on_mqtt_message
        self.mqtt_client.on_disconnect = self._on_mqtt_disconnect

        try:
            logger.info(f"正在连接到 MQTT Broker: {MQTT_Endpoint}:{MQTT_PORT}")
            self.mqtt_client.connect(MQTT_Endpoint, MQTT_PORT, 60)
            self.mqtt_client.loop_start()
        except Exception as e:
            logger.error(f"连接 MQTT Broker 失败: {e}", exc_info=True)
            raise

    def _on_mqtt_connect(self, client, userdata, flags, rc, properties=None):
        """MQTT连接成功的回调"""
        if rc == 0:
            logger.info("成功连接到 MQTT Broker")
            client.subscribe(MQTT_UPSTREAM_TOPIC)
            logger.info(f"已订阅主题: {MQTT_UPSTREAM_TOPIC}")
        else:
            logger.error(f"连接 MQTT Broker 失败，返回码: {rc}")

    def _on_mqtt_disconnect(self, client, userdata, flags, rc, properties=None):
        """MQTT断开连接的回调"""
        logger.warning(f"与 MQTT Broker 的连接已断开，返回码: {rc}")

    def _on_mqtt_message(self, client, userdata, msg):
        try:
            print(f"从主题 {msg.topic} 收到消息client: {msg.payload}")
            topic = msg.topic
            payload = msg.payload.decode('utf-8')
            data = json.loads(payload)
            msg_type = data.get("type")
            client_id = data.get("client_id")

            if not client_id:
                parts = topic.split('/')
                if len(parts) >= 3:
                    client_id = parts[2]
                else:
                    logger.warning(f"收到未知主题的消息: {topic}")
                    return

            logger.info(f"从客户端 '{client_id}' 收到消息, 类型: {msg_type}")

            if msg_type == "hello":
                self._handle_client_hello(client_id, data)
            elif msg_type == "goodbye":
                self._handle_client_goodbye(client_id, data)
            else:
                if client_id in self.gateway_map:
                    _, socket = self.gateway_map[client_id]
                    asyncio.run_coroutine_threadsafe(socket.feed_message(json.dumps(data)), self.loop)
                else:
                    logger.warning(f"未找到client_id={client_id}的gateway实例，消息丢弃")
        except json.JSONDecodeError:
            logger.error(f"收到了无效的JSON数据: {msg.payload.decode('utf-8')}")
        except Exception as e:
            logger.error(f"处理MQTT消息时出错: {e}", exc_info=True)

    def _handle_client_hello(self, client_id, hello_data):
        audio_params = hello_data.get("audio_params", {})
        self.session_manager.delete_session(client_id)
        session = self.session_manager.create_session(client_id, audio_params)

        response = {
            "type": "hello",
            "session_id": session["session_id"],
            "transport": "udp",
            "udp": {
                "server": SERVER_PUBLIC_IP,
                "port": UDP_PORT,
                "key": session["aes_key"].hex(),
                "nonce": session["aes_nonce"].hex()
            },
            'audio_params': {'format': 'opus', 'sample_rate': 16000, 'channels': 1, 'frame_duration': 60}
        }

        response_topic = MQTT_DOWNSTREAM_TOPIC_TPL.format(client_id)
        self.mqtt_client.publish(response_topic, json.dumps(response), qos=1)
        logger.info(f"已向客户端 '{client_id}' 发送hello响应至主题: {response_topic}")
        print(f"gateway_map: {self.gateway_map}")
        if client_id not in self.gateway_map:
            socket = CustomSocket(
                config,
                mqtt_client=self.mqtt_client,
                udp_socket=self.udp_server.socket,
                client_id=client_id,
                udp_addr=None,  # Will be updated by session_manager
                send_audio=self.udp_server.send_audio
            )
            socket.request.headers["client_id"] = client_id
            self.gateway_map[client_id] = (self.gateway, socket)
            print(f"已创建新的gateway实例，client_id: {client_id}")
            asyncio.run_coroutine_threadsafe(self.gateway._handle_connection(socket,session["session_id"]), self.loop)

    def _handle_client_goodbye(self, client_id, goodbye_data):
        session_id = goodbye_data.get("session_id")
        self.session_manager.delete_session(client_id, session_id)
        if client_id in self.gateway_map:
            del self.gateway_map[client_id]

    def handle_udp_data(self, client_id, decrypted_audio):
        logger.info(f"收到并解密了来自 {client_id} 的 {len(decrypted_audio)} 字节音频数据。")
        if client_id in self.gateway_map:
            _, socket = self.gateway_map[client_id]
            asyncio.run_coroutine_threadsafe(socket.feed_message(decrypted_audio), self.loop)
        else:
            logger.warning(f"未找到client_id={client_id}的gateway实例，UDP消息丢弃")


if __name__ == "__main__":
    main_loop = asyncio.get_event_loop()
    server = MqttServer(main_loop)

    try:
        server.start()
        while True:
            asyncio.sleep(1)
    except KeyboardInterrupt:
        logger.info("收到关闭信号...")
    except Exception as e:
        logger.error(f"服务器启动或运行期间发生严重错误: {e}", exc_info=True)
    finally:
        server.stop()
        main_loop.close()