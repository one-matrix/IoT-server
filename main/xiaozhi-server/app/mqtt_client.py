# mqtt_client.py
import paho.mqtt.client as mqtt
import json
from pydantic import ValidationError

from app.config import settings
from app.models import HelloMessage, HelloResponse, UdpTransportInfo, AudioParams
from app.session_manager import session_manager
from config.settings import load_config
config = load_config()
class MQTTClient:
    def __init__(self):
        self.client = mqtt.Client(mqtt.CallbackAPIVersion.VERSION2)
        self.client.username_pw_set(config.get("mqtt", {}).get("username", ""), config.get("mqtt", {}).get("password", ""))
        # 配置TLS加密连接
        # self.client.tls_set(
        #         ca_certs=None,
        #          certfile=None,
        #          keyfile=None,
        #          cert_reqs=mqtt.ssl.CERT_REQUIRED,
        #          tls_version=mqtt.ssl.PROTOCOL_TLS,
        #      )
        self.client.on_connect = self.on_connect
        self.client.on_message = self.on_message

    def on_connect(self, client, userdata, flags, rc, properties):
        """连接到 MQTT Broker 后的回调函数。"""
        if rc == 0:
            print("Connected to MQTT Broker!")
            client.subscribe(settings.mqtt_sub_topic)
            print(f"Subscribed to topic: {settings.mqtt_sub_topic}")
        else:
            print(f"Failed to connect, return code {rc}\n")

    def on_message(self, client, userdata, msg):
        """接收到消息时的回调函数。"""
        try:
            # 从主题中解析 device_id
            # "devices/DEVICE123/control" -> "DEVICE123"
            topic_parts = msg.topic.split('/')
            if len(topic_parts) < 3:
                return
            device_id = topic_parts[1]
            
            print(f"Received message from device {device_id} on topic {msg.topic}")

            payload = json.loads(msg.payload.decode())
            
            # 验证消息是否为 "hello"
            hello_msg = HelloMessage(**payload)
            if hello_msg.type != "hello":
                return

            # 创建会话
            session = session_manager.create_session(device_id=device_id)

            # 构建响应
            udp_info = UdpTransportInfo(
                server=settings.udp_public_ip,
                port=settings.udp_server_port,
                encryption="aes-128-ctr",
                key=session.udp_key.hex(),
                nonce=session.udp_nonce.hex(),
            )
            response = HelloResponse(
                type="hello",
                version=3,
                session_id=session.session_id,
                transport="udp",
                udp=udp_info,
                audio_params=hello_msg.audio_params # 确认音频参数
            )
            
            # 发送响应
            response_topic = settings.mqtt_pub_topic_format.format(device_id=device_id)
            client.publish(response_topic, response.model_dump_json())
            print(f"Sent hello response to {response_topic}")

        except json.JSONDecodeError:
            print(f"Error decoding JSON from topic {msg.topic}")
        except ValidationError as e:
            print(f"Message validation error from topic {msg.topic}: {e}")
        except Exception as e:
            print(f"An unexpected error occurred in on_message: {e}")

    def connect(self):
        """连接到 MQTT Broker。"""
        print(f"Connecting to MQTT Broker at {settings.mqtt_endpoint}:{settings.mqtt_port}")
        self.client.connect(settings.mqtt_endpoint, settings.mqtt_port, 60)

    def start(self):
        """以非阻塞方式启动 MQTT 客户端循环。"""
        self.client.loop_start()

    def stop(self):
        """停止 MQTT 客户端。"""
        self.client.loop_stop()

mqtt_client = MQTTClient()