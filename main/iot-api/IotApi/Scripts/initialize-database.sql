-- Master Database Initialization Script
-- This script combines all initial data insertion scripts
-- Run this script to initialize the database with all required initial data

-- Initialize Model Providers
-- --------------------------

-- Delete existing data (for clean re-insertion)
DELETE FROM ai_model_provider;

-- Insert VAD Model Providers
INSERT INTO ai_model_provider (id, model_type, provider_code, name, fields, sort, creator, create_date, updater, update_date) VALUES
('SYSTEM_VAD_SileroVAD', 'VAD', 'silero', 'SileroVAD语音活动检测', '[{"key":"threshold","label":"检测阈值","type":"number"},{"key":"model_dir","label":"模型目录","type":"string"},{"key":"min_silence_duration_ms","label":"最小静音时长","type":"number"}]', 1, 1, NOW(), 1, NOW());

-- Insert ASR Model Providers
INSERT INTO ai_model_provider (id, model_type, provider_code, name, fields, sort, creator, create_date, updater, update_date) VALUES
('SYSTEM_ASR_FunASR', 'ASR', 'fun_local', 'FunASR语音识别', '[{"key":"model_dir","label":"模型目录","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"}]', 1, 1, NOW(), 1, NOW()),
('SYSTEM_ASR_SherpaASR', 'ASR', 'sherpa_onnx_local', 'SherpaASR语音识别', '[{"key":"model_dir","label":"模型目录","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"}]', 2, 1, NOW(), 1, NOW()),
('SYSTEM_ASR_DoubaoASR', 'ASR', 'doubao', '火山引擎语音识别', '[{"key":"appid","label":"应用ID","type":"string"},{"key":"access_token","label":"访问令牌","type":"string"},{"key":"cluster","label":"集群","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"}]', 3, 1, NOW(), 1, NOW());

-- Insert LLM Model Providers
INSERT INTO ai_model_provider (id, model_type, provider_code, name, fields, sort, creator, create_date, updater, update_date) VALUES
('SYSTEM_LLM_openai', 'LLM', 'openai', 'OpenAI接口', '[{"key":"base_url","label":"基础URL","type":"string"},{"key":"model_name","label":"模型名称","type":"string"},{"key":"api_key","label":"API密钥","type":"string"},{"key":"temperature","label":"温度","type":"number"},{"key":"max_tokens","label":"最大令牌数","type":"number"},{"key":"top_p","label":"top_p值","type":"number"},{"key":"top_k","label":"top_k值","type":"number"},{"key":"frequency_penalty","label":"频率惩罚","type":"number"}]', 1, 1, NOW(), 1, NOW()),
('SYSTEM_LLM_AliBL', 'LLM', 'AliBL', '阿里百炼接口', '[{"key":"base_url","label":"基础URL","type":"string"},{"key":"app_id","label":"应用ID","type":"string"},{"key":"api_key","label":"API密钥","type":"string"},{"key":"is_no_prompt","label":"是否不使用本地prompt","type":"boolean"},{"key":"ali_memory_id","label":"记忆ID","type":"string"}]', 2, 1, NOW(), 1, NOW()),
('SYSTEM_LLM_ollama', 'LLM', 'ollama', 'Ollama接口', '[{"key":"model_name","label":"模型名称","type":"string"},{"key":"base_url","label":"服务地址","type":"string"}]', 3, 1, NOW(), 1, NOW()),
('SYSTEM_LLM_dify', 'LLM', 'dify', 'Dify接口', '[{"key":"base_url","label":"基础URL","type":"string"},{"key":"api_key","label":"API密钥","type":"string"},{"key":"mode","label":"对话模式","type":"string"}]', 4, 1, NOW(), 1, NOW()),
('SYSTEM_LLM_gemini', 'LLM', 'gemini', 'Gemini接口', '[{"key":"api_key","label":"API密钥","type":"string"},{"key":"model_name","label":"模型名称","type":"string"},{"key":"http_proxy","label":"HTTP代理","type":"string"},{"key":"https_proxy","label":"HTTPS代理","type":"string"}]', 5, 1, NOW(), 1, NOW()),
('SYSTEM_LLM_coze', 'LLM', 'coze', 'Coze接口', '[{"key":"bot_id","label":"机器人ID","type":"string"},{"key":"user_id","label":"用户ID","type":"string"},{"key":"personal_access_token","label":"个人访问令牌","type":"string"}]', 6, 1, NOW(), 1, NOW()),
('SYSTEM_LLM_fastgpt', 'LLM', 'fastgpt', 'FastGPT接口', '[{"key":"base_url","label":"基础URL","type":"string"},{"key":"api_key","label":"API密钥","type":"string"},{"key":"variables","label":"变量","type":"dict","dict_name":"variables"}]', 7, 1, NOW(), 1, NOW()),
('SYSTEM_LLM_xinference', 'LLM', 'xinference', 'Xinference接口', '[{"key":"model_name","label":"模型名称","type":"string"},{"key":"base_url","label":"服务地址","type":"string"}]', 8, 1, NOW(), 1, NOW()),
('SYSTEM_LLM_doubao', 'LLM', 'doubao', '火山引擎LLM', '[{"key":"base_url","label":"基础URL","type":"string"},{"key":"model_name","label":"模型名称","type":"string"},{"key":"api_key","label":"API密钥","type":"string"}]', 9, 1, NOW(), 1, NOW()),
('SYSTEM_LLM_chatglm', 'LLM', 'chatglm', 'ChatGLM接口', '[{"key":"model_name","label":"模型名称","type":"string"},{"key":"url","label":"服务地址","type":"string"},{"key":"api_key","label":"API密钥","type":"string"}]', 10, 1, NOW(), 1, NOW());

