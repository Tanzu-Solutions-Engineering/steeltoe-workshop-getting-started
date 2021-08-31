using Microsoft.EntityFrameworkCore;

namespace WeatherService.Models
{
    public class WeatherContext : DbContext
    {
        public WeatherContext() : base() { }
        public WeatherContext(DbContextOptions<WeatherContext> options) : base(options)
        {
        }

        public DbSet<WeatherForecast> WeatherForecasts { get; set; }
    }
}