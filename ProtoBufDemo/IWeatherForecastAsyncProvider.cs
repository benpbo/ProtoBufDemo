using System;
using System.Threading.Tasks;

namespace ProtoBufDemo
{
    public interface IWeatherForecastAsyncProvider
    {
        public Task<WeatherForecast> GetForecastAsync(DateTime forecastDate);
    }
}
