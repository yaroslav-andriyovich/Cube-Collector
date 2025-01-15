using System;
using DG.Tweening;
using UnityEngine;

namespace Code.Core.Pools.Poolable
{
    public abstract class PoolableBase : MonoBehaviour, IPoolable
    {
        private Action<IPoolable> _returnToPool;

        void IPoolable.Initialize(Action<IPoolable> returnAction) => 
            _returnToPool = returnAction;

        public void Release()
        {
            if (_returnToPool != null)
            {
                _returnToPool(this);
                return;
            }
            
            Destroy(gameObject);
        }

        public void Release(float delay) => 
            DOVirtual
                .DelayedCall(delay, Release, ignoreTimeScale: false)
                .SetLink(gameObject);

        public void Release(float delay, Action delayedAction) => 
            DOVirtual.DelayedCall(delay, () =>
            {
                delayedAction?.Invoke();
                Release();
            }, ignoreTimeScale: false).SetLink(gameObject);
    }
}