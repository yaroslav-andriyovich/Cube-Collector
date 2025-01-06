using System;

namespace Code.Infrastructure.Pools.Poolable
{
    public interface IPoolable
    {
        void Release();
    }
    
    public interface IPoolable<T> : IPoolable
    {
        void Initialize(Action<T> returnAction);
    }
}