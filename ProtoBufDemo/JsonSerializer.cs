using System.Text.Json;
using ProtoBufDemo.Abstractions;
using ProtoBufDemo.DTOs;

namespace ProtoBufDemo
{
    public class JsonWeatherForecastToStringSerializer : ISerializer<WeatherForecast, string>
    {
        public WeatherForecast Deserialize(string value) => JsonSerializer.Deserialize<WeatherForecast>(value);

        public string Serialize(WeatherForecast value) => JsonSerializer.Serialize<WeatherForecast>(value);
    }
}