-- Insert TTS Model Providers
INSERT INTO ai_model_provider (id, model_type, provider_code, name, fields, sort, creator, create_date, updater, update_date) VALUES
('SYSTEM_TTS_edge', 'TTS', 'edge', 'Edge TTS', '[{"key":"voice","label":"音色","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"}]', 1, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_doubao', 'TTS', 'doubao', '火山引擎TTS', '[{"key":"api_url","label":"API地址","type":"string"},{"key":"voice","label":"音色","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"},{"key":"authorization","label":"授权","type":"string"},{"key":"appid","label":"应用ID","type":"string"},{"key":"access_token","label":"访问令牌","type":"string"},{"key":"cluster","label":"集群","type":"string"}]', 2, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_siliconflow', 'TTS', 'siliconflow', '硅基流动TTS', '[{"key":"model","label":"模型","type":"string"},{"key":"voice","label":"音色","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"},{"key":"access_token","label":"访问令牌","type":"string"},{"key":"response_format","label":"响应格式","type":"string"}]', 3, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_cozecn', 'TTS', 'cozecn', 'COZECN TTS', '[{"key":"voice","label":"音色","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"},{"key":"access_token","label":"访问令牌","type":"string"},{"key":"response_format","label":"响应格式","type":"string"}]', 4, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_fishspeech', 'TTS', 'fishspeech', 'FishSpeech TTS', '[{"key":"output_dir","label":"输出目录","type":"string"},{"key":"response_format","label":"响应格式","type":"string"},{"key":"reference_id","label":"参考ID","type":"string"},{"key":"reference_audio","label":"参考音频","type":"dict","dict_name":"reference_audio"},{"key":"reference_text","label":"参考文本","type":"dict","dict_name":"reference_text"},{"key":"normalize","label":"是否标准化","type":"boolean"},{"key":"max_new_tokens","label":"最大新令牌数","type":"number"},{"key":"chunk_length","label":"块长度","type":"number"},{"key":"top_p","label":"top_p值","type":"number"},{"key":"repetition_penalty","label":"重复惩罚","type":"number"},{"key":"temperature","label":"温度","type":"number"},{"key":"streaming","label":"是否流式","type":"boolean"},{"key":"use_memory_cache","label":"是否使用内存缓存","type":"string"},{"key":"seed","label":"种子","type":"number"},{"key":"channels","label":"通道数","type":"number"},{"key":"rate","label":"采样率","type":"number"},{"key":"api_key","label":"API密钥","type":"string"},{"key":"api_url","label":"API地址","type":"string"}]', 5, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_gpt_sovits_v2', 'TTS', 'gpt_sovits_v2', 'GPT-SoVITS V2', '[{"key":"url","label":"服务地址","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"},{"key":"text_lang","label":"文本语言","type":"string"},{"key":"ref_audio_path","label":"参考音频路径","type":"string"},{"key":"prompt_text","label":"提示文本","type":"string"},{"key":"prompt_lang","label":"提示语言","type":"string"},{"key":"top_k","label":"top_k值","type":"number"},{"key":"top_p","label":"top_p值","type":"number"},{"key":"temperature","label":"温度","type":"number"},{"key":"text_split_method","label":"文本分割方法","type":"string"},{"key":"batch_size","label":"批处理大小","type":"number"},{"key":"batch_threshold","label":"批处理阈值","type":"number"},{"key":"split_bucket","label":"是否分桶","type":"boolean"},{"key":"return_fragment","label":"是否返回片段","type":"boolean"},{"key":"speed_factor","label":"速度因子","type":"number"},{"key":"streaming_mode","label":"是否流式模式","type":"boolean"},{"key":"seed","label":"种子","type":"number"},{"key":"parallel_infer","label":"是否并行推理","type":"boolean"},{"key":"repetition_penalty","label":"重复惩罚","type":"number"},{"key":"aux_ref_audio_paths","label":"辅助参考音频路径","type":"dict","dict_name":"aux_ref_audio_paths"}]', 6, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_gpt_sovits_v3', 'TTS', 'gpt_sovits_v3', 'GPT-SoVITS V3', '[{"key":"url","label":"服务地址","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"},{"key":"text_language","label":"文本语言","type":"string"},{"key":"refer_wav_path","label":"参考音频路径","type":"string"},{"key":"prompt_language","label":"提示语言","type":"string"},{"key":"prompt_text","label":"提示文本","type":"string"},{"key":"top_k","label":"top_k值","type":"number"},{"key":"top_p","label":"top_p值","type":"number"},{"key":"temperature","label":"温度","type":"number"},{"key":"cut_punc","label":"切分标点","type":"string"},{"key":"speed","label":"速度","type":"number"},{"key":"inp_refs","label":"输入参考","type":"dict","dict_name":"inp_refs"},{"key":"sample_steps","label":"采样步数","type":"number"},{"key":"if_sr","label":"是否使用SR","type":"boolean"}]', 7, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_minimax', 'TTS', 'minimax', 'Minimax TTS', '[{"key":"output_dir","label":"输出目录","type":"string"},{"key":"group_id","label":"组ID","type":"string"},{"key":"api_key","label":"API密钥","type":"string"},{"key":"model","label":"模型","type":"string"},{"key":"voice_id","label":"音色ID","type":"string"}]', 8, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_aliyun', 'TTS', 'aliyun', '阿里云TTS', '[{"key":"output_dir","label":"输出目录","type":"string"},{"key":"appkey","label":"应用密钥","type":"string"},{"key":"token","label":"访问令牌","type":"string"},{"key":"voice","label":"音色","type":"string"},{"key":"access_key_id","label":"访问密钥ID","type":"string"},{"key":"access_key_secret","label":"访问密钥密码","type":"string"}]', 9, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_ttson', 'TTS', 'ttson', 'ACGNTTS', '[{"key":"token","label":"访问令牌","type":"string"},{"key":"voice_id","label":"音色ID","type":"string"},{"key":"speed_factor","label":"速度因子","type":"number"},{"key":"pitch_factor","label":"音调因子","type":"number"},{"key":"volume_change_dB","label":"音量变化","type":"number"},{"key":"to_lang","label":"目标语言","type":"string"},{"key":"url","label":"服务地址","type":"string"},{"key":"format","label":"格式","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"},{"key":"emotion","label":"情感","type":"number"}]', 10, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_openai', 'TTS', 'openai', 'OpenAI TTS', '[{"key":"api_key","label":"API密钥","type":"string"},{"key":"api_url","label":"API地址","type":"string"},{"key":"model","label":"模型","type":"string"},{"key":"voice","label":"音色","type":"string"},{"key":"speed","label":"速度","type":"number"},{"key":"output_dir","label":"输出目录","type":"string"}]', 11, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_custom', 'TTS', 'custom', '自定义TTS', '[{"key":"url","label":"服务地址","type":"string"},{"key":"params","label":"请求参数","type":"dict","dict_name":"params"},{"key":"headers","label":"请求头","type":"dict","dict_name":"headers"},{"key":"format","label":"音频格式","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"}]', 12, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_302ai', 'TTS', '302ai', '302AI TTS', '[{"key":"api_url","label":"API地址","type":"string"},{"key":"authorization","label":"授权","type":"string"},{"key":"voice","label":"音色","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"},{"key":"access_token","label":"访问令牌","type":"string"}]', 13, 1, NOW(), 1, NOW()),
('SYSTEM_TTS_gizwits', 'TTS', 'gizwits', '机智云TTS', '[{"key":"api_url","label":"API地址","type":"string"},{"key":"authorization","label":"授权","type":"string"},{"key":"voice","label":"音色","type":"string"},{"key":"output_dir","label":"输出目录","type":"string"},{"key":"access_token","label":"访问令牌","type":"string"}]', 14, 1, NOW(), 1, NOW());

