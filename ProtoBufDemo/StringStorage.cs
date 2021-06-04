using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ProtoBufDemo.Abstractions;

namespace ProtoBufDemo
{
    public class StringStorage : IStorage<string, string>
    {
        private readonly IDictionary<string, string> data = new Dictionary<string, string>();

        private readonly int _storeDelay;
        private readonly int _retrieveDelay;

        public StringStorage(int storeDelay, int retrieveDelay)
        {
           _storeDelay = storeDelay;
           _retrieveDelay = retrieveDelay;
        }

        public async Task<string> Retrieve(string key)
        {
            await Task.Delay(_retrieveDelay);
            return data.TryGetValue(key, out string value) ?
                value : throw new InvalidOperationException("Key not found in storage");
        }

        public async Task Store(string key, string value)
        {
            await Task.Delay(_storeDelay);
            data[key] = value;
        }
    }
}
