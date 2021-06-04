using System;
using System.Threading.Tasks;

namespace ProtoBufDemo
{
    public class StringStorage : IStorage<string, string>
    {
        public Task<string> Retrieve(string key)
        {
            throw new NotImplementedException();
        }

        public Task Store(string key, string value)
        {
            throw new NotImplementedException();
        }
    }
}
