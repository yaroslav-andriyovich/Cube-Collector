using Code.Core.Services.Pools;
using Code.Gameplay;
using Code.Gameplay.Environment;
using Code.Gameplay.Services.GameControl;
using Code.Gameplay.Tracks;
using Code.Player;
using Code.UI;
using Code.VFX;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Core.Scopes
{
    public class LevelScope : LifetimeScope
    {
        [SerializeField] private UIView _uiView;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private TrackSpawner _trackSpawner;
        [SerializeField] private WarpEffect _warpEffectPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_playerController);
            builder.RegisterInstance(_trackSpawner).AsImplementedInterfaces().AsSelf();
            
            RegisterServices(builder);
            RegisterUI(builder);

            builder.RegisterComponentInNewPrefab(_warpEffectPrefab, Lifetime.Singleton).AsSelf();

            builder.Register<GameController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameLauncher>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterServices(IContainerBuilder builder)
        {
            builder.Register<CameraShaker>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<PoolService>(Lifetime.Scoped);
        }

        private void RegisterUI(IContainerBuilder builder)
        {
            builder.RegisterInstance(_uiView);
            builder.Register<UIClickMediator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UIWindowMediator>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}