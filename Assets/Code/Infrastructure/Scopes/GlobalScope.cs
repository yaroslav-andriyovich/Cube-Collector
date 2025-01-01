using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Infrastructure.Scopes
{
    public class GlobalScope : LifetimeScope
    {
        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
        }

        protected override void Configure(IContainerBuilder builder)
        {
            Debug.Log("Global scope is working!");
        }
    }
}