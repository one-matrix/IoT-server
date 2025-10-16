 using Microsoft.AspNetCore.Mvc;
using IotApi.Models;
using Microsoft.EntityFrameworkCore;

namespace IotApi.Controllers
{
    [ApiController]
    [Route("xiaozhi/admin/server")]
    public class ServerSideManageController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public ServerSideManageController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: xiaozhi/admin/server/server-list
        [HttpGet("server-list")]
        public async Task<ActionResult<IEnumerable<string>>> GetWsServerList()
        {
            // Get the SERVER_WEBSOCKET parameter from SysParams
            var wsParam = await _context.SysParams
                .FirstOrDefaultAsync(p => p.ParamCode == "SERVER_WEBSOCKET");

            if (wsParam == null || string.IsNullOrEmpty(wsParam.ParamValue))
            {
                return Ok(new List<string>());
            }

            var wsList = wsParam.ParamValue.Split(';').Where(s => !string.IsNullOrEmpty(s)).ToList();
            return Ok(wsList);
        }

        // POST: xiaozhi/admin/server/emit-action
        [HttpPost("emit-action")]
        public async Task<ActionResult<bool>> EmitServerAction([FromBody] EmitServerActionDTO emitServerActionDTO)
        {
            if (emitServerActionDTO.Action == null)
            {
                return BadRequest("Action cannot be null");
            }

            // Get the SERVER_WEBSOCKET parameter from SysParams
            var wsParam = await _context.SysParams
                .FirstOrDefaultAsync(p => p.ParamCode == "SERVER_WEBSOCKET");

            if (wsParam == null || string.IsNullOrEmpty(wsParam.ParamValue))
            {
                return BadRequest("WebSocket server not configured");
            }

            var wsList = wsParam.ParamValue.Split(';');
            if (string.IsNullOrEmpty(emitServerActionDTO.TargetWs) || 
                !wsList.Contains(emitServerActionDTO.TargetWs))
            {
                return BadRequest("Target WebSocket not found");
            }

            // In a real implementation, this would connect to the WebSocket server and send the action
            // For now, we'll just return true to indicate success
            bool result = EmitServerActionByWs(emitServerActionDTO.TargetWs, emitServerActionDTO.Action);
            return Ok(result);
        }

        private bool EmitServerActionByWs(string targetWsUri, string actionEnum)
        {
            if (string.IsNullOrEmpty(targetWsUri) || string.IsNullOrEmpty(actionEnum))
            {
                return false;
            }

            // Get the server secret
            var serverSecret = _context.SysParams
                .FirstOrDefault(p => p.ParamCode == "SERVER_SECRET")?.ParamValue;

            // In a real implementation, this would:
            // 1. Connect to the WebSocket server
            // 2. Send a JSON payload with the action and secret
            // 3. Wait for a response
            // 4. Return the result

            // For now, we'll just return true to indicate success
            return true;
        }
    }

    // DTO for the emit action request
    public class EmitServerActionDTO
    {
        public string TargetWs { get; set; }
        public string Action { get; set; }
    }
}