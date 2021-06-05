using System;
using ProtoBufDemo.Abstractions;
using ProtoBufDemo.DTOs;

namespace ProtoBufDemo
{
    public class Bootstrap
    {
        private readonly int _forecastCreationDelay;

        public Bootstrap(int forecastCreationDelay)
        {
            _forecastCreationDelay = forecastCreationDelay;
        }
        public JsonSerializer<WeatherForecast> CreateJsonSerializer() => new();

        public ProtoBufSerializer<WeatherForecast> CreateProtoBufSerializer() => new();

        public CachedWeatherForecastService<T> CreateCachedService<T>(ISerializer<WeatherForecast, T> serializer) where T : class
        {
            return new CachedWeatherForecastService<T>(
                serializer,
                CreateStorage<T>(),
                CreateWeatherForecastProvider());
        }

        private IStorage<DateTime, T> CreateStorage<T>() => new InMemoryStorage<DateTime, T>();

        private IWeatherForecastAsyncProvider CreateWeatherForecastProvider() => new WeatherForecastService(_forecastCreationDelay);
    }
}
