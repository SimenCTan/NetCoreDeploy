using Microsoft.AspNetCore.Mvc;

namespace ConsulService.Controllers
{
    [Route("[Controller]")]
    public class HealthCheckController:Controller
    {
        [HttpGet("")]
        [HttpHead("")]
        public IActionResult Ping()
        {
            return Ok("this service healthy");
        }
    }
}