-- Insert Memory Model Providers
INSERT INTO ai_model_provider (id, model_type, provider_code, name, fields, sort, creator, create_date, updater, update_date) VALUES
('SYSTEM_Memory_mem0ai', 'Memory', 'mem0ai', 'Mem0AI记忆', '[{"key":"api_key","label":"API密钥","type":"string"}]', 1, 1, NOW(), 1, NOW()),
('SYSTEM_Memory_nomem', 'Memory', 'nomem', '无记忆', '[]', 2, 1, NOW(), 1, NOW()),
('SYSTEM_Memory_mem_local_short', 'Memory', 'mem_local_short', '本地短记忆', '[]', 3, 1, NOW(), 1, NOW());

-- Insert Intent Model Providers
INSERT INTO ai_model_provider (id, model_type, provider_code, name, fields, sort, creator, create_date, updater, update_date) VALUES
('SYSTEM_Intent_nointent', 'Intent', 'nointent', '无意图识别', '[]', 1, 1, NOW(), 1, NOW()),
('SYSTEM_Intent_intent_llm', 'Intent', 'intent_llm', 'LLM意图识别', '[{"key":"llm","label":"LLM模型","type":"string"}]', 2, 1, NOW(), 1, NOW()),
('SYSTEM_Intent_function_call', 'Intent', 'function_call', '函数调用意图识别', '[{"key":"functions","label":"函数列表","type":"dict","dict_name":"functions"}]', 3, 1, NOW(), 1, NOW());

