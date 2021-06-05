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

        public CachedWeatherForecastService<string> CreateJsonCachedService()
        {
            return CreateCachedService<string>(new JsonSerializer<WeatherForecast>());
        }

        public CachedWeatherForecastService<byte[]> CreateProtoBufCachedService()
        {
            return CreateCachedService<byte[]>(new ProtoBufSerializer<WeatherForecast>());
        }

        public CachedWeatherForecastService<T> CreateCachedService<T>(ISerializer<WeatherForecast, T> serializer) where T : class
        {
            return new CachedWeatherForecastService<T>(
                serializer,
                CreateStorage<T>(),
                CreateWeatherForecastProvider());
        }

        private IStorage<DateTime, T> CreateStorage<T>() => new InMemoryStorage<DateTime, T>(_storeDelay, _retrieveDelay);

        private IWeatherForecastAsyncProvider CreateWeatherForecastProvider() => new WeatherForecastService(_forecastCreationDelay);
    }
}
