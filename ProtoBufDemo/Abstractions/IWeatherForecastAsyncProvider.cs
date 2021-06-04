using System;
using System.Threading.Tasks;
using ProtoBufDemo.DTOs;

namespace ProtoBufDemo.Abstractions
{
    public interface IWeatherForecastAsyncProvider
    {
        public Task<WeatherForecast> GetForecastAsync(DateTime forecastDate);
    }
}
