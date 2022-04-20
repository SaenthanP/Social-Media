using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace FeedService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {
        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly IDatabase _redis;

        public WeatherForecastController(ILogger<WeatherForecastController> logger,IConnectionMultiplexer muxer)
        {
            _logger = logger;
            _redis=muxer.GetDatabase();
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            var rng = new Random();
            var setTask = _redis.StringSetAsync("key", "e");
            var result2=_redis.ListRightPushAsync("key2","e2");
            var result3=_redis.ListRightPushAsync("key2","e3426");

            // var expireTask = _redis.KeyExpireAsync("key", TimeSpan.FromSeconds(3600));
            await Task.WhenAll(setTask,result2,result3);
            return null;
        }
    }
}