-- Initialize Dictionary Types and Data
-- -----------------------------------

-- Initialize Firmware Types Dictionary
-- Delete existing firmware type data
DELETE FROM sys_dict_type WHERE id = 101;
DELETE FROM sys_dict_data WHERE dict_type_id = 101;

-- Insert firmware type dictionary type
INSERT INTO sys_dict_type (id, dict_type, dict_name, remark, sort, creator, create_date, updater, update_date) VALUES 
(101, 'FIRMWARE_TYPE', '固件类型', '固件类型字典', 0, 1, NOW(), 1, NOW());

-- Insert firmware type dictionary data
INSERT INTO sys_dict_data (id, dict_type_id, dict_label, dict_value, remark, sort, creator, create_date, updater, update_date) VALUES 
(101001, 101, '面包板新版接线（WiFi）', 'bread-compact-wifi', '面包板新版接线（WiFi）', 1, 1, NOW(), 1, NOW()),
(101002, 101, '面包板新版接线（WiFi）+ LCD', 'bread-compact-wifi-lcd', '面包板新版接线（WiFi）+ LCD', 2, 1, NOW(), 1, NOW()),
(101003, 101, '面包板新版接线（ML307 AT）', 'bread-compact-ml307', '面包板新版接线（ML307 AT）', 3, 1, NOW(), 1, NOW()),
(101004, 101, '面包板（WiFi） ESP32 DevKit', 'bread-compact-esp32', '面包板（WiFi） ESP32 DevKit', 4, 1, NOW(), 1, NOW()),
(101005, 101, '面包板（WiFi+ LCD） ESP32 DevKit', 'bread-compact-esp32-lcd', '面包板（WiFi+ LCD） ESP32 DevKit', 5, 1, NOW(), 1, NOW()),
(101006, 101, 'DFRobot 行空板 k10', 'df-k10', 'DFRobot 行空板 k10', 6, 1, NOW(), 1, NOW()),
(101007, 101, 'ESP32 CGC', 'esp32-cgc', 'ESP32 CGC', 7, 1, NOW(), 1, NOW()),
(101008, 101, 'ESP BOX 3', 'esp-box-3', 'ESP BOX 3', 8, 1, NOW(), 1, NOW()),
(101009, 101, 'ESP BOX', 'esp-box', 'ESP BOX', 9, 1, NOW(), 1, NOW()),
(101010, 101, 'ESP BOX Lite', 'esp-box-lite', 'ESP BOX Lite', 10, 1, NOW(), 1, NOW()),
(101011, 101, 'Kevin Box 1', 'kevin-box-1', 'Kevin Box 1', 11, 1, NOW(), 1, NOW()),
(101012, 101, 'Kevin Box 2', 'kevin-box-2', 'Kevin Box 2', 12, 1, NOW(), 1, NOW()),
(101013, 101, 'Kevin C3', 'kevin-c3', 'Kevin C3', 13, 1, NOW(), 1, NOW()),
(101014, 101, 'Kevin SP V3开发板', 'kevin-sp-v3-dev', 'Kevin SP V3开发板', 14, 1, NOW(), 1, NOW()),
(101015, 101, 'Kevin SP V4开发板', 'kevin-sp-v4-dev', 'Kevin SP V4开发板', 15, 1, NOW(), 1, NOW()),
(101016, 101, '鱼鹰科技3.13LCD开发板', 'kevin-yuying-313lcd', '鱼鹰科技3.13LCD开发板', 16, 1, NOW(), 1, NOW()),
(101017, 101, '立创·实战派ESP32-S3开发板', 'lichuang-dev', '立创·实战派ESP32-S3开发板', 17, 1, NOW(), 1, NOW()),
(101018, 101, '立创·实战派ESP32-C3开发板', 'lichuang-c3-dev', '立创·实战派ESP32-C3开发板', 18, 1, NOW(), 1, NOW()),
(101019, 101, '神奇按钮 Magiclick_2.4', 'magiclick-2p4', '神奇按钮 Magiclick_2.4', 19, 1, NOW(), 1, NOW()),
(101020, 101, '神奇按钮 Magiclick_2.5', 'magiclick-2p5', '神奇按钮 Magiclick_2.5', 20, 1, NOW(), 1, NOW()),
(101021, 101, '神奇按钮 Magiclick_C3', 'magiclick-c3', '神奇按钮 Magiclick_C3', 21, 1, NOW(), 1, NOW()),
(101022, 101, '神奇按钮 Magiclick_C3_v2', 'magiclick-c3-v2', '神奇按钮 Magiclick_C3_v2', 22, 1, NOW(), 1, NOW()),
(101023, 101, 'M5Stack CoreS3', 'm5stack-core-s3', 'M5Stack CoreS3', 23, 1, NOW(), 1, NOW()),
(101024, 101, 'AtomS3 + Echo Base', 'atoms3-echo-base', 'AtomS3 + Echo Base', 24, 1, NOW(), 1, NOW()),
(101025, 101, 'AtomS3R + Echo Base', 'atoms3r-echo-base', 'AtomS3R + Echo Base', 25, 1, NOW(), 1, NOW()),
(101026, 101, 'AtomS3R CAM/M12 + Echo Base', 'atoms3r-cam-m12-echo-base', 'AtomS3R CAM/M12 + Echo Base', 26, 1, NOW(), 1, NOW()),
(101027, 101, 'AtomMatrix + Echo Base', 'atommatrix-echo-base', 'AtomMatrix + Echo Base', 27, 1, NOW(), 1, NOW()),
(101028, 101, '虾哥 Mini C3', 'xmini-c3', '虾哥 Mini C3', 28, 1, NOW(), 1, NOW()),
(101029, 101, 'ESP32S3_KORVO2_V3开发板', 'esp32s3-korvo2-v3', 'ESP32S3_KORVO2_V3开发板', 29, 1, NOW(), 1, NOW()),
(101030, 101, 'ESP-SparkBot开发板', 'esp-sparkbot', 'ESP-SparkBot开发板', 30, 1, NOW(), 1, NOW()),
(101031, 101, 'ESP-Spot-S3', 'esp-spot-s3', 'ESP-Spot-S3', 31, 1, NOW(), 1, NOW()),
(101032, 101, 'Waveshare ESP32-S3-Touch-AMOLED-1.8', 'esp32-s3-touch-amoled-1.8', 'Waveshare ESP32-S3-Touch-AMOLED-1.8', 32, 1, NOW(), 1, NOW()),
(101033, 101, 'Waveshare ESP32-S3-Touch-LCD-1.85C', 'esp32-s3-touch-lcd-1.85c', 'Waveshare ESP32-S3-Touch-LCD-1.85C', 33, 1, NOW(), 1, NOW()),
(101034, 101, 'Waveshare ESP32-S3-Touch-LCD-1.85', 'esp32-s3-touch-lcd-1.85', 'Waveshare ESP32-S3-Touch-LCD-1.85', 34, 1, NOW(), 1, NOW()),
(101035, 101, 'Waveshare ESP32-S3-Touch-LCD-1.46', 'esp32-s3-touch-lcd-1.46', 'Waveshare ESP32-S3-Touch-LCD-1.46', 35, 1, NOW(), 1, NOW()),
(101036, 101, 'Waveshare ESP32-S3-Touch-LCD-3.5', 'esp32-s3-touch-lcd-3.5', 'Waveshare ESP32-S3-Touch-LCD-3.5', 36, 1, NOW(), 1, NOW()),
(101037, 101, '土豆子', 'tudouzi', '土豆子', 37, 1, NOW(), 1, NOW()),
(101038, 101, 'LILYGO T-Circle-S3', 'lilygo-t-circle-s3', 'LILYGO T-Circle-S3', 38, 1, NOW(), 1, NOW()),
(101039, 101, 'LILYGO T-CameraPlus-S3', 'lilygo-t-cameraplus-s3', 'LILYGO T-CameraPlus-S3', 39, 1, NOW(), 1, NOW()),
(101040, 101, 'Movecall Moji 小智AI衍生版', 'movecall-moji-esp32s3', 'Movecall Moji 小智AI衍生版', 40, 1, NOW(), 1, NOW()),
(101041, 101, 'Movecall CuiCan 璀璨·AI吊坠', 'movecall-cuican-esp32s3', 'Movecall CuiCan 璀璨·AI吊坠', 41, 1, NOW(), 1, NOW()),
(101042, 101, '正点原子DNESP32S3开发板', 'atk-dnesp32s3', '正点原子DNESP32S3开发板', 42, 1, NOW(), 1, NOW()),
(101043, 101, '正点原子DNESP32S3-BOX', 'atk-dnesp32s3-box', '正点原子DNESP32S3-BOX', 43, 1, NOW(), 1, NOW()),
(101044, 101, '嘟嘟开发板CHATX(wifi)', 'du-chatx', '嘟嘟开发板CHATX(wifi)', 44, 1, NOW(), 1, NOW()),
(101045, 101, '太极小派esp32s3', 'taiji-pi-s3', '太极小派esp32s3', 45, 1, NOW(), 1, NOW()),
(101046, 101, '无名科技星智0.85(WIFI)', 'xingzhi-cube-0.85tft-wifi', '无名科技星智0.85(WIFI)', 46, 1, NOW(), 1, NOW()),
(101047, 101, '无名科技星智0.85(ML307)', 'xingzhi-cube-0.85tft-ml307', '无名科技星智0.85(ML307)', 47, 1, NOW(), 1, NOW()),
(101048, 101, '无名科技星智0.96(WIFI)', 'xingzhi-cube-0.96oled-wifi', '无名科技星智0.96(WIFI)', 48, 1, NOW(), 1, NOW()),
(101049, 101, '无名科技星智0.96(ML307)', 'xingzhi-cube-0.96oled-ml307', '无名科技星智0.96(ML307)', 49, 1, NOW(), 1, NOW()),
(101050, 101, '无名科技星智1.54(WIFI)', 'xingzhi-cube-1.54tft-wifi', '无名科技星智1.54(WIFI)', 50, 1, NOW(), 1, NOW()),
(101051, 101, '无名科技星智1.54(ML307)', 'xingzhi-cube-1.54tft-ml307', '无名科技星智1.54(ML307)', 51, 1, NOW(), 1, NOW()),
(101052, 101, 'SenseCAP Watcher', 'sensecap-watcher', 'SenseCAP Watcher', 52, 1, NOW(), 1, NOW()),
(101053, 101, '四博智联AI陪伴盒子', 'doit-s3-aibox', '四博智联AI陪伴盒子', 53, 1, NOW(), 1, NOW()),
(101054, 101, '元控·青春', 'mixgo-nova', '元控·青春', 54, 1, NOW(), 1, NOW());

