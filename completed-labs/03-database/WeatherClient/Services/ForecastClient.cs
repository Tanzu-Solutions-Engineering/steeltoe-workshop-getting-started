using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace WeatherClient.Services
{
    public class ForecastClient
    {
        private HttpClient _httpClient;
        private ILogger _logger;
        public ForecastClient(HttpClient httpClient, ILogger<ForecastClient> logger)
        {
            _httpClient = httpClient;
            _logger = logger;
        }

        public async Task<IEnumerable<WeatherForecast>> GetForecast()
        {
            var result = await _httpClient.GetFromJsonAsync<List<WeatherForecast>>("/weatherforecast");
            _logger.LogInformation($"Received {result.Count} forecasts");
            return result;
        }
    }
}