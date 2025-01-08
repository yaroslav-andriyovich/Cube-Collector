using System;
using DG.Tweening;
using UnityEngine;

namespace Code.Core.Pools.Poolable
{
    public abstract class PoolableBase<T> : MonoBehaviour, IPoolable<T> where T : PoolableBase<T>
    {
        private Action<T> _returnToPool;

        void IPoolable<T>.Initialize(Action<T> returnAction) => 
            _returnToPool = returnAction;

        public void Release()
        {
            if (_returnToPool != null)
            {
                _returnToPool((T)this);
                return;
            }
            
            Destroy(gameObject);
        }

        protected void Release(float delay) => 
            DOVirtual.DelayedCall(delay, Release, ignoreTimeScale: false);
        
        protected void Release(float delay, Action delayedAction) => 
            DOVirtual.DelayedCall(delay, () =>
            {
                delayedAction?.Invoke();
                Release();
            }, ignoreTimeScale: false);
    }
}