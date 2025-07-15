# udp_server.py

import socket
import struct
import threading
import time
import logging
from typing import Callable, Optional

from cryptography.hazmat.backends import default_backend
from cryptography.hazmat.primitives.ciphers import Cipher, algorithms, modes

from app.session_manager import SessionManager

logger = logging.getLogger(__name__)

def aes_ctr_decrypt(key: bytes, nonce: bytes, ciphertext: bytes) -> bytes:
    """AES-CTR mode decryption function."""
    cipher = Cipher(algorithms.AES(key), modes.CTR(nonce), backend=default_backend())
    decryptor = cipher.decryptor()
    return decryptor.update(ciphertext) + decryptor.finalize()

def aes_ctr_encrypt(key: bytes, nonce: bytes, plaintext: bytes) -> bytes:
    """AES-CTR mode encryption function."""
    cipher = Cipher(algorithms.AES(key), modes.CTR(nonce), backend=default_backend())
    encryptor = cipher.encryptor()
    return encryptor.update(plaintext) + encryptor.finalize()


class UdpServer:
    """Handles UDP communication, including packet listening, encryption, and decryption."""

    def __init__(self, host: str, port: int, session_manager: SessionManager, data_callback: Callable):
        self.host = host
        self.port = port
        self.session_manager = session_manager
        self.data_callback = data_callback # Callback to pass decrypted data to the main logic
        self.socket: Optional[socket.socket] = None
        self.thread: Optional[threading.Thread] = None
        self.is_running = False

    def start(self):
        """Binds the UDP socket and starts the listening thread."""
        try:
            self.socket = socket.socket(socket.AF_INET, socket.SOCK_DGRAM)
            self.socket.bind((self.host, self.port))
            self.is_running = True
            self.thread = threading.Thread(target=self._receive_loop)
            self.thread.daemon = True
            self.thread.start()
            logger.info(f"UDP server listening on {self.host}:{self.port}")
        except Exception as e:
            logger.error(f"Failed to start UDP server: {e}", exc_info=True)
            raise

    def stop(self):
        """Stops the UDP server and closes the socket."""
        self.is_running = False
        if self.socket:
            self.socket.close() # This will cause recvfrom to raise an exception
        if self.thread:
            self.thread.join(timeout=2.0)
        logger.info("UDP server stopped.")

    def _receive_loop(self):
        """The main loop for receiving and processing UDP packets."""
        while self.is_running:
            try:
                data, addr = self.socket.recvfrom(4096)
                if len(data) < 16:
                    logger.error(f"Invalid packet size {len(data)} from {addr}.")
                    continue
                session = self.session_manager.get_session_by_udp_addr(addr)
                client_id = None

                if session:
                    client_id = self.session_manager.get_client_id_by_udp_addr(addr)
                else:
                    # If address is not known, try to find the correct session by attempting decryption
                    unassociated_sessions = self.session_manager.get_unassociated_sessions()
                    for temp_client_id, temp_session in unassociated_sessions.items():
                        try:
                            if len(data) < 16:
                                continue # Not a valid packet to check
                            received_nonce = data[:16]
                            encrypted_audio = data[16:]
                            aes_ctr_decrypt(
                                temp_session["aes_key"], received_nonce, encrypted_audio
                            )
                            # If decryption is successful, we found our client
                            self.session_manager.associate_udp_addr(temp_client_id, addr)
                            session = temp_session
                            client_id = temp_client_id
                            logger.info(f"Successfully associated UDP address {addr} with client '{client_id}'.")
                            break
                        except Exception:
                            # Decryption failed, this is not the correct session
                            continue

                if not session or not client_id:
                    logger.info(f"Discarding packet from unknown/unassociable UDP source {addr}.")
                    continue

                self.session_manager.update_last_seen(client_id)

                # Decrypt packet (if it wasn't already decrypted during association)
                received_nonce = data[:16]
                encrypted_audio = data[16:]

                decrypted_audio = aes_ctr_decrypt(
                    session["aes_key"], received_nonce, encrypted_audio
                )

                # Pass the data to the application logic via callback
                self.data_callback(client_id, decrypted_audio)

            except Exception as e:
                if self.is_running:  # Avoid logging errors during a clean shutdown
                    logger.error(f"UDP receive loop error: {e}", exc_info=True)

    def send_audio(self, client_id: str, audio_data: bytes) -> bool:
        """Encrypts and sends audio data to a specific client."""
        session = self.session_manager.get_session(client_id)
        if not session:
            logger.error(f"Cannot send audio: Session not found for client '{client_id}'.")
            return False
        
        udp_addr = session.get("udp_addr")
        if not udp_addr:
            logger.error(f"Cannot send audio: UDP address unknown for client '{client_id}'.")
            return False

        try:
            session["local_sequence"] = (session["local_sequence"] + 1) & 0xFFFFFFFF
            # UDP Encrypted OPUS Packet Format: 
            # |type 1u|flags 1u|payload_len 2u|ssrc 4u|timestamp 4u|sequence 4u|
            packet_type = 1
            flags = 0
            payload_len = len(audio_data)
            ssrc = int.from_bytes(session["aes_nonce"][:4], 'big')
            timestamp = int(time.time())
            sequence = session["local_sequence"]
            
            nonce_header = struct.pack('!BBHIII', packet_type, flags, payload_len, ssrc, timestamp, sequence)
            
            encrypted_data = aes_ctr_encrypt(session["aes_key"], nonce_header, audio_data)
            
            packet = nonce_header + encrypted_data
            self.socket.sendto(packet, udp_addr)
            
            logger.info(f"send audio {len(packet)} bytes to {udp_addr}.")
            return True
            

        except Exception as e:
            logger.error(f"Failed to send audio to client '{client_id}': {e}", exc_info=True)
            return False