# main.py
import asyncio
from fastapi import FastAPI, HTTPException, Request, WebSocket, WebSocketDisconnect
from fastapi.middleware.cors import CORSMiddleware
from fastapi.staticfiles import StaticFiles
from contextlib import asynccontextmanager
import uvicorn
import time

from websockets import client 
from config import settings
from app.mqtt_client import mqtt_client
# from app.udp_server import UdpServerProtocol
from app.session_manager import session_manager
from app.mqtt_server import MqttServer
from config.logger import setup_logging
from config.settings import load_config
from config.config_loader import get_config_from_api
from core.utils.modules_initialize import initialize_modules
from core.connection import ConnectionHandler
from core.utils.util import check_vad_update, check_asr_update
#from core.websocket_server import WebSocketServer
logger = setup_logging()
config = load_config()

# 初始化 FastAPI 应用
@asynccontextmanager
async def lifespan(app: FastAPI):
    # Startup logic
    # mqtt_client.connect()
    # mqtt_client.start()
   
    loop = asyncio.get_running_loop()
    server = MqttServer(loop)
    try:
        server.start()
        # auth_key = config.get("manager-api", {}).get("secret", "")
        # if not auth_key or len(auth_key) == 0 or "你" in auth_key:
        #     auth_key = str(uuid.uuid4().hex)
        # config["server"]["auth_key"] = auth_key
        # # 启动 WebSocket 服务器
        # ws_server = WebSocketServer(config)
        # ws_task = asyncio.create_task(ws_server.start())
        # 保持主线程运行
    except KeyboardInterrupt:
        logger.info("收到关闭信号...")
    except Exception as e:
        logger.error(f"服务器启动或运行期间发生严重错误: {e}", exc_info=True)
    # loop.create_datagram_endpoint(
    #     lambda: UdpServerProtocol(),
    #     local_addr=(settings.udp_server_host, settings.udp_server_port)
    # )
    #asyncio.create_task(run_session_cleanup())
    
    # 初始化WebSocketServer所需的组件
    global ws_modules
    ws_modules = initialize_modules(
        logger,
        config,
        "VAD" in config["selected_module"],
        "ASR" in config["selected_module"],
        "LLM" in config["selected_module"],
        False,
        "Memory" in config["selected_module"],
        "Intent" in config["selected_module"],
    )
    
    yield
    # Shutdown logic
    # mqtt_client.stop()
    #ws_task.cancel()
    server.stop()
    print("Application shutting down.")

app = FastAPI(
    title="IoT Audio Bridge",
    description="A bridge service for MQTT control and UDP audio streaming.",
    version="1.0.0",
    lifespan=lifespan
)
# 添加CORS中间件
app.add_middleware(
    CORSMiddleware,
    allow_origins=["*"],
    allow_credentials=True,
    allow_methods=["*"],
    allow_headers=["*"],
)

# 挂载静态文件目录（例如 "static" 里放 HTML、CSS、JS）
app.mount("/ui", StaticFiles(directory="app/ui"), name="ui")
# 全局变量，用于存储WebSocketServer的配置和组件
ws_config = config
ws_modules = {}
ws_active_connections = set()
ws_config_lock = asyncio.Lock()

# --- Background Tasks ---
async def run_session_cleanup():
    """定期清理过期会话的后台任务。"""
    while True:
        await asyncio.sleep(60) # 每60秒检查一次
        session_manager.cleanup_stale_sessions()

# --- API Endpoints (for monitoring/debugging) ---
@app.get("/", tags=["Status"])
async def read_root():
    """根路径，用于简单的健康检查。"""
    return {"status": "IoT Audio Bridge is running"}

@app.get("/sessions", tags=["Monitoring"])
async def get_active_sessions():
    """查看当前所有活跃会话。"""
    # 注意：返回的数据不应包含敏感信息如key/nonce
    active_sessions = {
        str(sid): {
            "device_id": session.device_id,
            "last_seen_ago_s": int(time.time() - session.last_seen)
        }
        for sid, session in session_manager.sessions.items()
    }
    return active_sessions

