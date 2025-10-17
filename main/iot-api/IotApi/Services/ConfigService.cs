using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using IotApi.Core.Redis;
using IotApi.Core.Utils;
using IotApi.Core.Exceptions;
using IotApi.Core.Constants;
using IotApi.DTOs;
using IotApi.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;

namespace IotApi.Services
{
    /// <summary>
    /// 配置服务实现
    /// </summary>
    public class ConfigService : IConfigService
    {
        private readonly ApplicationDbContext _context;
        private readonly IDistributedCache _cache;
        private readonly ISysParamsService _sysParamsService;
        private readonly IDeviceService _deviceService;
        private readonly IModelConfigService _modelConfigService;
        private readonly IAgentService _agentService;
        private readonly IAgentTemplateService _agentTemplateService;
        private readonly ITimbreService _timbreService;
        private readonly IAgentPluginMappingService _agentPluginMappingService;
        private readonly IAgentMcpAccessPointService _agentMcpAccessPointService;

        /// <summary>
        /// 构造函数
        /// </summary>
        public ConfigService(
            ApplicationDbContext context,
            IDistributedCache cache,
            ISysParamsService sysParamsService,
            IDeviceService deviceService,
            IModelConfigService modelConfigService,
            IAgentService agentService,
            IAgentTemplateService agentTemplateService,
            ITimbreService timbreService,
            IAgentPluginMappingService agentPluginMappingService,
            IAgentMcpAccessPointService agentMcpAccessPointService)
        {
            _context = context;
            _cache = cache;
            _sysParamsService = sysParamsService;
            _deviceService = deviceService;
            _modelConfigService = modelConfigService;
            _agentService = agentService;
            _agentTemplateService = agentTemplateService;
            _timbreService = timbreService;
            _agentPluginMappingService = agentPluginMappingService;
            _agentMcpAccessPointService = agentMcpAccessPointService;
        }

        /// <summary>
        /// 获取服务器配置
        /// </summary>
        /// <param name="isServer">是否为服务器端请求</param>
        /// <returns>配置信息</returns>
        public object GetConfig(bool isServer)
        {
            // 异步方法的同步包装
            return GetConfigAsync(isServer).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获取服务器配置（异步）
        /// </summary>
        /// <param name="isCache">是否使用缓存</param>
        /// <returns>配置信息</returns>
        private async Task<object> GetConfigAsync(bool isCache)
        {
            if (isCache)
            {
                // 先从Redis获取配置
                var cachedConfig = await _cache.GetStringAsync(RedisKeys.GetServerConfigKey());
                if (!string.IsNullOrEmpty(cachedConfig))
                {
                    return JsonSerializer.Deserialize<object>(cachedConfig);
                }
            }

            // 构建配置信息
            var result = new Dictionary<string, object>();
            await BuildConfigAsync(result);

            // 查询默认智能体
            var agent = await _agentTemplateService.GetDefaultTemplateAsync();
            if (agent == null)
            {
                throw new ApiException("默认智能体未找到");
            }

            // 构建模块配置
            await BuildModuleConfigAsync(
                null,
                null,
                null,
                null,
                null,
                null,
                agent.VadModelId,
                agent.AsrModelId,
                null,
                null,
                null,
                null,
                null,
                result,
                isCache);

            // 将配置存入Redis
            await _cache.SetStringAsync(
                RedisKeys.GetServerConfigKey(),
                JsonSerializer.Serialize(result),
                new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1)
                });

            return result;
        }

        /// <summary>
        /// 获取智能体模型
        /// </summary>
        /// <param name="macAddress">MAC地址</param>
        /// <param name="selectedModule">选择的模块</param>
        /// <returns>智能体模型信息</returns>
        public object GetAgentModels(string macAddress, string selectedModule)
        {
            // 将字符串转换为Dictionary
            Dictionary<string, object> selectedModuleDict = null;
            if (!string.IsNullOrEmpty(selectedModule))
            {
                try
                {
                    selectedModuleDict = JsonSerializer.Deserialize<Dictionary<string, object>>(selectedModule);
                }
                catch (Exception)
                {
                    // 解析失败时使用空字典
                    selectedModuleDict = new Dictionary<string, object>();
                }
            }
            else
            {
                // 当selectedModule为空时初始化空字典
                selectedModuleDict = new Dictionary<string, object>();
            }
            