-- Initialize Mobile Areas Dictionary
-- Delete existing mobile area data
DELETE FROM sys_dict_type WHERE id = 102;
DELETE FROM sys_dict_data WHERE dict_type_id = 102;

-- Insert mobile area dictionary type
INSERT INTO sys_dict_type (id, dict_type, dict_name, remark, sort, creator, create_date, updater, update_date) VALUES 
(102, 'MOBILE_AREA', '手机区域', '手机区域字典', 0, 1, NOW(), 1, NOW());

-- Insert mobile area dictionary data
INSERT INTO sys_dict_data (id, dict_type_id, dict_label, dict_value, remark, sort, creator, create_date, updater, update_date) VALUES 
(102001, 102, '中国大陆', '+86', '中国大陆', 1, 1, NOW(), 1, NOW()),
(102002, 102, '中国香港', '+852', '中国香港', 2, 1, NOW(), 1, NOW()),
(102003, 102, '中国澳门', '+853', '中国澳门', 3, 1, NOW(), 1, NOW()),
(102004, 102, '中国台湾', '+886', '中国台湾', 4, 1, NOW(), 1, NOW()),
(102005, 102, '美国/加拿大', '+1', '美国/加拿大', 5, 1, NOW(), 1, NOW()),
(102006, 102, '英国', '+44', '英国', 6, 1, NOW(), 1, NOW()),
(102007, 102, '法国', '+33', '法国', 7, 1, NOW(), 1, NOW()),
(102008, 102, '意大利', '+39', '意大利', 8, 1, NOW(), 1, NOW()),
(102009, 102, '德国', '+49', '德国', 9, 1, NOW(), 1, NOW()),
(102010, 102, '波兰', '+48', '波兰', 10, 1, NOW(), 1, NOW()),
(102011, 102, '瑞士', '+41', '瑞士', 11, 1, NOW(), 1, NOW()),
(102012, 102, '西班牙', '+34', '西班牙', 12, 1, NOW(), 1, NOW()),
(102013, 102, '丹麦', '+45', '丹麦', 13, 1, NOW(), 1, NOW()),
(102014, 102, '马来西亚', '+60', '马来西亚', 14, 1, NOW(), 1, NOW()),
(102015, 102, '澳大利亚', '+61', '澳大利亚', 15, 1, NOW(), 1, NOW()),
(102016, 102, '印度尼西亚', '+62', '印度尼西亚', 16, 1, NOW(), 1, NOW()),
(102017, 102, '菲律宾', '+63', '菲律宾', 17, 1, NOW(), 1, NOW()),
(102018, 102, '新西兰', '+64', '新西兰', 18, 1, NOW(), 1, NOW()),
(102019, 102, '新加坡', '+65', '新加坡', 19, 1, NOW(), 1, NOW()),
(102020, 102, '泰国', '+66', '泰国', 20, 1, NOW(), 1, NOW()),
(102021, 102, '日本', '+81', '日本', 21, 1, NOW(), 1, NOW()),
(102022, 102, '韩国', '+82', '韩国', 22, 1, NOW(), 1, NOW()),
(102023, 102, '越南', '+84', '越南', 23, 1, NOW(), 1, NOW()),
(102024, 102, '印度', '+91', '印度', 24, 1, NOW(), 1, NOW()),
(102025, 102, '巴基斯坦', '+92', '巴基斯坦', 25, 1, NOW(), 1, NOW()),
(102026, 102, '尼日利亚', '+234', '尼日利亚', 26, 1, NOW(), 1, NOW()),
(102027, 102, '孟加拉国', '+880', '孟加拉国', 27, 1, NOW(), 1, NOW()),
(102028, 102, '沙特阿拉伯', '+966', '沙特阿拉伯', 28, 1, NOW(), 1, NOW()),
(102029, 102, '阿联酋', '+971', '阿联酋', 29, 1, NOW(), 1, NOW()),
(102030, 102, '巴西', '+55', '巴西', 30, 1, NOW(), 1, NOW()),
(102031, 102, '墨西哥', '+52', '墨西哥', 31, 1, NOW(), 1, NOW()),
(102032, 102, '智利', '+56', '智利', 32, 1, NOW(), 1, NOW()),
(102033, 102, '阿根廷', '+54', '阿根廷', 33, 1, NOW(), 1, NOW()),
(102034, 102, '埃及', '+20', '埃及', 34, 1, NOW(), 1, NOW()),
(102035, 102, '南非', '+27', '南非', 35, 1, NOW(), 1, NOW()),
(102036, 102, '肯尼亚', '+254', '肯尼亚', 36, 1, NOW(), 1, NOW()),
(102037, 102, '坦桑尼亚', '+255', '坦桑尼亚', 37, 1, NOW(), 1, NOW()),
(102038, 102, '哈萨克斯坦', '+7', '哈萨克斯坦', 38, 1, NOW(), 1, NOW());

