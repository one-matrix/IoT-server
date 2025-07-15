import asyncio
import json
import os
from typing import List, Optional, Tuple

import aiohttp

from config.logger import setup_logging
from core.providers.asr.base import ASRProviderBase
from core.providers.asr.dto.dto import InterfaceType

TAG = __name__
logger = setup_logging()


class ASRProvider(ASRProviderBase):
    def __init__(self, config: dict, delete_audio_file: bool):
        super().__init__()
        self.interface_type = InterfaceType.NON_STREAM
        self.api_url = config.get("api_url", "http://localhost:5000/api/v1/asr/transcribe")
        self.language = config.get("language", "zh")
        self.model = config.get("model", "funasr")
        self.output_dir = config.get("output_dir", "./audio_output")
        self.delete_audio_file = delete_audio_file

        os.makedirs(self.output_dir, exist_ok=True)

    async def _send_request(self, file_path: str) -> Optional[str]:
        try:
            async with aiohttp.ClientSession() as session:
                with open(file_path, 'rb') as f:
                    print("fa song qing")
                    data = aiohttp.FormData()
                    data.add_field('audio_file', f, filename=os.path.basename(file_path), content_type='audio/wav')
                    data.add_field('language', self.language)
                    data.add_field('model', self.model)

                    async with session.post(self.api_url, data=data) as response:
                        if response.status == 200:
                            body = await response.json()
                            text = body.get("text")
                            logger.bind(tag=TAG).debug(f"ASR 结果: {text}")
                            return text
                        else:
                            error_text = await response.text()
                            logger.bind(tag=TAG).error(f"ASR 失败，状态码: {response.status}, 响应: {error_text}")
                            return None
        except Exception as e:
            logger.bind(tag=TAG).error(f"ASR 请求失败: {e}", exc_info=True)
            return None

    async def speech_to_text(
        self, opus_data: List[bytes], session_id: str, audio_format="opus"
    ) -> Tuple[Optional[str], Optional[str]]:
        file_path = None
        try:
            if audio_format == "pcm":
                pcm_data = opus_data
            else:
                pcm_data = self.decode_opus(opus_data)

            file_path = self.save_audio_to_file(pcm_data, session_id)

            text = await self._send_request(file_path)

            if not self.delete_audio_file:
                pass  # 文件已保存
            else:
                if file_path and os.path.exists(file_path):
                    os.remove(file_path)
                    file_path = None # 文件已删除，路径置空

            if text:
                return text, file_path

            return "", file_path

        except Exception as e:
            logger.bind(tag=TAG).error(f"语音识别失败: {e}", exc_info=True)
            if not self.delete_audio_file and file_path:
                pass
            elif file_path and os.path.exists(file_path):
                os.remove(file_path)
                file_path = None

            return "", file_path