@app.post("/yzy/ota", tags=["OTA"])
async def ota_info(request: Request):
    headers = request.headers
    device_id = headers.get("device-id")
    client_id = headers.get("client-id","c2")
    # 解析请求体（如有需要）
    #body = await request.json() if request.headers.get("content-type", "").startswith("application/json") else None
    now = int(time.time() * 1000)
    timezone_offset = 480
    mqtt_config = config.get("mqtt", {})
    return {
        "mqtt": {
            "endpoint": mqtt_config.get("endpoint", "api.shyunzhiyi.cn"),
            "port": mqtt_config.get("port", "1883"),
            "client_id":f"{client_id}",
            "username": mqtt_config.get("username", "client_a"),
            "password": mqtt_config.get("password", "J4h58G8l"),
            "publish_topic": f"devices/up/{client_id}",
            "subscribe_topic": f"devices/down/{client_id}"
        },
        "websocket": {
            "url": "ws://127.0.0.1:38030/yzy/ws/",
            "token": "test-token"
        },
        "server_time": {
            "timestamp": now,
            "timezone_offset": timezone_offset
        },
        "firmware": {
            "version": "2.0.0",
            "url": ""
        },
        "parsed_headers": {
            "Device-Id": device_id,
            "Client-Id": client_id
        },
    }
from app.mqtt_gateway import WebSocketAdapter 
# --- WebSocket Endpoint ---
@app.websocket("/yzy/ws")
async def websocket_endpoint(websocket: WebSocket):
    """WebSocket端点，处理客户端连接"""
    # 创建ConnectionHandler实例
    handler = ConnectionHandler(
        ws_config,
        ws_modules.get("vad"),
        ws_modules.get("asr"),
        ws_modules.get("llm"),
        ws_modules.get("memory"),
        ws_modules.get("intent"),
        None  # 不传入server实例
    )
    ws_active_connections.add(handler)
    await websocket.accept()
    ws = WebSocketAdapter(websocket)  # 适配成类似 websockets 库的接口
    websocket.headers.get("authorization")
    try:
        asyncio.create_task(handler.handle_connection(ws))
        while True:
            msg = await ws.recv()  # 类似 websockets.recv()
            # print(f"收到: {msg}")
            #await ws.send(f"回显: {msg}")  # 类似 websockets.send()
    except WebSocketDisconnect:
        print("客户端断开连接")
        await ws.close()
    except Exception as e:
        logger.error(f"处理WebSocket连接时出错: {e}")
    finally:
        # 从活动连接集合中移除
        ws_active_connections.discard(handler)
# # --- WebSocket Server Config Update ---
# async def update_ws_config() -> bool:
#     """更新WebSocket服务器配置并重新初始化组件

#     Returns:
#         bool: 更新是否成功
#     """
#     try:
#         async with ws_config_lock:
#             # 重新获取配置
#             new_config = get_config_from_api(ws_config)
#             if new_config is None:
#                 logger.error("获取新配置失败")
#                 return False
#             logger.info(f"获取新配置成功")
#             # 检查 VAD 和 ASR 类型是否需要更新
#             update_vad = check_vad_update(ws_config, new_config)
#             update_asr = check_asr_update(ws_config, new_config)
#             logger.info(f"检查VAD和ASR类型是否需要更新: {update_vad} {update_asr}")
#             # 更新配置
#             global ws_config
#             ws_config = new_config
#             # 重新初始化组件
#             global ws_modules
#             ws_modules = initialize_modules(
#                 logger,
#                 new_config,
#                 update_vad,
#                 update_asr,
#                 "LLM" in new_config["selected_module"],
#                 False,
#                 "Memory" in new_config["selected_module"],
#                 "Intent" in new_config["selected_module"],
#             )
#             logger.info(f"更新配置任务执行完毕")
#             return True
#     except Exception as e:
#         logger.error(f"更新服务器配置失败: {str(e)}")
#         return False

if __name__ == "__main__":
    # Ensure a directory for recordings exists
    uvicorn.run(app, host="0.0.0.0", port=38005)