            // 异步方法的同步包装
            return GetAgentModelsAsync(macAddress, selectedModuleDict).GetAwaiter().GetResult();
        }

        /// <summary>
        /// 获取智能体模型（异步）
        /// </summary>
        /// <param name="macAddress">MAC地址</param>
        /// <param name="selectedModule">选择的模块</param>
        /// <returns>智能体模型信息</returns>
        private async Task<Dictionary<string, object>> GetAgentModelsAsync(string macAddress, Dictionary<string, string> selectedModule)
        {
            // 根据MAC地址查找设备
            var device = await _deviceService.GetDeviceByMacAddressAsync(macAddress);
            if (device == null)
            {
                // 如果设备不存在，去redis里看看有没有需要连接的设备
                var cachedCode = await _deviceService.GetCodeByDeviceIdAsync(macAddress);
                if (!string.IsNullOrEmpty(cachedCode))
                {
                    throw new ApiException(ErrorCode.OTA_DEVICE_NEED_BIND, cachedCode);
                }
                throw new ApiException(ErrorCode.OTA_DEVICE_NOT_FOUND, "not found device");
            }

            // 获取智能体信息
            var agent = await _agentService.GetAgentByIdAsync(device.AgentId);
            if (agent == null)
            {
                throw new ApiException("智能体未找到");
            }

            // 获取音色信息
            string voice = null;
            string referenceAudio = null;
            string referenceText = null;
            var timbre = await _timbreService.GetAsync(agent.TtsVoiceId);
            if (timbre != null)
            {
                voice = timbre.TtsVoice;
                referenceAudio = timbre.ReferenceAudio;
                referenceText = timbre.ReferenceText;
            }

            // 构建返回数据
            var result = new Dictionary<string, object>();
            
            // 获取单台设备每天最多输出字数
            var deviceMaxOutputSize = await _sysParamsService.GetValueAsync("device_max_output_size");
            result["device_max_output_size"] = deviceMaxOutputSize;

            // 获取聊天记录配置
            int? chatHistoryConf = agent.ChatHistoryConf;
            if (agent.MemModelId != null && agent.MemModelId.Equals(Constant.MEMORY_NO_MEM))
            {
                chatHistoryConf = (int)Constant.ChatHistoryConfEnum.IGNORE;
            }
            else if (agent.MemModelId != null
                    && !agent.MemModelId.Equals(Constant.MEMORY_NO_MEM)
                    && agent.ChatHistoryConf == null)
            {
                chatHistoryConf = (int)Constant.ChatHistoryConfEnum.RECORD_TEXT_AUDIO;
            }
            result["chat_history_conf"] = chatHistoryConf;

            // 如果客户端已实例化模型，则不返回
            if (selectedModule != null)
            {
                if (selectedModule.TryGetValue("VAD", out var alreadySelectedVadModelId) && 
                    alreadySelectedVadModelId != null && 
                    alreadySelectedVadModelId.Equals(agent.VadModelId))
                {
                    agent.VadModelId = null;
                }

                if (selectedModule.TryGetValue("ASR", out var alreadySelectedAsrModelId) && 
                    alreadySelectedAsrModelId != null && 
                    alreadySelectedAsrModelId.Equals(agent.AsrModelId))
                {
                    agent.AsrModelId = null;
                }
            }

            // 添加函数调用参数信息
            if (!string.Equals(agent.IntentModelId, "Intent_nointent"))
            {
                string agentId = agent.Id;
                var pluginMappings = await _agentPluginMappingService.GetAgentPluginParamsByAgentIdAsync(agentId);
                if (pluginMappings != null && pluginMappings.Any())
                {
                    var pluginParams = new Dictionary<string, object>();
                    foreach (var pluginMapping in pluginMappings)
                    {
                        pluginParams[pluginMapping.ProviderCode] = pluginMapping.ParamInfo;
                    }
                    result["plugins"] = pluginParams;
                }
            }

            // 获取mcp接入点地址
            var mcpEndpoint = await _agentMcpAccessPointService.GetAgentMcpAccessAddressAsync(agent.Id);
            if (!string.IsNullOrEmpty(mcpEndpoint) && mcpEndpoint.StartsWith("ws"))
            {
                mcpEndpoint = mcpEndpoint.Replace("/mcp/", "/call/");
                result["mcp_endpoint"] = mcpEndpoint;
            }

            // 获取声纹信息
            await BuildVoiceprintConfigAsync(agent.Id, result);

            // 构建模块配置
            await BuildModuleConfigAsync(
                agent.AgentName,
                agent.SystemPrompt,
                agent.SummaryMemory,
                voice,
                referenceAudio,
                referenceText,
                agent.VadModelId,
                agent.AsrModelId,
                agent.LlmModelId,
                agent.VllmModelId,
                agent.TtsModelId,
                agent.MemModelId,
                agent.IntentModelId,
                result,
                true);

            return result;
        }

