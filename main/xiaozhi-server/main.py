# main.py
import asyncio
from fastapi import FastAPI, HTTPException, Request, WebSocket, WebSocketDisconnect
from fastapi.middleware.cors import CORSMiddleware
from fastapi.staticfiles import StaticFiles
from contextlib import asynccontextmanager
import uvicorn
import time
import uuid
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
    
    # 默认使用manager-api的secret作为auth_key
    # 如果secret为空，则生成随机密钥
    # auth_key用于jwt认证，比如视觉分析接口的jwt认证
    auth_key = config.get("manager-api", {}).get("secret", "")
    if not auth_key or len(auth_key) == 0 or "你" in auth_key:
        auth_key = str(uuid.uuid4().hex)
    config["server"]["auth_key"] = auth_key
    
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
# 添加角色管理器导入
from app.role_manager import role_manager

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

# 添加角色管理API端点
@app.get("/roles", tags=["Roles"])
async def list_roles():
    """列出所有可用的角色"""
    try:
        roles = role_manager.list_roles()
        return {"roles": list(roles.keys())}
    except Exception as e:
        logger.error(f"获取角色列表失败: {e}")
        raise HTTPException(status_code=500, detail="获取角色列表失败")

@app.get("/roles/{role_name}", tags=["Roles"])
async def get_role(role_name: str):
    """获取指定角色的详细信息"""
    try:
        role = role_manager.get_role(role_name)
        if not role:
            raise HTTPException(status_code=404, detail=f"角色 {role_name} 不存在")
        return {"role": role_name, "config": role}
    except HTTPException:
        raise
    except Exception as e:
        logger.error(f"获取角色信息失败: {e}")
        raise HTTPException(status_code=500, detail="获取角色信息失败")

@app.post("/roles/{role_name}/switch", tags=["Roles"])
async def switch_role(role_name: str):
    """切换到指定角色"""
    try:
        role = role_manager.get_role(role_name)
        if not role:
            raise HTTPException(status_code=404, detail=f"角色 {role_name} 不存在")
        
        # 这里可以添加切换所有连接的角色的逻辑
        # 或者返回成功信息让客户端重新连接时指定角色
        return {"message": f"成功切换到角色 {role_name}", "role": role}
    except HTTPException:
        raise
    except Exception as e:
        logger.error(f"切换角色失败: {e}")
        raise HTTPException(status_code=500, detail="切换角色失败")

# 添加WebSocket参数支持角色切换
# #ws://localhost:38005/yzy/ws/sleep_assistant copd_assistant
# @app.websocket("/yzy/ws/{role_name}")
# async def websocket_endpoint_with_role(websocket: WebSocket, role_name: str = None):
#     """支持指定角色的WebSocket端点"""
#     # 创建ConnectionHandler实例
#     print(f"正在处理角色 {ws_config} 的 WebSocket 连接")
#     handler = ConnectionHandler(
#         ws_config,
#         ws_modules.get("vad"),
#         ws_modules.get("asr"),
#         ws_modules.get("llm"),
#         ws_modules.get("memory"),
#         ws_modules.get("intent"),
#         None  # 不传入server实例
#     )
    
#     # 如果指定了角色，尝试切换
#     if role_name:
#         if not handler.switch_role(role_name):
#             await websocket.close(code=4000, reason=f"角色 {role_name} 不存在")
#             return
    
#     ws_active_connections.add(handler)
#     await websocket.accept()
#     ws = WebSocketAdapter(websocket)  # 适配成类似 websockets 库的接口
#     websocket.headers.get("authorization")
#     try:
#         asyncio.create_task(handler.handle_connection(ws))
#         while True:
#             msg = await ws.recv()  # 类似 websockets.recv()
#             # print(f"收到: {msg}")
#             #await ws.send(f"回显: {msg}")  # 类似 websockets.send()
#     except WebSocketDisconnect:
#         print("客户端断开连接")
#         await ws.close()
#     except Exception as e:
#         logger.error(f"处理WebSocket连接时出错: {e}")
#     finally:
#         # 从活动连接集合中移除
#         ws_active_connections.discard(handler)

if __name__ == "__main__":
    # Ensure a directory for recordings exists
    uvicorn.run(app, host="0.0.0.0", port=38005)