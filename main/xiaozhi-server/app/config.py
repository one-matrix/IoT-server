# config.py
from pydantic_settings import BaseSettings

class Settings(BaseSettings):
    # MQTT
    mqtt_endpoint: str
    mqtt_port: int
    mqtt_sub_topic: str
    mqtt_pub_topic_format: str

    # UDP
    udp_server_host: str
    udp_server_port: int
    udp_public_ip: str # The public IP to send back to the device

    # Session
    session_timeout_seconds: int

    class Config:
        env_file = ".env"

# 创建一个全局可用的配置实例
settings = Settings()