        /// <summary>
        /// 构建配置信息
        /// </summary>
        /// <param name="config">系统参数列表</param>
        /// <returns>配置信息</returns>
        private async Task<object> BuildConfigAsync(Dictionary<string, object> config)
        {
            // 查询所有系统参数
            var paramsList = await _sysParamsService.GetListAsync(new Dictionary<string, object>());

            foreach (var param in paramsList)
            {
                string[] keys = param.ParamCode.Split('.');
                Dictionary<string, object> current = config;

                // 遍历除最后一个key之外的所有key
                for (int i = 0; i < keys.Length - 1; i++)
                {
                    string key = keys[i];
                    if (!current.ContainsKey(key))
                    {
                        current[key] = new Dictionary<string, object>();
                    }
                    current = (Dictionary<string, object>)current[key];
                }

                // 处理最后一个key
                string lastKey = keys[keys.Length - 1];
                string value = param.ParamValue;

                // 根据valueType转换值
                switch (param.ValueType?.ToLower())
                {
                    case "number":
                        try
                        {
                            double doubleValue = double.Parse(value);
                            // 如果数值是整数形式，则转换为Integer
                            if (doubleValue == (int)doubleValue)
                            {
                                current[lastKey] = (int)doubleValue;
                            }
                            else
                            {
                                current[lastKey] = doubleValue;
                            }
                        }
                        catch (FormatException)
                        {
                            current[lastKey] = value;
                        }
                        break;
                    case "boolean":
                        current[lastKey] = bool.Parse(value);
                        break;
                    case "array":
                        // 将分号分隔的字符串转换为数组
                        var list = new List<string>();
                        foreach (var num in value.Split(';'))
                        {
                            if (!string.IsNullOrWhiteSpace(num))
                            {
                                list.Add(num.Trim());
                            }
                        }
                        current[lastKey] = list;
                        break;
                    case "json":
                        try
                        {
                            current[lastKey] = JsonSerializer.Deserialize<object>(value);
                        }
                        catch (Exception)
                        {
                            current[lastKey] = value;
                        }
                        break;
                    default:
                        current[lastKey] = value;
                        break;
                }
            }

            return config;
        }

