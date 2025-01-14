using System;
using System.Collections.Generic;
using Code.Core.Pools.Poolable;
using Code.Core.Pools.Poolable.Factory;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Core.Pools
{
    public class MonoPool<T> where T : Component, IPoolable
    {
        public Transform ObjectsParent { get; private set; }
        
        private readonly IPoolableFactory<T> _poolableFactory;
        private readonly int _maxSize;
        private readonly bool _autoExpand;
        private readonly Stack<T> _availableObjects;

        private int _currentSize;

        public MonoPool(IPoolableFactory<T> poolableFactory, int maxSize = 0, bool autoExpand = true)
        {
            _poolableFactory = poolableFactory;
            _maxSize = maxSize;
            _autoExpand = autoExpand;
            _availableObjects = new Stack<T>();
            _currentSize = 0;

            CreateObjectsParent();
        }
        
        public T Get()
        {
            if (HasAvailableObjects())
                return GetAvailableObject();

            if (CanCreateNewObject())
            {
                CreateObject();
                return GetAvailableObject();
            }

            throw new Exception("Pool limit reached and autoExpand is disabled.");
        }

        public void ReturnToPool(IPoolable poolable)
        {
            T type = poolable as T;
            
            if (poolable is null)
                return;

            if (_availableObjects.Contains(type))
                return;

            if (CanReturnObjectToPool())
            {
                type.gameObject.SetActive(false);
                type.transform.SetParent(ObjectsParent);
            
                _availableObjects.Push(type);
                return;
            }
            
            Object.Destroy(type.gameObject);
            --_currentSize;
        }

        public void Warmup(int count)
        {
            for (int i = 0; i < count; i++)
            {
                if (!CanCreateNewObject())
                    break; 
                
                CreateObject();
            }
        }

        private void CreateObjectsParent()
        {
            GameObject gameObject = new GameObject("[Pool] " + typeof(T).Name);

            ObjectsParent = gameObject.transform;
        }

        private T CreateObject()
        {
            T createdObject = _poolableFactory.Create();

            createdObject.Initialize(ReturnToPool);
            createdObject.transform.SetParent(ObjectsParent);
            createdObject.gameObject.SetActive(false);

            _availableObjects.Push(createdObject);
            ++_currentSize;
            
            return createdObject;
        }

        private T GetAvailableObject()
        {
            T element = _availableObjects.Pop();
            
            element.gameObject.SetActive(true);
            
            return element;
        }

        private bool HasAvailableObjects() => 
            _availableObjects.Count > 0;

        private bool CanCreateNewObject() => 
            _currentSize < _maxSize || _autoExpand;

        private bool CanReturnObjectToPool() => 
            _currentSize <= _maxSize || _autoExpand;
    }
}