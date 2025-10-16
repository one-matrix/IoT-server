using Microsoft.AspNetCore.Mvc;

namespace IotApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok(new { message = "IoT API is running successfully!", timestamp = DateTime.Now });
        }
    }
}