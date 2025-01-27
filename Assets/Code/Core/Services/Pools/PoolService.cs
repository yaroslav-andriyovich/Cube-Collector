using System;
using System.Collections.Generic;
using System.Linq;
using Code.Core.Pools;
using Code.Core.Pools.Poolable;
using Code.Core.Pools.Poolable.Factory;
using UnityEngine;
using VContainer;
using Random = UnityEngine.Random;

namespace Code.Core.Services.Pools
{
    public class PoolService
    {
        private readonly Dictionary<Type, Dictionary<int, object>> _pools;
        private readonly IObjectResolver _objectResolver;

        private Transform _monoContainer;

        public PoolService(IObjectResolver objectResolver)
        {
            _pools = new Dictionary<Type, Dictionary<int, object>>();
            _objectResolver = objectResolver;
            
            CreateMonoContainer();
        }

        public T Spawn<T>() where T : Component, IPoolable
        {
            MonoPool<T> pool = GetRandomPool<T>();

            return pool.Get();
        }

        public T Spawn<T>(T prefab) where T : Component, IPoolable
        {
            MonoPool<T> pool = GetOrCreatePool(prefab);

            return pool.Get();
        }
        
        public T Spawn<T>(T prefab, Vector3 at, Quaternion rotation) where T : Component, IPoolable
        {
            T element = Spawn(prefab);
            Transform transform = element.transform;

            transform.position = at;
            transform.rotation = rotation;

            return element;
        }
        
        public T Spawn<T>(T prefab, Vector3 at, Quaternion rotation, Transform parent) where T : Component, IPoolable
        {
            T element = Spawn(prefab);
            Transform transform = element.transform;

            transform.position = at;
            transform.rotation = rotation;
            transform.SetParent(parent);

            return element;
        }

        public void Warmup<T>(T prefab, int count) where T : Component, IPoolable
        {
            MonoPool<T> pool = GetOrCreatePool(prefab);

            pool.Warmup(count);
        }

        public MonoPool<T> CreatePool<T>(T prefab, int maxSize = 0, bool autoExpand = true) where T : Component, IPoolable
        {
            IPoolableFactory<T> poolableFactory = new PoolableFactory<T>(_objectResolver, prefab);

            return CreatePool(poolableFactory, maxSize, autoExpand);
        }

        public MonoPool<T> CreatePool<T>(IPoolableFactory<T> poolableFactory, int maxSize = 0, bool autoExpand = true) where T : Component, IPoolable
        {
            Type type = typeof(T);
            GameObject gameObject = poolableFactory.Prefab.gameObject;

            if (!IsTypeRegistered(type))
            {
                _pools[type] = new Dictionary<int, object>();
            }

            if (IsObjectRegisteredByType(gameObject, type))
                throw new Exception($"Pool of type {type} with prefab {gameObject.name} already created");

            MonoPool<T> pool = new MonoPool<T>(poolableFactory, maxSize, autoExpand);

            pool.ObjectsParent.SetParent(_monoContainer);
            _pools[type][gameObject.GetInstanceID()] = pool;

            return pool;
        }

        public bool TryGetPool<T>(T prefab, out MonoPool<T> pool) where T : Component, IPoolable
        {
            Type type = typeof(T);
            GameObject gameObject = prefab.gameObject;

            if (IsPoolCreated(prefab, type))
            {
                pool = (MonoPool<T>)_pools[type][gameObject.GetInstanceID()];
                return true;
            }

            pool = null;
            return false;
        }

        public MonoPool<T> GetRandomPool<T>() where T : Component, IPoolable
        {
            Type type = typeof(T);

            if (_pools.TryGetValue(type, out Dictionary<int, object> pools))
            {
                return (MonoPool<T>)pools.Values.ElementAt(Random.Range(0, pools.Count));
            }
            
            throw new KeyNotFoundException($"No pool found for type {type}");
        }

        private void CreateMonoContainer()
        {
            GameObject gameObject = new GameObject("[Pool Service]");

            _monoContainer = gameObject.transform;
        }

        private bool IsTypeRegistered(Type type) => 
            _pools.ContainsKey(type);

        private bool IsObjectRegisteredByType(GameObject gameObject, Type type)
        {
            return _pools[type].ContainsKey(gameObject.GetInstanceID());
        }

        private MonoPool<T> GetOrCreatePool<T>(T prefab) where T : Component, IPoolable
        {
            if (!TryGetPool(prefab, out MonoPool<T> pool))
                pool = CreatePool(prefab);

            return pool;
        }

        private bool IsPoolCreated<T>(T prefab, Type type) where T : Component, IPoolable => 
            IsTypeRegistered(type) && IsObjectRegisteredByType(prefab.gameObject, type);
    }
}