import httpx
from core.providers.tts.base import TTSProviderBase
from config.logger import setup_logging

logger = setup_logging()

class TTSProvider(TTSProviderBase):

    def __init__(self, config, delete_audio_file):
        super().__init__(config, delete_audio_file)
        self.api_url = config.get("api_url", "")
        self.voice = config.get("voice", "zh-CN-XiaoxiaoNeural")
        self.model = config.get("model", "cosyvoice")
        self.language = config.get("language", "zh")
        self.pitch = config.get("pitch", 1)
        self.speed = config.get("speed", 1)
 
    async def text_to_speak(self, text, output_file):
        # If API URL is not configured, 默认让发送LLM返回的文本，但不生成语音
        if self.api_url=="":
            # When API URL is not configured, generate silent audio data
            # Create minimal valid WAV audio data (silent)
            silent_wav = (
                b'RIFF$\x00\x00\x00WAVEfmt \x10\x00\x00\x00\x01\x00\x01\x00D\xac'
                b'\x00\x00\x88X\x01\x00\x02\x00\x10\x00data\x00\x00\x00\x00'
            )
            
            if output_file:
                # Write silent WAV data to file
                with open(output_file, 'wb') as f:
                    f.write(silent_wav)
                return output_file
            else:
                # Return silent WAV data
                return silent_wav
                
        payload = {
            "language": self.language,
            "model": self.model,
            "pitch": self.pitch,
            "speed": self.speed,
            "text": text,
            "voice": self.voice
        }
        headers = {
            'accept': 'application/json',
            'Content-Type': 'application/json'
        }

        try:
            async with httpx.AsyncClient(timeout=20.0) as client:
                async with client.stream("POST", self.api_url, json=payload, headers=headers) as response:
                    if response.status_code != 200:
                        try:
                            error_details = await response.aread()
                            logger.error(f"TTS API returned error: {response.status_code}, details: {error_details.decode()}")
                        except Exception:
                            logger.error(f"TTS API returned error: {response.status_code}, could not read details.")
                    response.raise_for_status()

                    if output_file:
                        with open(output_file, 'wb') as f:
                            async for chunk in response.aiter_bytes():
                                f.write(chunk)
                        return output_file
                    else:
                        return await response.aread()

        except httpx.HTTPStatusError as e:
            logger.error(f"HTTP error occurred: {e.response.status_code}", exc_info=True)
            raise
        except httpx.RequestError as e:
            logger.error(f"Request error occurred: {e}", exc_info=True)
            raise
        except Exception as e:
            logger.error(f"An unexpected error occurred in yzy_tts", exc_info=True)
            raise