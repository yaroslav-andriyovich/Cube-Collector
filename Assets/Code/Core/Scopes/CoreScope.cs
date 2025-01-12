using Code.Core.AssetManagement;
using Code.Core.SceneManagement;
using Code.Core.Services.Input;
using Code.Core.Services.StaticData;
using Code.Core.Services.Vibration;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Core.Scopes
{
    public class CoreScope : LifetimeScope
    {
        [SerializeReference] private LoadingScreen _loadingScreenPrefab;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterEntryPoint<GameBoot>();
            
            builder.Register<AssetProvider>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<ConfigService>(Lifetime.Singleton);
            builder.RegisterComponentInNewPrefab(_loadingScreenPrefab, Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<SceneLoader>(Lifetime.Singleton);
            builder.Register<InputService>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<VibrationService>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}