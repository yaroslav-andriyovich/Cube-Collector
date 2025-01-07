using System;
using System.Collections.Generic;
using Code.Core.Pools.Poolable;
using Code.Core.Pools.Poolable.Factory;
using UnityEngine;
using Object = UnityEngine.Object;

namespace Code.Core.Pools
{
    public class MonoPool<T> where T : MonoBehaviour, IPoolable<T>
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

        public T Get(Vector3 at, Quaternion rotation)
        {
            T element = Get();

            element.transform.position = at;
            element.transform.rotation = rotation;

            return element;
        }

        public T Get(Vector3 at, Quaternion rotation, Transform parent)
        {
            T element = Get();

            element.transform.position = at;
            element.transform.rotation = rotation;
            
            if (parent != null)
                element.transform.SetParent(parent);

            return element;
        }

        public void ReturnToPool(T element)
        {
            if (_availableObjects.Contains(element))
                throw new Exception("This object is already in the pool.");

            if (CanReturnObjectToPool())
            {
                element.gameObject.SetActive(false);
                element.transform.SetParent(ObjectsParent);
            
                _availableObjects.Push(element);
                return;
            }
            
            Object.Destroy(element.gameObject);
            --_currentSize;
        }

        public void Warmup(int count)
        {
            for (int i = 0; i < count; i++)
                CreateObject();
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