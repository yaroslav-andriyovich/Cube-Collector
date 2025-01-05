using Code.Infrastructure.Services.Input;
using Code.Infrastructure.Services.Vibration;
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
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<VibrationService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}