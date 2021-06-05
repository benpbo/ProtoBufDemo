using ProtoBufDemo.Abstractions;
using ProtoBufDemo.DTOs;
using System;
using System.Threading.Tasks;

namespace ProtoBufDemo
{
    public class CachedWeatherForecastService<T> : IWeatherForecastAsyncProvider where T : class
    {
        private readonly ISerializer<WeatherForecast, T> _weatherForecastSerializer;
        private readonly IStorage<DateTime, T> _weatherForecastStorage;
        private readonly IWeatherForecastAsyncProvider _weatherForecastService;

        public CachedWeatherForecastService(
            ISerializer<WeatherForecast, T> weatherForecastSerializer,
            IStorage<DateTime, T> weatherForecastStorage,
            IWeatherForecastAsyncProvider weatherForecastService)
        {
            _weatherForecastSerializer = weatherForecastSerializer;
            _weatherForecastStorage = weatherForecastStorage;
            _weatherForecastService = weatherForecastService;
        }

        public async Task<WeatherForecast> GetForecastAsync(DateTime forecastDate)
        {
            T cachedWeatherForecast = await _weatherForecastStorage.Retrieve(forecastDate);
            if (cachedWeatherForecast is not null)
            {
                return _weatherForecastSerializer.Deserialize(cachedWeatherForecast);
            }

            WeatherForecast weatherForecast = await _weatherForecastService.GetForecastAsync(forecastDate);
            await _weatherForecastStorage.Store(forecastDate, _weatherForecastSerializer.Serialize(weatherForecast));
            return weatherForecast;
        }
    }
}
