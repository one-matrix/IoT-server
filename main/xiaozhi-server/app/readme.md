改动点：
main\xiaozhi-server\app
main\xiaozhi-server\core\providers\asr\yzy_asr.py
main\xiaozhi-server\core\providers\tts\yzy_tts.py
main\xiaozhi-server\README1.md
main\xiaozhi-server\data\.config.yaml

改动：
main\xiaozhi-server\core\connection.py
        session_id=None,
    ):
        self.common_config = config
        self.config = copy.deepcopy(config)
        
        self.session_id = session_id
        if not session_id:
            self.session_id = str(uuid.uuid4())
   
config.myl 增加：
mqtt:
  broker_host: "api.shyunzhiyi.cn"
  broker_port: 1883
  username: "client_a"
  password: "J4h58G8l"
  upstream_topic: "devices/up/+"
  downstream_topic_tpl: "devices/down/{}"
  server_client_id_prefix: "server-"

udp:
  host: "0.0.0.0"
  port: 12345
  server_public_ip: "127.0.0.1"
  

fastapi
uvicorn[standard]
paho-mqtt
pydantic
pydantic-settings
cryptography
python-dotenv
aiortc
paho-mqtt

https://github.com/78/xiaozhi-mqtt-gateway/blob/c5e3235df8db8f06d1710074ec10e870159e0844/app.js#L350

该项目是一个开发一个智能音箱后台，提供一个完整且可运行的 Python FastAPI 项目骨架，该骨架实现了您描述的所有核心功能：MQTT 握手，和传输命令，会话开启结束等。会话管理、UDP音频流接收和配置管理。并建立一个网页用于测试。智能音箱与服务器之间的双向语音传输

提供一个完整且可运行的 Python FastAPI 项目骨架，该骨架实现了您描述的所有核心功能：MQTT 握手、会话管理、UDP 音频流接收和配置管理。
项目目标
创建一个服务端应用程序，它：

监听 MQTT 主题：等待新设备连接并发送 hello 消息。

管理会话：为每个成功连接的设备创建一个唯一的会话，并生成加密参数。

响应设备：通过 MQTT 将会话 ID 和 UDP 连接信息（IP、端口、加密密钥）返回给设备。

运行 UDP 服务器：接收加密的音频数据包。

验证和处理音频：根据会话 ID 查找会话信息，解密并处理传入的音频数据。

技术选型
Web 框架 & 应用服务器: FastAPI + Uvicorn (用于管理后台任务和可能的 HTTP API)

MQTT 客户端: paho-mqtt (最流行和稳定的 Python MQTT 库)

配置管理: pydantic-settings (用于从环境变量加载配置)

数据校验: Pydantic (FastAPI 内置，用于消息格式校验)

加密: cryptography (用于 AES-CTR 加密/解密)

异步处理: asyncio (Python 原生库，用于构建高性能的 UDP 服务器)