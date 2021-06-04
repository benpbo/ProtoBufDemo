using ProtoBufDemo.Abstractions;
using ProtoBufDemo.DTOs;
using System;
using System.Threading.Tasks;

namespace ProtoBufDemo
{
    public class JsonCachedWeatherForecastService : IWeatherForecastAsyncProvider
    {
        private readonly IStorage<string, string> _weatherForecastStorage;
        private readonly WeatherForecastService _weatherForecastService;

        public JsonCachedWeatherForecastService(IStorage<string, string> weatherForecastStorage, WeatherForecastService weatherForecastService)
        {
            _weatherForecastStorage = weatherForecastStorage;
            _weatherForecastService = weatherForecastService;
        }

        public Task<WeatherForecast> GetForecastAsync(DateTime forecastDate)
        {
            throw new NotImplementedException();
        }
    }
}
