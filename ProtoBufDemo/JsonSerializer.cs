using System.Text.Json;
using ProtoBufDemo.Abstractions;

namespace ProtoBufDemo
{
    public class JsonSerializer<T> : ISerializer<T, string>
    {
        public T Deserialize(string value) => JsonSerializer.Deserialize<T>(value);

        public string Serialize(T value) => JsonSerializer.Serialize<T>(value);
    }
}
