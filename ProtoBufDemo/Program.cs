using System;
using System.Diagnostics;
using System.Linq;
using System.Text;
using ProtoBufDemo.Abstractions;
using ProtoBufDemo.DTOs;

namespace ProtoBufDemo
{
    public class Program
    {
        public const int FORECAST_CREATION_DELAY = 200;
        public const int ROUNDS_IN_TEST = 1_000_000;

        public static void Main()
        {
            var program = new Program();
            Bootstrap bootstrap = new Bootstrap(FORECAST_CREATION_DELAY);

            JsonSerializer<WeatherForecast> jsonSerializer = bootstrap.CreateJsonSerializer();
            CachedWeatherForecastService<string> jsonCachedService = bootstrap.CreateCachedService(jsonSerializer);

            ProtoBufSerializer<WeatherForecast> protoBufSerializer = bootstrap.CreateProtoBufSerializer();
            CachedWeatherForecastService<byte[]> protoCachedService = bootstrap.CreateCachedService(protoBufSerializer);

            var outputBuilder = new StringBuilder();

            var weatherForecast = new WeatherForecast()
            {
                Date = DateTime.Today,
                TemperatureC = 22,
                Summary = WeatherForecastSummary.Mild
            };

            string jsonString = jsonSerializer.Serialize(weatherForecast);
            byte[] byteArray = protoBufSerializer.Serialize(weatherForecast);
            var deserializedJsonWeatherForecast = jsonSerializer.Deserialize(jsonString);
            var deserializedProtoBufWeatherForecast = protoBufSerializer.Deserialize(byteArray);
            TimeSpan jsonCachedServiceExecutionTime = program.TestWeatherForecastService(jsonCachedService);
            TimeSpan protoBufCachedServiceExecutionTime = program.TestWeatherForecastService(protoCachedService);

            outputBuilder.AppendLine($"Serialized Json size: {program.GetStringSize(jsonString)}")
                         .AppendLine($"Serialized ProtoBuf size: {program.GetByteArraySize(byteArray)}")
                         .AppendLine($"Deserialized values equal: {deserializedJsonWeatherForecast.Equals(deserializedProtoBufWeatherForecast)}")
                         .AppendLine($"Json caching execution time: {jsonCachedServiceExecutionTime}")
                         .AppendLine($"ProtoBuf caching execution time: {protoBufCachedServiceExecutionTime}");
            Console.WriteLine(outputBuilder);
        }

        public TimeSpan TestWeatherForecastService(IWeatherForecastAsyncProvider service)
        {
            var initialDay = DateTime.Today;
            var days = Enumerable.Range(0, 7).Select(dayNumber => initialDay.AddDays(dayNumber));

            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < ROUNDS_IN_TEST; i++)
            {
                foreach (var day in days)
                {
                    service.GetForecastAsync(day).Wait();
                }
            }

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        public int GetStringSize(string stringToMeasure) => sizeof(char) * stringToMeasure.Length;

        public int GetByteArraySize(byte[] byteArrayToMeasure) => sizeof(byte) * byteArrayToMeasure.Length;
    }
}
