using System;
using ProtoBufDemo.Abstractions;
using ProtoBufDemo.DTOs;

namespace ProtoBufDemo
{
    public class Bootstrap
    {
        private readonly int _forecastCreationDelay;
        private readonly int _storeDelay;
        private readonly int _retrieveDelay;

        public Bootstrap(int forecastCreationDelay, int storeDelay, int retrieveDelay)
        {
            _forecastCreationDelay = forecastCreationDelay;
            _storeDelay = storeDelay;
            _retrieveDelay = retrieveDelay;
        }

        public CachedWeatherForecastService<string> CreateJsonCachedService() => new CachedWeatherForecastService<string>(
                new JsonSerializer<WeatherForecast>(),
                CreateStorage<string>(),
                CreateWeatherForecastProvider());

        public CachedWeatherForecastService<byte[]> CreateProtoBufCachedService() => new CachedWeatherForecastService<byte[]>(
                new ProtoBufSerializer<WeatherForecast>(),
                CreateStorage<byte[]>(),
                CreateWeatherForecastProvider());

        private IWeatherForecastAsyncProvider CreateWeatherForecastProvider() => new WeatherForecastService(_forecastCreationDelay);

        private IStorage<DateTime, T> CreateStorage<T>() => new InMemoryStorage<DateTime, T>(_storeDelay, _retrieveDelay);
    }
}
