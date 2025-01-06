using UnityEngine;

namespace Code.Infrastructure.Pools.Poolable.Factory
{
    public interface IPoolableFactory<T> where T : MonoBehaviour, IPoolable<T>
    {
        int PrefabId { get; }
        T Create();
    }
}