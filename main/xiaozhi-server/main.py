# main.py
import asyncio
from fastapi import FastAPI, HTTPException,Request
from fastapi.middleware.cors import CORSMiddleware
from contextlib import asynccontextmanager
import uvicorn
import time 
from config import settings
from app.mqtt_client import mqtt_client
# from app.udp_server import UdpServerProtocol
from app.session_manager import session_manager
from app.mqtt_server import MqttServer
from config.logger import setup_logging
logger = setup_logging()
from config.settings import load_config
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
    yield
    # Shutdown logic
    # mqtt_client.stop()
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
            "url": "wss://api.tenclass.net/xiaozhi/v1/",
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
if __name__ == "__main__":
    # Ensure a directory for recordings exists
    uvicorn.run(app, host="0.0.0.0", port=8000)