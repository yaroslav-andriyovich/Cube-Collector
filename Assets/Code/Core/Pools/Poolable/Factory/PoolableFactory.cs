using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Core.Pools.Poolable.Factory
{
    public class PoolableFactory<T> : IPoolableFactory<T> where T : Component, IPoolable
    {
        public T Prefab { get; }

        protected readonly IObjectResolver objectResolver;

        public PoolableFactory(IObjectResolver objectResolver, T prefab)
        {
            this.objectResolver = objectResolver;
            Prefab = prefab;
        }

        public virtual T Create()
        {
            T poolable = objectResolver.Instantiate(Prefab);
            
            return poolable;
        }
    }
}