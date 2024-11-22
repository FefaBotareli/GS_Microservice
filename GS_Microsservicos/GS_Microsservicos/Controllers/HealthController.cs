using Microsoft.AspNetCore.Mvc;

namespace GS_Microsservicos.Controllers
{
    [ApiController]
    [Route("health")]
    public class HealthController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetHealthStatus()
        {
            return Ok(new { status = "Service is running", timestamp = DateTime.UtcNow });
        }
    }
}

