using System;
using ProtoBuf;

namespace ProtoBufDemo.DTOs
{
    [ProtoContract]
    public class WeatherForecast
    {
        [ProtoMember(1)]
        public DateTime Date { get; set; }

        [ProtoMember(2)]
        public int TemperatureC { get; set; }

        public int TemperatureF => 32 + (int)(TemperatureC / 0.5556);

        [ProtoMember(3)]
        public WeatherForecastSummary Summary { get; set; }
    }
}
