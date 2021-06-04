using System.Threading.Tasks;

namespace ProtoBufDemo
{
    public interface IStorage<K, V>
    {
        Task Store(K key, V value);

        Task<V> Retrieve(K key);
    }
}
