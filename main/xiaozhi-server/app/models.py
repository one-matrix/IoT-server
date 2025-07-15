# models.py
from pydantic import BaseModel, Field
from typing import Dict, Any, Literal
import uuid

# --- Incoming Messages (Device -> Server) ---
class AudioParams(BaseModel):
    # 示例参数，根据你的实际需求修改
    sample_rate: int = 16000
    channels: int = 1
    sample_width: int = 2

class HelloMessage(BaseModel):
    type: Literal["hello"]
    version: int
    audio_params: AudioParams = Field(default_factory=AudioParams)
    features: Dict[str, Any] = Field(default_factory=dict)

# --- Outgoing Messages (Server -> Device) ---

class UdpTransportInfo(BaseModel):
    server: str
    port: int
    encryption: Literal["aes-128-ctr"]
    key: str  # Hex-encoded
    nonce: str # Hex-encoded

class HelloResponse(BaseModel):
    type: Literal["hello"]
    version: int
    session_id: uuid.UUID
    transport: Literal["udp"]
    udp: UdpTransportInfo
    audio_params: AudioParams # 可以是服务器确认或修改后的参数