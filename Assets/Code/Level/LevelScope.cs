using Code.Core.CameraManagement;
using Code.Core.Services.Camera;
using Code.Core.Services.Pools;
using Code.Gameplay;
using Code.Gameplay.Systems;
using Code.PlayerLogic;
using Code.UI;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Level
{
    public class LevelScope : LifetimeScope
    {
        [SerializeField] private CameraProvider _cameraProvider;
        [SerializeField] private Player _player;
        [SerializeField] private UIInstaller _uiInstaller;

        protected override void Configure(IContainerBuilder builder)
        {
            builder.Register<LevelBoot>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameController>(Lifetime.Singleton).AsImplementedInterfaces();
            
            builder.RegisterComponent(_cameraProvider);
            builder.Register<CameraShakeService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            
            builder.Register<PoolService>(Lifetime.Scoped);

            builder.RegisterComponent(_player);

            builder.Register<TrackSpawner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<TrackCubesSpawner>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<LostCubeHandler>(Lifetime.Singleton).AsImplementedInterfaces();

            _uiInstaller.Install(builder);
        }
    }
}