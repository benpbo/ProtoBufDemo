using System;
using System.IO;
using ProtoBuf;
using ProtoBufDemo.Abstractions;

namespace ProtoBufDemo
{
    public class ProtoBufSerializer<T> : ISerializer<T, byte[]>
    {
        public T Deserialize(byte[] value) => Serializer.Deserialize<T>((ReadOnlyMemory<byte>)value);

        public byte[] Serialize(T value)
        {
            using var stream = new MemoryStream();
            Serializer.Serialize(stream, value);
            return stream.ToArray();
        }
    }
}
