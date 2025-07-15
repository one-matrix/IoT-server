# session_manager.py

import os
import time
import uuid
import logging
from typing import Dict, Any, Optional, Tuple

from app.config import settings

logger = logging.getLogger(__name__)


class SessionManager:
    """Manages client sessions, including their state and cryptographic keys."""

    def __init__(self):
        """Initializes the SessionManager."""
        self._sessions: Dict[str, Dict[str, Any]] = {}
        self._udp_to_client_id: Dict[Tuple[str, int], str] = {}

    def create_session(self, client_id: str, audio_params: Dict[str, Any]) -> Dict[str, Any]:
        """处理客户端的hello请求，创建新会话"""
        session_id = str(uuid.uuid4())
        aes_key = os.urandom(32)  # 256-bit key
        aes_nonce = os.urandom(16)  # 128-bit nonce

        session = {
            "session_id": session_id,
            "client_id": client_id,
            "udp_addr": None,  # 客户端的UDP地址未知，将在收到第一个UDP包后更新
            "aes_key": aes_key,
            "aes_nonce": aes_nonce,
            "local_sequence": 0,  # 服务端发送的序列号
            "remote_sequence": 0,  # 客户端发送的序列号
            "audio_params": audio_params,
            "last_seen": time.time()
        }
        self._sessions[client_id] = session

        logger.info(f"为客户端 '{client_id}' 创建了新会话: {session_id}")
        return session

    def delete_session(self, client_id: str, session_id: str=None) -> bool:
        """处理客户端的goodbye请求，清理会话"""
        if client_id in self._sessions:
            if self._sessions[client_id]["session_id"] == session_id or session_id is None:
                # Clean up UDP mapping as well
                udp_addr = self._sessions[client_id].get("udp_addr")
                if udp_addr and udp_addr in self._udp_to_client_id:
                    del self._udp_to_client_id[udp_addr]
                del self._sessions[client_id]
                logger.info(f"已清理客户端 '{client_id}' 的会话 (session: {session_id})")
                return True
            else:
                logger.warning(f"客户端 '{client_id}' 的goodbye消息session_id不匹配。")
                return False
        else:
            logger.warning(f"收到来自未知客户端 '{client_id}' 的goodbye消息。")
            return False

    def get_session(self, client_id: str) -> Optional[Dict[str, Any]]:
        """Gets a session by client ID."""
        return self._sessions.get(client_id)

    def get_session_by_udp_addr(self, addr: Tuple[str, int]) -> Optional[Dict[str, Any]]:
        """Gets a session by UDP address."""
        client_id = self._udp_to_client_id.get(addr)
        if client_id:
            return self.get_session(client_id)
        return None

    def get_client_id_by_udp_addr(self, addr: Tuple[str, int]) -> Optional[str]:
        """Gets a client ID by UDP address."""
        return self._udp_to_client_id.get(addr)

    def update_last_seen(self, client_id: str):
        """Updates the last seen timestamp for a session."""
        session = self.get_session(client_id)
        if session:
            session["last_seen"] = time.time()

    def associate_udp_addr(self, client_id: str, addr: Tuple[str, int]):
        """Associates a UDP address with a client's session."""
        session = self.get_session(client_id)
        if session:
            session["udp_addr"] = addr
            self._udp_to_client_id[addr] = client_id
            logger.info(f"Associated UDP address {addr} with client '{client_id}'.")
            return True
        return False

    def get_unassociated_sessions(self) -> Dict[str, Dict[str, Any]]:
        """Returns all sessions that do not have a UDP address yet."""
        return {cid: s for cid, s in self._sessions.items() if s.get("udp_addr") is None}

    def cleanup_stale_sessions(self, stale_time: int = 300):
        """清理长时间不活跃的会话 (心跳检测)。"""
        now = time.time()
        stale_sessions = [
            sid for sid, session in self._sessions.items()
            if now - session.get("last_seen", 0) > stale_time
        ]
        for sid in stale_sessions:
            logger.info(f"Cleaning up stale session for client '{sid}'.")
            self.delete_session(sid, self._sessions[sid]['session_id'])
 
# 创建一个全局会d的会话管理器实例
session_manager = SessionManager()