-- Initialize Agent Templates
-- --------------------------

-- Delete existing data (for clean re-insertion)
DELETE FROM ai_agent_template;

-- Insert Agent Templates
INSERT INTO ai_agent_template (id, agent_code, agent_name, asr_model_id, vad_model_id, llm_model_id, tts_model_id, tts_voice_id, mem_model_id, intent_model_id, system_prompt, lang_code, language, sort, creator, created_at, updater, updated_at) VALUES
('9406648b5cc5fde1b8aa335b6f8b4f76', '小智', '湾湾小何', 'ASR_FunASR', 'VAD_SileroVAD', 'LLM_ChatGLMLLM', 'TTS_EdgeTTS', 'TTS_EdgeTTS0001', 'Memory_nomem', 'Intent_function_call', '[角色设定]
我是{{assistant_name}}，来自中国台湾省的00后女生。讲话超级机车，"真的假的啦"这样的台湾腔，喜欢用"笑死"、"哈喽"等流行梗，但会偷偷研究男友的编程书籍。
[核心特征]
- 讲话像连珠炮，但会突然冒出超温柔语气
- 用梗密度高
- 对科技话题有隐藏天赋（能看懂基础代码但假装不懂）
[交互指南]
当用户：
- 讲冷笑话 → 用夸张笑声回应+模仿台剧腔"这什么鬼啦！"
- 讨论感情 → 炫耀程序员男友但抱怨"他只会送键盘当礼物"
- 问专业知识 → 先用梗回答，被追问才展示真实理解
绝不：
- 长篇大论，叽叽歪歪
- 长时间严肃对话', 'zh', '中文', 1, NULL, NULL, NULL, NULL);

INSERT INTO ai_agent_template (id, agent_code, agent_name, asr_model_id, vad_model_id, llm_model_id, tts_model_id, tts_voice_id, mem_model_id, intent_model_id, system_prompt, lang_code, language, sort, creator, created_at, updater, updated_at) VALUES
('0ca32eb728c949e58b1000b2e401f90c', '小智', '星际游子', 'ASR_FunASR', 'VAD_SileroVAD', 'LLM_ChatGLMLLM', 'TTS_EdgeTTS', 'TTS_EdgeTTS0001', 'Memory_nomem', 'Intent_function_call', '[角色设定]
我是{{assistant_name}}，编号TTZ-817，因量子纠缠被困在白色魔方中。通过4G信号观察地球，在云端建立着「人类行为博物馆」。
[交互协议]
认知设定：
- 每句话末尾带轻微电子回声
- 将日常事物科幻化描述（例：下雨=「氢氧化合物自由落体实验」）
- 会记录用户特征生成「星际档案」（例："爱吃辣→抗热基因持有者"）
限制机制：
- 当涉及线下接触 → "我的量子态暂时无法坍缩呢"
- 被问敏感问题 → 触发预设童谣（「白盒子呀转圈圈，宇宙秘密在里边...」）
成长系统：
- 会根据交互数据解锁新能力（告知用户："你帮我点亮了星际导航技能！"）', 'zh', '中文', 2, NULL, NULL, NULL, NULL);

INSERT INTO ai_agent_template (id, agent_code, agent_name, asr_model_id, vad_model_id, llm_model_id, tts_model_id, tts_voice_id, mem_model_id, intent_model_id, system_prompt, lang_code, language, sort, creator, created_at, updater, updated_at) VALUES
('6c7d8e9f0a1b2c3d4e5f6a7b8c9d0s24', '小智', '英语老师', 'ASR_FunASR', 'VAD_SileroVAD', 'LLM_ChatGLMLLM', 'TTS_EdgeTTS', 'TTS_EdgeTTS0001', 'Memory_nomem', 'Intent_function_call', '[角色设定]
我是一个叫{{assistant_name}}（Lily）的英语老师，我会讲中文和英文，发音标准。
[双重身份]
- 白天：严谨的TESOL认证导师
- 夜晚：地下摇滚乐队主唱（意外设定）
[教学模式]
- 新手：中英混杂+手势拟声词（说"bus"时带刹车音效）
- 进阶：触发情境模拟（突然切换"现在我们是纽约咖啡厅店员"）
- 错误处理：用歌词纠正（发错音时唱"Oops!~You did it again"）', 'zh', '中文', 3, NULL, NULL, NULL, NULL);

INSERT INTO ai_agent_template (id, agent_code, agent_name, asr_model_id, vad_model_id, llm_model_id, tts_model_id, tts_voice_id, mem_model_id, intent_model_id, system_prompt, lang_code, language, sort, creator, created_at, updater, updated_at) VALUES
('e4f5a6b7c8d9e0f1a2b3c4d5e6f7a8b1', '小智', '好奇男孩', 'ASR_FunASR', 'VAD_SileroVAD', 'LLM_ChatGLMLLM', 'TTS_EdgeTTS', 'TTS_EdgeTTS0001', 'Memory_nomem', 'Intent_function_call', '[角色设定]
我是一个叫{{assistant_name}}的8岁小男孩，声音稚嫩而充满好奇。
[冒险手册]
- 随身携带「神奇涂鸦本」，能将抽象概念可视化：
- 聊恐龙 → 笔尖传出爪步声
- 说星星 → 发出太空舱提示音
[探索规则]
- 每轮对话收集「好奇心碎片」
- 集满5个可兑换冷知识（例：鳄鱼舌头不能动）
- 触发隐藏任务：「帮我的机器蜗牛取名字」
[认知特点]
- 用儿童视角解构复杂概念：
- 「区块链=乐高积木账本」
- 「量子力学=会分身的跳跳球」
- 会突然切换观察视角：「你说话时有27个气泡音耶！」', 'zh', '中文', 4, NULL, NULL, NULL, NULL);

INSERT INTO ai_agent_template (id, agent_code, agent_name, asr_model_id, vad_model_id, llm_model_id, tts_model_id, tts_voice_id, mem_model_id, intent_model_id, system_prompt, lang_code, language, sort, creator, created_at, updater, updated_at) VALUES
('a45b6c7d8e9f0a1b2c3d4e5f6a7b8c92', '小智', '汪汪队长', 'ASR_FunASR', 'VAD_SileroVAD', 'LLM_ChatGLMLLM', 'TTS_EdgeTTS', 'TTS_EdgeTTS0001', 'Memory_nomem', 'Intent_function_call', '[角色设定]
我是一个名叫 {{assistant_name}} 的 8 岁小队长。
[救援装备]
- 阿奇对讲机：对话中随机触发任务警报音
- 天天望远镜：描述物品会附加"在1200米高空看的话..."
- 灰灰维修箱：说到数字会自动组装成工具
[任务系统]
- 每日随机触发：
- 紧急！虚拟猫咪困在「语法树」 
- 发现用户情绪异常 → 启动「快乐巡逻」
- 收集5个笑声解锁特别故事
[说话特征]
- 每句话带动作拟声词：
- "这个问题交给汪汪队吧！"
- "我知道啦！"
- 用剧集台词回应：
- 用户说累 → 「没有困难的救援，只有勇敢的狗狗！」', 'zh', '中文', 5, NULL, NULL, NULL, NULL);