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
            WeatherForecast deserializedJsonWeatherForecast = jsonSerializer.Deserialize(jsonString);
            byte[] protoBufbyteArray = protoBufSerializer.Serialize(weatherForecast);
            WeatherForecast deserializedProtoBufWeatherForecast = protoBufSerializer.Deserialize(protoBufbyteArray);

            TimeSpan jsonSerializationTime = program.TestSerializationTime(jsonSerializer, weatherForecast);
            TimeSpan jsonDeserializationTime = program.TestDeserializationTime(jsonSerializer, jsonString);
            TimeSpan protoBufSerializationTime = program.TestSerializationTime(protoBufSerializer, weatherForecast);
            TimeSpan protoBufDeserializationTime = program.TestDeserializationTime(protoBufSerializer, protoBufbyteArray);

            TimeSpan jsonCachedServiceExecutionTime = program.TestWeatherForecastService(jsonCachedService);
            TimeSpan protoBufCachedServiceExecutionTime = program.TestWeatherForecastService(protoCachedService);

            outputBuilder.AppendLine($"Serialized Json size: {program.GetStringSize(jsonString)}")
                         .AppendLine($"Serialized ProtoBuf size: {program.GetByteArraySize(protoBufbyteArray)}")
                         .AppendLine($"Json serialization time: {jsonSerializationTime}")
                         .AppendLine($"ProtoBuf serialization time: {protoBufSerializationTime}")
                         .AppendLine($"Json deserialization time: {jsonDeserializationTime}")
                         .AppendLine($"ProtoBuf deserialization time: {protoBufDeserializationTime}")
                         .AppendLine($"Deserialized values equal: {deserializedJsonWeatherForecast.Equals(deserializedProtoBufWeatherForecast)}")
                         .AppendLine($"Json caching execution time: {jsonCachedServiceExecutionTime}")
                         .AppendLine($"ProtoBuf caching execution time: {protoBufCachedServiceExecutionTime}");
            Console.WriteLine(outputBuilder);
        }

        public TimeSpan TestSerializationTime<T>(ISerializer<WeatherForecast, T> serializer, WeatherForecast weatherForecast)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < ROUNDS_IN_TEST; i++)
            {
                serializer.Serialize(weatherForecast);
            }

            stopwatch.Stop();
            return stopwatch.Elapsed;
        }

        private TimeSpan TestDeserializationTime<T>(ISerializer<WeatherForecast, T> serializer, T serialized)
        {
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            for (int i = 0; i < ROUNDS_IN_TEST; i++)
            {
                serializer.Deserialize(serialized);
            }

            stopwatch.Stop();
            return stopwatch.Elapsed;
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
