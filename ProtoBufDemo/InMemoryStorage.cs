using System.Collections.Generic;
using System.Threading.Tasks;
using ProtoBufDemo.Abstractions;

namespace ProtoBufDemo
{
    public class InMemoryStorage<K, V> : IStorage<K, V>
    {
        private readonly IDictionary<K, V> data = new Dictionary<K, V>();

        private readonly int _storeDelay;
        private readonly int _retrieveDelay;

        public InMemoryStorage(int storeDelay, int retrieveDelay)
        {
           _storeDelay = storeDelay;
           _retrieveDelay = retrieveDelay;
        }

        public async Task<V> Retrieve(K key)
        {
            await Task.Delay(_retrieveDelay);
            return data.TryGetValue(key, out V value) ? value : default;
        }

        public async Task Store(K key, V value)
        {
            await Task.Delay(_storeDelay);
            data[key] = value;
        }
    }
}
