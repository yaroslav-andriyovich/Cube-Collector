using Code.Core.SceneManagement;
using Code.Core.Services.StaticData;
using Code.Core.Services.Vibration;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Core.DI
{
    public class CoreScope : LifetimeScope
    {
        [SerializeReference] private LoadingScreen _loadingScreenPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<Bootstrap>();
            
            builder.Register<ConfigService>(Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_loadingScreenPrefab, Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<GameInputActions>(Lifetime.Singleton);
            builder.Register<VibrationService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}