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
            var bootstrap = new Bootstrap(FORECAST_CREATION_DELAY);

            WeatherForecast weatherForecast = bootstrap.CreateWeatherForecast();

            JsonSerializer<WeatherForecast> jsonSerializer = bootstrap.CreateJsonSerializer();
            Console.WriteLine("Json");
            program.TestAll(weatherForecast, jsonSerializer, bootstrap.CreateCachedService(jsonSerializer));

            ProtoBufSerializer<WeatherForecast> protoBufSerializer = bootstrap.CreateProtoBufSerializer();
            Console.WriteLine("ProtoBuf");
            program.TestAll(weatherForecast, protoBufSerializer, bootstrap.CreateCachedService(protoBufSerializer));
        }

        private void TestAll<T>(WeatherForecast weatherForecast, ISerializer<WeatherForecast, T> serializer, IWeatherForecastAsyncProvider weatherForecastService)
        {
            T serialized = serializer.Serialize(weatherForecast);
            WeatherForecast deserialized = serializer.Deserialize(serialized);

            TimeSpan serializationTime = TestSerializationTime(serializer, weatherForecast);
            TimeSpan deserializationTime = TestDeserializationTime(serializer, serialized);

            TimeSpan executionTime = TestWeatherForecastService(weatherForecastService);

            var outputBuilder = new StringBuilder();
            outputBuilder.AppendLine($"Serialized size: {GetSize(serialized)}")
                         .AppendLine($"Serialization time: {serializationTime}")
                         .AppendLine($"Deserialization time: {deserializationTime}")
                         .AppendLine($"Deserialized value correct: {deserialized.Equals(weatherForecast)}")
                         .AppendLine($"Json caching execution time: {executionTime}");
            Console.WriteLine(outputBuilder);
        }

        private TimeSpan TestSerializationTime<T>(ISerializer<WeatherForecast, T> serializer, WeatherForecast weatherForecast)
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

        private TimeSpan TestWeatherForecastService(IWeatherForecastAsyncProvider service)
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

        private int GetSize<T>(T toMeasure) => toMeasure switch
        {
            byte[] bytesToMeasure => GetByteArraySize(bytesToMeasure),
            string stringToMeasure => GetStringSize(stringToMeasure),
            _ => throw new ArgumentException()
        };

        private int GetStringSize(string stringToMeasure) => sizeof(char) * stringToMeasure.Length;

        private int GetByteArraySize(byte[] bytesToMeasure) => sizeof(byte) * bytesToMeasure.Length;
    }
}
