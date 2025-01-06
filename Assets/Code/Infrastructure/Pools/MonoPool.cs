using System;
using System.Collections.Generic;
using System.Linq;
using Code.Infrastructure.Pools.Poolable;
using Code.Infrastructure.Pools.Poolable.Factory;
using UnityEngine;

namespace Code.Infrastructure.Pools
{
    public class MonoPool<T> where T : MonoBehaviour, IPoolable<T>
    {
        private readonly bool _autoExpand;
        private readonly IPoolableFactory<T> _poolableFactory;

        private Transform _parent;
        private List<T> _pool;

        public MonoPool(IPoolableFactory<T> poolableFactory, int count, bool autoExpand = true)
        {
            _poolableFactory = poolableFactory;
            _autoExpand = autoExpand;

            CreatePoolParent();
            CreatePool(count);
        }
        
        public T Get()
        {
            if (TryGetFreeElement(out T element))
                return element;
 
            if (_autoExpand)
                return CreateObject(true);
 
            throw new Exception("There is no free element in pool");
        }

        public T Get(Vector3 at, Quaternion rotation)
        {
            T element = Get();

            element.transform.position = at;
            element.transform.rotation = rotation;

            return element;
        }

        public void Release(T element)
        {
            element.gameObject.SetActive(false);

            /*if (_parent != null)
                element.transform.parent = _parent;*/
        }

        private void CreatePoolParent()
        {
            GameObject gameObject = new GameObject("[Pool] " + typeof(T).Name);

            _parent = gameObject.transform;
        }

        private void CreatePool(int count)
        {
            _pool = new List<T>();
            
            for (int i = 0; i < count; i++)
                CreateObject();
        }

        private T CreateObject(bool isActiveByDefault = false)
        {
            T createdObject = _poolableFactory.Create();
            
            createdObject.gameObject.SetActive(isActiveByDefault);
            createdObject.transform.SetParent(_parent);
            createdObject.Initialize(Release);
            
            _pool.Add(createdObject);
            return createdObject;
        }

        private bool TryGetFreeElement(out T element)
        {
            T poolable = _pool.FirstOrDefault(x => !x.isActiveAndEnabled);

            if (poolable == null)
            {
                element = null;
                return false;
            }

            element = poolable;
            poolable.gameObject.SetActive(true);
            return true;
        }
    }
}