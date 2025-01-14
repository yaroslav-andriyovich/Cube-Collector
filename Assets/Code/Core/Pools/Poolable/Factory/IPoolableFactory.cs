using UnityEngine;

namespace Code.Core.Pools.Poolable.Factory
{
    public interface IPoolableFactory<T> where T : Component, IPoolable
    {
        T Prefab { get; }
        T Create();
    }
}