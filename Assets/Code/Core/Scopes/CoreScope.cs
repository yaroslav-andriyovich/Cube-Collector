using Code.Core.Services.Input;
using Code.Core.Services.Loading;
using Code.Core.Services.Vibration;
using DG.Tweening;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Core.Scopes
{
    public class CoreScope : LifetimeScope
    {
        protected override void Awake()
        {
            base.Awake();
            Application.targetFrameRate = 60;
            DOTween.Init();
        }

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LevelLoader>(Lifetime.Singleton);
            builder.Register<VibrationService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}