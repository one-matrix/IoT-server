import asyncio
import websockets
from fastapi import WebSocket
from config.logger import setup_logging
from core.connection import ConnectionHandler
from config.config_loader import get_config_from_api
from core.utils.modules_initialize import initialize_modules
from core.utils.util import check_vad_update, check_asr_update

TAG = __name__

class MqttGateway:
    def __init__(self, config: dict):
        self.config = config
        self.logger = setup_logging()
        self.config_lock = asyncio.Lock()
        modules = initialize_modules(
            self.logger,
            self.config,
            "VAD" in self.config["selected_module"],
            "ASR" in self.config["selected_module"],
            "LLM" in self.config["selected_module"],
            False,
            "Memory" in self.config["selected_module"],
            "Intent" in self.config["selected_module"],
        )
        self._vad = modules["vad"] if "vad" in modules else None
        self._asr = modules["asr"] if "asr" in modules else None
        self._llm = modules["llm"] if "llm" in modules else None
        self._intent = modules["intent"] if "intent" in modules else None
        self._memory = modules["memory"] if "memory" in modules else None

        self.active_connections = set()

    async def start(self):
        server_config = self.config["server"]
        host = server_config.get("ip", "0.0.0.0")
        port = int(server_config.get("port", 8000))
        #self._handle_connection, host, port, process_request=self._http_response
        socket = CustomSocket()
        self._handle_connection(socket)
      
    async def _handle_connection(self, websocket,session_id=None):

        """处理新连接，每次创建独立的ConnectionHandler"""
        # 创建ConnectionHandler时传入当前server实例
        handler = ConnectionHandler(
            self.config,
            self._vad,
            self._asr,
            self._llm,
            self._memory,
            self._intent,
            self,  # 传入server实例
            session_id,
        )
        self.active_connections.add(handler)
        try:
            await handler.handle_connection(websocket)
        finally:
            self.active_connections.discard(handler)

    async def _http_response(self, websocket, request_headers):
        # 检查是否为 WebSocket 升级请求
        if request_headers.headers.get("connection", "").lower() == "upgrade":
            # 如果是 WebSocket 请求，返回 None 允许握手继续
            return None
        else:
            # 如果是普通 HTTP 请求，返回 "server is running"
            print("server is running")
            return websocket.respond(200, "Server is running\n")

    async def update_config(self) -> bool:
        """更新服务器配置并重新初始化组件

        Returns:
            bool: 更新是否成功
        """
        try:
            async with self.config_lock:
                # 重新获取配置
                new_config = get_config_from_api(self.config)
                if new_config is None:
                    self.logger.bind(tag=TAG).error("获取新配置失败")
                    return False
                self.logger.bind(tag=TAG).info(f"获取新配置成功")
                # 检查 VAD 和 ASR 类型是否需要更新
                update_vad = check_vad_update(self.config, new_config)
                update_asr = check_asr_update(self.config, new_config)
                self.logger.bind(tag=TAG).info(
                    f"检查VAD和ASR类型是否需要更新: {update_vad} {update_asr}"
                )
                # 更新配置
                self.config = new_config
                # 重新初始化组件
                modules = initialize_modules(
                    self.logger,
                    new_config,
                    update_vad,
                    update_asr,
                    "LLM" in new_config["selected_module"],
                    False,
                    "Memory" in new_config["selected_module"],
                    "Intent" in new_config["selected_module"],
                )

                # 更新组件实例
                if "vad" in modules:
                    self._vad = modules["vad"]
                if "asr" in modules:
                    self._asr = modules["asr"]
                if "llm" in modules:
                    self._llm = modules["llm"]
                if "intent" in modules:
                    self._intent = modules["intent"]
                if "memory" in modules:
                    self._memory = modules["memory"]
                self.logger.bind(tag=TAG).info(f"更新配置任务执行完毕")
                return True
        except Exception as e:
            self.logger.bind(tag=TAG).error(f"更新服务器配置失败: {str(e)}")
            return False

