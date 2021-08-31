using Microsoft.Extensions.Logging;
using Steeltoe.Common;
using System;
using System.Linq;
using WeatherService.Models;

namespace WeatherService
{
    public class SeedTask : IApplicationTask
    {
        private readonly WeatherContext _context;
        private readonly ILogger<SeedTask> _logger;

        public string Name => "seed";
        public SeedTask(WeatherContext context, ILogger<SeedTask> logger)
        {
            _context = context;
            _logger = logger;
        }

        public void Run()
        {

            if (_context.Database.EnsureCreated())
            {
                _logger.LogInformation("Database created");
                string[] Summaries = new[] { "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching" };
                var rng = new Random();
                var forecasts = Enumerable.Range(1, 5)
                    .Select(index => new WeatherForecast
                    {
                        Date = DateTime.Now.AddDays(index),
                        TemperatureC = rng.Next(-20, 55),
                        Summary = Summaries[rng.Next(Summaries.Length)]
                    })
                    .ToArray();
                _context.WeatherForecasts.AddRange(forecasts);
                _context.SaveChanges();
                _logger.LogInformation($"Database seeded with {forecasts.Length} records");
            }
            else
            {
                _logger.LogInformation("Database already exists");
            }
        }
    }
}


