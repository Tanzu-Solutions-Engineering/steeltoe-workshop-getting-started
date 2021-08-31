using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;

using System.Threading.Tasks;
using WeatherService.Models;

namespace WeatherService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private readonly ILogger<WeatherForecastController> _logger;
        private readonly WeatherContext _dbcontext;

        public WeatherForecastController(ILogger<WeatherForecastController> logger, WeatherContext dbcontext)
        {
            _logger = logger;
            _dbcontext = dbcontext;
        }

        [HttpGet]
        public async Task<IEnumerable<WeatherForecast>> Get()
        {
            _logger.LogDebug("Weather is good");
            return await _dbcontext.WeatherForecasts.ToListAsync();
        }
    }
}