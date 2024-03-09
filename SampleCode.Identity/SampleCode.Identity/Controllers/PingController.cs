using Microsoft.AspNetCore.Mvc;

namespace SampleCode.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PingController(ILogger<WeatherForecastController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Ok("Pong from Identity");
        }
    }
}
