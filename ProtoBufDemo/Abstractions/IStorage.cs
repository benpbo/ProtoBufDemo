using System.Threading.Tasks;

namespace ProtoBufDemo.Abstractions
{
    public interface IStorage<K, V>
    {
        Task Store(K key, V value);

        Task<V> Retrieve(K key);
    }
}
