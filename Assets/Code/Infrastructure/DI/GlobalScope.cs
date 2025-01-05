using Code.Infrastructure.Services.InputService;
using DG.Tweening;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Infrastructure.DI
{
    public class GlobalScope : LifetimeScope
    {
        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
            DOTween.Init();
            Vibration.Init();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}