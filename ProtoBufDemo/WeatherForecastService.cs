using System;
using System.Threading.Tasks;
using ProtoBufDemo.Abstractions;
using ProtoBufDemo.DTOs;

namespace ProtoBufDemo
{
    public class WeatherForecastService : IWeatherForecastAsyncProvider
    {
        private readonly WeatherForecastSummary[] _summaries = Enum.GetValues<WeatherForecastSummary>();

        private readonly int _forecastCreationDelay;

        public WeatherForecastService(int forecastCreationDelay)
        {
            _forecastCreationDelay = forecastCreationDelay;
        }

        public async Task<WeatherForecast> GetForecastAsync(DateTime forecastDate)
        {
            await Task.Delay(_forecastCreationDelay);
            var _rng = new Random();
            int summaryIndex = _rng.Next(_summaries.Length);
            return new WeatherForecast()
            {
                Date = forecastDate.Date,
                TemperatureC = summaryIndex * 5 + _rng.Next(5),
                Summary = _summaries[summaryIndex]
            };
        }
    }
}
