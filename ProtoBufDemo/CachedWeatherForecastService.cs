using ProtoBufDemo.Abstractions;
using ProtoBufDemo.DTOs;
using System;
using System.Threading.Tasks;

namespace ProtoBufDemo
{
    public class CachedWeatherForecastService<T> : IWeatherForecastAsyncProvider
    {
        private readonly ISerializer<WeatherForecast, T> _weatherForecastSerializer;
        private readonly IStorage<DateTime, T> _weatherForecastStorage;
        private readonly WeatherForecastService _weatherForecastService;

        protected CachedWeatherForecastService(
            ISerializer<WeatherForecast, T> weatherForecastSerializer,
            IStorage<DateTime, T> weatherForecastStorage,
            WeatherForecastService weatherForecastService)
        {
            _weatherForecastSerializer = weatherForecastSerializer;
            _weatherForecastStorage = weatherForecastStorage;
            _weatherForecastService = weatherForecastService;
        }

        public async Task<WeatherForecast> GetForecastAsync(DateTime forecastDate)
        {
            T cachedWeatherForecast = await _weatherForecastStorage.Retrieve(forecastDate);
            if (cachedWeatherForecast.Equals(default(T)))
            {
                return _weatherForecastSerializer.Deserialize(cachedWeatherForecast);
            }

            WeatherForecast weatherForecast = await _weatherForecastService.GetForecastAsync(forecastDate);
            await _weatherForecastStorage.Store(forecastDate, _weatherForecastSerializer.Serialize(weatherForecast));
            return weatherForecast;
        }
    }
}
