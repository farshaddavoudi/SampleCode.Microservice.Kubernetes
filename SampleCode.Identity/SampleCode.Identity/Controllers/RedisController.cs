using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;

namespace SampleCode.Identity.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RedisController(IDistributedCache cache, ILogger<WeatherForecastController> logger) : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            var val = "If you see this, the app is able to connect to Redis DB successfully";
            cache.SetString("K8sTest", val, new DistributedCacheEntryOptions { SlidingExpiration = TimeSpan.FromSeconds(5) });
            var fromRedis = cache.GetString("K8sTest");
            if (fromRedis == val)
            {
                return Ok("Pong from Redis");
            }
            else
            {
                return Ok("Something is wrong. The K8sTest key didn't set in Redis, probably due to connection issues. Check out the Redis pod");
            }
        }
    }
}