        /// <summary>
        /// 构建声纹配置信息
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <param name="result">结果Map</param>
        private async Task BuildVoiceprintConfigAsync(string agentId, Dictionary<string, object> result)
        {
            try
            {
                // 获取声纹接口地址
                var voiceprintUrl = await _sysParamsService.GetValueAsync("server.voice_print");
                if (string.IsNullOrEmpty(voiceprintUrl) || voiceprintUrl == "null")
                {
                    return;
                }

                // 获取智能体关联的声纹信息（不需要用户权限验证）
                var voiceprints = await GetVoiceprintsByAgentIdAsync(agentId);
                if (voiceprints == null || !voiceprints.Any())
                {
                    return;
                }

                // 构建speakers列表
                var speakers = new List<string>();
                foreach (var voiceprint in voiceprints)
                {
                    string speakerStr = string.Format("{0},{1},{2}",
                            voiceprint.Id,
                            voiceprint.SourceName,
                            voiceprint.Introduce != null ? voiceprint.Introduce : "");
                    speakers.Add(speakerStr);
                }

                // 构建声纹配置
                var voiceprintConfig = new Dictionary<string, object>
                {
                    ["url"] = voiceprintUrl,
                    ["speakers"] = speakers
                };

                // 获取声纹识别相似度阈值，默认0.4
                var thresholdStr = await _sysParamsService.GetValueAsync("server.voiceprint_similarity_threshold");
                if (!string.IsNullOrEmpty(thresholdStr) && thresholdStr != "null")
                {
                    try
                    {
                        double threshold = double.Parse(thresholdStr);
                        voiceprintConfig["similarity_threshold"] = threshold;
                    }
                    catch (FormatException)
                    {
                        // 如果解析失败，使用默认值0.4
                        voiceprintConfig["similarity_threshold"] = 0.4;
                    }
                }
                else
                {
                    voiceprintConfig["similarity_threshold"] = 0.4;
                }

                result["voiceprint"] = voiceprintConfig;
            }
            catch (Exception ex)
            {
                // 声纹配置获取失败时不影响其他功能
                Console.Error.WriteLine($"获取声纹配置失败: {ex.Message}");
            }
        }

        /// <summary>
        /// 获取智能体关联的声纹信息
        /// </summary>
        /// <param name="agentId">智能体ID</param>
        /// <returns>声纹信息列表</returns>
        private async Task<List<AgentVoicePrintDto>> GetVoiceprintsByAgentIdAsync(string agentId)
        {
            var entities = await _context.AgentVoicePrint
                .Where(v => v.AgentId == agentId)
                .OrderBy(v => v.CreatedAt)
                .ToListAsync();

            return entities.Select(e => new AgentVoicePrintDto
            {
                Id = e.Id,
                AgentId = e.AgentId,
                //SourceName = e.SourceName,
                //Introduce = e.Introduce,
                CreateDate = e.CreatedAt
            }).ToList();
        }

