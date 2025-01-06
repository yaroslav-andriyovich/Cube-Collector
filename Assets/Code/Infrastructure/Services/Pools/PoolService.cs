using System;
using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.Pools;
using Code.Infrastructure.Pools.Poolable;
using Code.Infrastructure.Pools.Poolable.Factory;
using UnityEngine;
using VContainer;

namespace Code.Infrastructure.Services.Pools
{
    public class PoolService
    {
        private readonly Dictionary<Type, Dictionary<int, object>> _pools;
        private readonly IObjectResolver _objectResolver;

        public PoolService(IObjectResolver objectResolver)
        {
            _pools = new Dictionary<Type, Dictionary<int, object>>();
            _objectResolver = objectResolver;
        }
        
        public MonoPool<T> CreatePool<T>(GameObject prefab, int count, bool autoExpand = true) where T : MonoBehaviour, IPoolable<T>
        {
            IPoolableFactory<T> poolableFactory = new PoolableFactory<T>(_objectResolver, prefab.GetComponent<T>());

            return CreatePool(poolableFactory, count, autoExpand);
        }

        public MonoPool<T> CreatePool<T>(T prefab, int count, bool autoExpand = true) where T : MonoBehaviour, IPoolable<T>
        {
            IPoolableFactory<T> poolableFactory = new PoolableFactory<T>(_objectResolver, prefab);

            return CreatePool(poolableFactory, count, autoExpand);
        }

        public MonoPool<T> CreatePool<T>(IPoolableFactory<T> poolableFactory, int count, bool autoExpand = true) where T : MonoBehaviour, IPoolable<T>
        {
            Type type = typeof(T);
            int prefabId = poolableFactory.PrefabId;
            
            if (!_pools.ContainsKey(type))
            {
                _pools[type] = new Dictionary<int, object>();
            }
            
            if (_pools[type].ContainsKey(prefabId))
                throw new Exception($"Pool of type {type} with prefab ID {prefabId} already created");

            MonoPool<T> pool = new MonoPool<T>(poolableFactory, count, autoExpand);

            _pools[type][prefabId] = pool;
            
            return pool;
        }

        public MonoPool<T> GetPool<T>() where T : MonoBehaviour, IPoolable<T>
        {
            if (_pools.TryGetValue(typeof(T), out Dictionary<int, object> prefabs))
            {
                return (MonoPool<T>)prefabs.Values.First();
            }

            throw new Exception($"No pool found for type {typeof(T)}");
        }
        
        public MonoPool<T> GetPool<T>(T prefab) where T : MonoBehaviour, IPoolable<T>
        {
            Type type = typeof(T);
            int prefabId = prefab.GetInstanceID();

            if (_pools.ContainsKey(type) && _pools[type].ContainsKey(prefabId))
            {
                return (MonoPool<T>)_pools[type][prefabId];
            }

            throw new KeyNotFoundException($"No pool found for type {type} with prefab {prefab.name}({prefabId})");
        }
        
        public MonoPool<T> GetPool<T>(GameObject prefab) where T : MonoBehaviour, IPoolable<T>
        {
            Type type = typeof(T);
            T component = prefab.GetComponent<T>();
            int prefabId = component.GetInstanceID();

            if (_pools.ContainsKey(type) && _pools[type].ContainsKey(prefabId))
            {
                return (MonoPool<T>)_pools[type][prefabId];
            }

            throw new KeyNotFoundException($"No pool found for type {type} with prefab {prefab.name}({prefabId})");
        }
    }
}