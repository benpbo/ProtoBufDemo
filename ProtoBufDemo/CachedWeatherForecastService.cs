using ProtoBufDemo.Abstractions;
using ProtoBufDemo.DTOs;
using System;
using System.Threading.Tasks;

namespace ProtoBufDemo
{
    public class CachedWeatherForecastService : IWeatherForecastAsyncProvider
    {
        private readonly ISerializer<WeatherForecast, string> _weatherForecastSerializer;
        private readonly IStorage<DateTime, string> _weatherForecastStorage;
        private readonly WeatherForecastService _weatherForecastService;

        protected CachedWeatherForecastService(
            ISerializer<WeatherForecast, string> weatherForecastSerializer,
            IStorage<DateTime, string> weatherForecastStorage,
            WeatherForecastService weatherForecastService)
        {
            _weatherForecastSerializer = weatherForecastSerializer;
            _weatherForecastStorage = weatherForecastStorage;
            _weatherForecastService = weatherForecastService;
        }

        public async Task<WeatherForecast> GetForecastAsync(DateTime forecastDate)
        {
            string cachedWeatherForecast = await _weatherForecastStorage.Retrieve(forecastDate);
            if (!string.IsNullOrEmpty(cachedWeatherForecast))
            {
                return _weatherForecastSerializer.Deserialize(cachedWeatherForecast);
            }

            WeatherForecast weatherForecast = await _weatherForecastService.GetForecastAsync(forecastDate);
            await _weatherForecastStorage.Store(forecastDate, _weatherForecastSerializer.Serialize(weatherForecast));
            return weatherForecast;
        }
    }
}