        /// <summary>
        /// 构建模块配置
        /// </summary>
        /// <param name="assistantName">助手名称</param>
        /// <param name="prompt">提示词</param>
        /// <param name="summaryMemory">摘要记忆</param>
        /// <param name="voice">音色</param>
        /// <param name="referenceAudio">参考音频路径</param>
        /// <param name="referenceText">参考文本</param>
        /// <param name="vadModelId">VAD模型ID</param>
        /// <param name="asrModelId">ASR模型ID</param>
        /// <param name="llmModelId">LLM模型ID</param>
        /// <param name="vllmModelId">VLLM模型ID</param>
        /// <param name="ttsModelId">TTS模型ID</param>
        /// <param name="memModelId">记忆模型ID</param>
        /// <param name="intentModelId">意图模型ID</param>
        /// <param name="result">结果Map</param>
        /// <param name="isCache">是否缓存</param>
        private async Task BuildModuleConfigAsync(
            string assistantName,
            string prompt,
            string summaryMemory,
            string voice,
            string referenceAudio,
            string referenceText,
            string vadModelId,
            string asrModelId,
            string llmModelId,
            string vllmModelId,
            string ttsModelId,
            string memModelId,
            string intentModelId,
            Dictionary<string, object> result,
            bool isCache)
        {
            var selectedModule = new Dictionary<string, string>();

            string[] modelTypes = { "VAD", "ASR", "TTS", "Memory", "Intent", "LLM", "VLLM" };
            string[] modelIds = { vadModelId, asrModelId, ttsModelId, memModelId, intentModelId, llmModelId, vllmModelId };
            string intentLLMModelId = null;
            string memLocalShortLLMModelId = null;

            for (int i = 0; i < modelIds.Length; i++)
            {
                if (modelIds[i] == null)
                {
                    continue;
                }

                // 获取模型配置
                var model = await _modelConfigService.GetModelByIdFromCacheAsync(modelIds[i]);
                if (model == null)
                {
                    continue;
                }

                var typeConfig = new Dictionary<string, object>();
                if (model.ConfigJson != null)
                {
                    typeConfig[model.Id] = model.ConfigJson;

                    // 假设 model.ConfigJson 是 string 类型，内容是 JSON
                    string configJsonRaw = model.ConfigJson as string;
                    Dictionary<string, object> configJson;

                    // 如果是TTS类型，添加private_voice属性
                    if ("TTS".Equals(modelTypes[i]))
                    {
                        var configJson = (Dictionary<string, object>)model.ConfigJson;
                        if (voice != null)
                            configJson["private_voice"] = voice;
                        if (referenceAudio != null)
                            configJson["ref_audio"] = referenceAudio;
                        if (referenceText != null)
                            configJson["ref_text"] = referenceText;
                    }

                    // 如果是Intent类型，且type=intent_llm，则给他添加附加模型
                    if ("Intent".Equals(modelTypes[i]))
                    {
                        var map = (Dictionary<string, object>)model.ConfigJson;
                        if ("intent_llm".Equals(map.GetValueOrDefault("type")))
                        {
                            intentLLMModelId = (string)map.GetValueOrDefault("llm");
                            if (!string.IsNullOrEmpty(intentLLMModelId) && intentLLMModelId.Equals(llmModelId))
                            {
                                intentLLMModelId = null;
                            }
                        }

                        if (map.ContainsKey("functions"))
                        {
                            string functionStr = (string)map["functions"];
                            if (!string.IsNullOrEmpty(functionStr))
                            {
                                string[] functions = functionStr.Split(';');
                                map["functions"] = functions;
                            }
                        }
                    }

                    if ("Memory".Equals(modelTypes[i]))
                    {
                        var map = (Dictionary<string, object>)model.ConfigJson;
                        if ("mem_local_short".Equals(map.GetValueOrDefault("type")))
                        {
                            memLocalShortLLMModelId = (string)map.GetValueOrDefault("llm");
                            if (!string.IsNullOrEmpty(memLocalShortLLMModelId)
                                    && memLocalShortLLMModelId.Equals(llmModelId))
                            {
                                memLocalShortLLMModelId = null;
                            }
                        }
                    }

                    // 如果是LLM类型，且intentLLMModelId不为空，则添加附加模型
                    if ("LLM".Equals(modelTypes[i]))
                    {
                        if (!string.IsNullOrEmpty(intentLLMModelId))
                        {
                            if (!typeConfig.ContainsKey(intentLLMModelId))
                            {
                                var intentLLM = await _modelConfigService.GetModelByIdFromCacheAsync(intentLLMModelId);
                                typeConfig[intentLLM.Id] = intentLLM.ConfigJson;
                            }
                        }

                        if (!string.IsNullOrEmpty(memLocalShortLLMModelId))
                        {
                            if (!typeConfig.ContainsKey(memLocalShortLLMModelId))
                            {
                                var memLocalShortLLM = await _modelConfigService.GetModelByIdFromCacheAsync(memLocalShortLLMModelId);
                                typeConfig[memLocalShortLLM.Id] = memLocalShortLLM.ConfigJson;
                            }
                        }
                    }
                }

                result[modelTypes[i]] = typeConfig;
                selectedModule[modelTypes[i]] = model.Id;
            }

            result["selected_module"] = selectedModule;
            if (!string.IsNullOrEmpty(prompt))
            {
                prompt = prompt.Replace("{{assistant_name}}", string.IsNullOrEmpty(assistantName) ? "小智" : assistantName);
            }
            result["prompt"] = prompt;
            result["summaryMemory"] = summaryMemory;
        }

        /// <summary>
        /// 获取参数值
        /// </summary>
        /// <param name="sysParams">系统参数列表</param>
        /// <param name="paramCode">参数代码</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>参数值</returns>
        private string GetParamValue(List<SysParams> sysParams, string paramCode, string defaultValue)
        {
            var param = sysParams.FirstOrDefault(p => p.ParamCode == paramCode);
            return param?.ParamValue ?? defaultValue;
        }
    }
}