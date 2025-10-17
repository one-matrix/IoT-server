using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using Microsoft.Extensions.Logging;
using IotApi.Core.Utils;
using IotApi.Core.utils;

namespace IotApi.Services
{
    /// <summary>
    /// 智能体MCP接入点服务实现类
    /// </summary>
    public class AgentMcpAccessPointService : IAgentMcpAccessPointService
    {
        private readonly ApplicationDbContext _dbContext;
        private readonly IConfigService _configService;
        private readonly ILogger<AgentMcpAccessPointService> _logger;

        public AgentMcpAccessPointService(
            ApplicationDbContext dbContext, 
            IConfigService configService,
            ILogger<AgentMcpAccessPointService> logger)
        {
            _dbContext = dbContext;
            _configService = configService;
            _logger = logger;
        }

        /// <summary>
        /// 获取智能体的MCP接入点地址
        /// </summary>
        /// <param name="id">智能体ID</param>
        /// <returns>MCP接入点地址</returns>
        public async Task<string> GetAgentMcpAccessAddressAsync(string id)
        {
            // 获取到mcp的地址
            var url = await _configService.GetConfigValueAsync("SERVER_MCP_ENDPOINT");
            if (string.IsNullOrEmpty(url) || url == "null")
            {
                return null;
            }

            try
            {
                // 解析URI
                var uri = new Uri(url);
                
                // 获取智能体mcp的url前缀
                var agentMcpUrl = GetAgentMcpUrl(uri);
                
                // 获取密钥
                var key = GetSecretKey(uri);
                
                // 获取加密的token
                var encryptToken = EncryptToken(id, key);
                
                // 对token进行URL编码
                var encodedToken = HttpUtility.UrlEncode(encryptToken);
                
                // 返回智能体Mcp路径的格式
                return $"{agentMcpUrl}/mcp/?token={encodedToken}";
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "路径格式不正确路径：{Url}", url);
                throw new InvalidOperationException("mcp的地址存在错误，请进入参数管理修改mcp接入点地址", ex);
            }
        }

        /// <summary>
        /// 获取智能体的MCP接入点已有的工具列表
        /// </summary>
        /// <param name="id">智能体ID</param>
        /// <returns>工具列表</returns>
        public async Task<IEnumerable<string>> GetAgentMcpToolsListAsync(string id)
        {
            var wsUrl = await GetAgentMcpAccessAddressAsync(id);
            if (string.IsNullOrEmpty(wsUrl))
            {
                return new List<string>();
            }

            // 将 /mcp 替换为 /call
            wsUrl = wsUrl.Replace("/mcp/", "/call/");

            try
            {
                using (var client = new ClientWebSocket())
                {
                    // 设置连接超时
                    var cts = new CancellationTokenSource();
                    cts.CancelAfter(TimeSpan.FromSeconds(8));

                    // 连接WebSocket
                    _logger.LogInformation("连接MCP WebSocket，智能体ID: {AgentId}", id);
                    await client.ConnectAsync(new Uri(wsUrl), cts.Token);

                    // 步骤1: 发送初始化消息
                    _logger.LogInformation("发送MCP初始化消息，智能体ID: {AgentId}", id);
                    var initializeJson = GetInitializeJson();
                    await SendMessageAsync(client, initializeJson, cts.Token);

                    // 等待初始化响应
                    var initResponse = await ReceiveMessageAsync(client, cts.Token);
                    var initJsonResponse = JsonSerializer.Deserialize<JsonElement>(initResponse);
                    
                    if (!ValidateResponse(initJsonResponse, 1))
                    {
                        _logger.LogError("MCP初始化失败，智能体ID: {AgentId}", id);
                        return new List<string>();
                    }

                    _logger.LogInformation("MCP初始化成功，智能体ID: {AgentId}", id);

                    // 步骤2: 发送初始化完成通知
                    _logger.LogInformation("发送MCP初始化完成通知，智能体ID: {AgentId}", id);
                    var notificationsInitializedJson = GetNotificationsInitializedJson();
                    await SendMessageAsync(client, notificationsInitializedJson, cts.Token);

                    // 步骤3: 发送工具列表请求
                    _logger.LogInformation("发送MCP工具列表请求，智能体ID: {AgentId}", id);
                    var toolsListJson = GetToolsListJson();
                    await SendMessageAsync(client, toolsListJson, cts.Token);

                    // 等待工具列表响应
                    var toolsResponse = await ReceiveMessageAsync(client, cts.Token);
                    var toolsJsonResponse = JsonSerializer.Deserialize<JsonElement>(toolsResponse);

                    if (!ValidateResponse(toolsJsonResponse, 2))
                    {
                        _logger.LogError("获取工具列表失败，智能体ID: {AgentId}", id);
                        return new List<string>();
                    }

                    // 提取工具名称列表
                    var toolsList = new List<string>();
                    if (toolsJsonResponse.TryGetProperty("result", out var result) && 
                        result.TryGetProperty("tools", out var tools) && 
                        tools.ValueKind == JsonValueKind.Array)
                    {
                        foreach (var tool in tools.EnumerateArray())
                        {
                            if (tool.TryGetProperty("name", out var name))
                            {
                                toolsList.Add(name.GetString());
                            }
                        }
                    }

                    _logger.LogInformation("成功获取MCP工具列表，智能体ID: {AgentId}, 工具数量: {Count}", id, toolsList.Count);
                    return toolsList;
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "获取智能体 MCP 工具列表失败，智能体ID: {AgentId}", id);
                return new List<string>();
            }
        }

