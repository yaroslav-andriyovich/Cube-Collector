using System;

namespace Code.Core.Pools.Poolable
{
    public interface IPoolable
    {
        void Initialize(Action<IPoolable> returnAction);
        void Release();
    }
}