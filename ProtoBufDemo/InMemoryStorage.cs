using System.Collections.Generic;
using System.Threading.Tasks;
using ProtoBufDemo.Abstractions;

namespace ProtoBufDemo
{
    public class InMemoryStorage<K, V> : IStorage<K, V>
    {
        private readonly IDictionary<K, V> data = new Dictionary<K, V>();

        public InMemoryStorage()
        {
        }

        public Task<V> Retrieve(K key) => Task.FromResult(data.TryGetValue(key, out V value) ? value : default);

        public Task Store(K key, V value)
        {
            data[key] = value;
            return Task.CompletedTask;
        }
    }
}