        /// <summary>
        /// 获取智能体mcp接入点url
        /// </summary>
        /// <param name="uri">mcp地址</param>
        /// <returns>智能体mcp接入点url</returns>
        private string GetAgentMcpUrl(Uri uri)
        {
            // 获取协议
            var wsScheme = (uri.Scheme == "https") ? "wss" : "ws";
            
            // 获取主机和端口
            var authority = uri.Authority;
            
            // 获取路径，截取到最后一个/前
            var path = uri.AbsolutePath;
            path = path.Substring(0, path.LastIndexOf('/'));
            
            return $"{wsScheme}://{authority}{path}";
        }

        /// <summary>
        /// 获取密钥
        /// </summary>
        /// <param name="uri">mcp地址</param>
        /// <returns>密钥</returns>
        private string GetSecretKey(Uri uri)
        {
            // 获取参数
            var query = uri.Query;
            
            // 获取aes加密密钥
            var str = "key=";
            var keyIndex = query.IndexOf(str);
            if (keyIndex < 0)
            {
                throw new InvalidOperationException("MCP地址中未找到密钥参数");
            }
            
            return query.Substring(keyIndex + str.Length);
        }

        /// <summary>
        /// 获取对智能体id加密的token
        /// </summary>
        /// <param name="agentId">智能体id</param>
        /// <param name="key">加密密钥</param>
        /// <returns>加密后token</returns>
        private string EncryptToken(string agentId, string key)
        {
            // 使用md5对智能体id进行加密
            var md5 = HashUtils.Md5(agentId);
            
            // aes需要加密文本
            var json = $"{{\"agentId\": \"{md5}\"}}";
            
            // 加密后成token值
            return AESUtils.Encrypt(key, json);
        }

        /// <summary>
        /// 发送WebSocket消息
        /// </summary>
        private async Task SendMessageAsync(ClientWebSocket client, string message, CancellationToken cancellationToken)
        {
            var buffer = Encoding.UTF8.GetBytes(message);
            await client.SendAsync(new ArraySegment<byte>(buffer), WebSocketMessageType.Text, true, cancellationToken);
        }

        /// <summary>
        /// 接收WebSocket消息
        /// </summary>
        private async Task<string> ReceiveMessageAsync(ClientWebSocket client, CancellationToken cancellationToken)
        {
            var buffer = new byte[4096];
            var result = await client.ReceiveAsync(new ArraySegment<byte>(buffer), cancellationToken);
            return Encoding.UTF8.GetString(buffer, 0, result.Count);
        }

        /// <summary>
        /// 验证WebSocket响应
        /// </summary>
        private bool ValidateResponse(JsonElement response, int expectedId)
        {
            if (response.TryGetProperty("id", out var id) && id.GetInt32() == expectedId)
            {
                return response.TryGetProperty("result", out _) && !response.TryGetProperty("error", out _);
            }
            return false;
        }

        /// <summary>
        /// 获取初始化JSON
        /// </summary>
        private string GetInitializeJson()
        {
            return @"{
                ""jsonrpc"": ""2.0"",
                ""id"": 1,
                ""method"": ""initialize"",
                ""params"": {
                    ""capabilities"": {
                        ""workspace"": {
                            ""configuration"": true
                        }
                    }
                }
            }";
        }

        /// <summary>
        /// 获取初始化完成通知JSON
        /// </summary>
        private string GetNotificationsInitializedJson()
        {
            return @"{
                ""jsonrpc"": ""2.0"",
                ""method"": ""initialized"",
                ""params"": {}
            }";
        }

        /// <summary>
        /// 获取工具列表JSON
        /// </summary>
        private string GetToolsListJson()
        {
            return @"{
                ""jsonrpc"": ""2.0"",
                ""id"": 2,
                ""method"": ""workspace/executeCommand"",
                ""params"": {
                    ""command"": ""xiaozhi.getToolsList"",
                    ""arguments"": []
                }
            }";
        }
    }
}