from types import SimpleNamespace
class CustomSocket:
    def __init__(self, config: dict, mqtt_client=None, udp_socket=None, client_id=None, udp_addr=None, send_audio=None):
        self.config = config
        #headers 和 remote_address 没有意义，只是为了兼容
        self.request = SimpleNamespace()
        self.request.headers = {"content-type": "application/json", "host": "127.0.0.1", "device-id": "xiaozhi-device-id0", "client-id": "xiaozhi-client-id0"}
        self.request.headers["client-id"] = client_id
        self.remote_address = ["127.0.0.1"]
        self._messages = asyncio.Queue()
        self._closed = False
        self.mqtt_client = mqtt_client
        self.udp_socket = udp_socket
        self.client_id = client_id
        self.udp_addr = udp_addr
        self.send_audio = send_audio
    async def send(self, data):
        if isinstance(data, str):
            # data = data.encode('utf-8')
            if "websocket" in data: 
                print("ignore websocket") 
                return
            topic_tpl = self.config.get("mqtt", {}).get("downstream_topic_tpl", "devices/down/{}")
            topic = topic_tpl.format(self.client_id)
            self.mqtt_client.publish(topic, data, qos=1)
            print(f"CustomSocket send to client {data}")
        elif isinstance(data, bytes):
            if self.send_audio:
                self.send_audio(self.client_id, data)
            else:
                print("send_audio方法未设置，无法通过UDP下发")
        else:
            print("无效的数据类型")
    async def feed_message(self, msg):
        # print(f"CustomSocket feed_message to client {msg}")
        await self._messages.put(msg)
    async def close(self):
        self._closed = True
    def __aiter__(self):
        return self
    async def __anext__(self):
        if self._closed and self._messages.empty():
            raise StopAsyncIteration
        msg = await self._messages.get()
        return msg

from types import SimpleNamespace
from fastapi import WebSocket
import asyncio
from urllib.parse import parse_qs, urlparse

class WebSocketAdapter:
    def __init__(self, websocket: WebSocket):
        self.websocket = websocket
        self.request = SimpleNamespace()

        # 先复制原 headers（转成可变 dict）
        headers = dict(websocket.headers)

        # 解析 query 参数
        query_params = parse_qs(urlparse(str(websocket.url)).query)

        # 如果 headers 中没有 device-id 或为空，从 query 里取
        if not headers.get("device-id") and "device-id" in query_params:
            headers["device-id"] = query_params["device-id"][0]
        if not headers.get("client-id") and "client-id" in query_params:
            headers["client-id"] = query_params["client-id"][0]

        # 存回 request.headers
        self.request.headers = headers

        self.remote_address = ["127.0.0.1"]
        self._messages = asyncio.Queue()
        self._closed = False
    async def send(self, message):
        if isinstance(message, str):
            print(f"WebSocketAdapter send to client {message}")
            await self.websocket.send_text(message)
        elif isinstance(message, bytes):
            print(f"WebSocketAdapter send to client {len(message)}")
            await self.websocket.send_bytes(message)
        else:
            # 如果是 dict/obj，就转 JSON
            import json
            await self.websocket.send_text(json.dumps(message))
    async def recv(self) -> str:
        message = await self.websocket.receive()
        print(f"WebSocketAdapter recv from client {len(message)}")
        if message["type"] == "websocket.receive":
            if "text" in message:
                print(f"WebSocketAdapter recv from client text {message}")
                # msg = await self.websocket.receive_text()
                msg = message["text"]
                await self._messages.put(msg)
                return msg
            elif "bytes" in message:
                print(f"WebSocketAdapter recv from client bytes ")
                #msg = await self.websocket.receive_bytes()
                msg = message["bytes"]
                await self._messages.put(msg)
                return msg
        elif message["type"] == "websocket.disconnect":
            self._closed = True
    async def close(self, code: int = 1000):
        self._closed = True
        await self.websocket.close(code=code)

    async def feed_message(self, msg):
        await self._messages.put(msg)

    def __aiter__(self):
        return self

    async def __anext__(self):
        if self._closed and self._messages.empty():
            raise StopAsyncIteration
        msg = await self._messages.get()
        return msg
