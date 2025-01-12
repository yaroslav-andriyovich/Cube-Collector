using Code.Core.Services.Camera;
using Code.Core.Services.Pools;
using Code.Gameplay;
using Code.Gameplay.CameraManagement;
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
        [SerializeField] private CameraProvider _cameraProvider;
        [SerializeField] private UIView _uiView;
        [SerializeField] private PlayerController _playerController;
        [SerializeField] private TrackSpawner _trackSpawner;
        [SerializeField] private WarpEffect _warpEffectPrefab;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<GameController>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameLauncher>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.RegisterComponent(_cameraProvider);
            builder.Register<CameraShakeService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.RegisterComponent(_playerController);

            builder.RegisterComponent(_trackSpawner).AsImplementedInterfaces().AsSelf();
            
            builder.Register<PoolService>(Lifetime.Scoped);
            RegisterUI(builder);

            builder.RegisterComponentInNewPrefab(_warpEffectPrefab, Lifetime.Singleton).AsSelf();
        }
        
        private void RegisterUI(IContainerBuilder builder)
        {
            builder.RegisterComponent(_uiView);
            builder.Register<UIClickMediator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UIWindowMediator>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}