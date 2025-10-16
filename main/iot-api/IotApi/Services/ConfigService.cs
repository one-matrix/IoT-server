using IotApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IotApi.Services
{
    /// <summary>
    /// 配置服务实现
    /// </summary>
    public class ConfigService : IConfigService
    {
        private readonly ApplicationDbContext _context;

        public ConfigService(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// 获取服务器配置
        /// </summary>
        /// <param name="isServer">是否为服务器端请求</param>
        /// <returns>配置信息</returns>
        public object GetConfig(bool isServer)
        {
            // 从数据库获取系统参数
            var sysParams = _context.SysParams.ToList();
            
            // 构建配置对象
            var config = new
            {
                server = new
                {
                    name = "xiaozhi-server",
                    version = GetParamValue(sysParams, "server.version", "1.0.0"),
                    host = GetParamValue(sysParams, "server.host", "localhost"),
                    port = int.Parse(GetParamValue(sysParams, "server.port", "8080"))
                },
                mqtt = new
                {
                    host = GetParamValue(sysParams, "mqtt.host", "localhost"),
                    port = int.Parse(GetParamValue(sysParams, "mqtt.port", "1883")),
                    username = GetParamValue(sysParams, "mqtt.username", ""),
                    password = GetParamValue(sysParams, "mqtt.password", "")
                }
            };

            return config;
        }

        /// <summary>
        /// 获取智能体模型
        /// </summary>
        /// <param name="macAddress">MAC地址</param>
        /// <param name="selectedModule">选择的模块</param>
        /// <returns>智能体模型信息</returns>
        public object GetAgentModels(string macAddress, string selectedModule)
        {
            // 根据MAC地址查找设备
            var device = _context.AiDevices.FirstOrDefault(d => d.MacAddress == macAddress);
            if (device == null)
            {
                return new { error = "设备不存在" };
            }

            // 获取设备关联的智能体
            var agent = _context.AiAgents.FirstOrDefault(a => a.Id == device.AgentId);
            if (agent == null)
            {
                return new { error = "智能体不存在" };
            }

            // 获取模型配置
            var asrModel = _context.AiModelConfigs.FirstOrDefault(m => m.Id == agent.AsrModelId);
            var vadModel = _context.AiModelConfigs.FirstOrDefault(m => m.Id == agent.VadModelId);
            var llmModel = _context.AiModelConfigs.FirstOrDefault(m => m.Id == agent.LlmModelId);
            var ttsModel = _context.AiModelConfigs.FirstOrDefault(m => m.Id == agent.TtsModelId);
            var vllmModel = _context.AiModelConfigs.FirstOrDefault(m => m.Id == agent.VllmModelId);

            // 构建返回对象
            var models = new
            {
                asr = asrModel != null ? new { name = asrModel.ModelName, config = asrModel.ConfigJson } : null,
                vad = vadModel != null ? new { name = vadModel.ModelName, config = vadModel.ConfigJson } : null,
                llm = llmModel != null ? new { name = llmModel.ModelName, config = llmModel.ConfigJson } : null,
                tts = ttsModel != null ? new { name = ttsModel.ModelName, config = ttsModel.ConfigJson } : null,
                vllm = vllmModel != null ? new { name = vllmModel.ModelName, config = vllmModel.ConfigJson } : null
            };

            return models;
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