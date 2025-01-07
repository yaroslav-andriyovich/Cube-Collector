using UnityEngine;

namespace Code.Core.Pools.Poolable.Factory
{
    public interface IPoolableFactory<T> where T : MonoBehaviour, IPoolable<T>
    {
        T Prefab { get; }
        T Create();
    }
}