using Cinemachine;
using Code.Core.Services.Camera;
using Code.Gameplay;
using Code.Gameplay.Systems;
using Code.PlayerLogic;
using Code.UI;
using Code.VFX;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Level
{
    public class LevelScope : LifetimeScope
    {
        [SerializeField] private CinemachineVirtualCamera _camera;
        [SerializeField] private Player _player;
        [SerializeField] private WarpEffect _warpEffect;
        [SerializeField] private UIInstaller _uiInstaller;

        protected override void Configure(IContainerBuilder builder)
        {
            RegisterCore(builder);
            RegisterCamera(builder);
            builder.RegisterComponentInNewPrefab(_warpEffect, Lifetime.Singleton);
            builder.RegisterComponent(_player);
            RegisterSystems(builder);
            
            _uiInstaller.Install(builder);
        }

        private void RegisterCore(IContainerBuilder builder)
        {
            builder.Register<LevelBoot>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<GameController>(Lifetime.Singleton).AsImplementedInterfaces();
        }

        private void RegisterCamera(IContainerBuilder builder)
        {
            builder.RegisterComponent(_camera);
            builder.Register<CameraShakeService>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
        }

        private void RegisterSystems(IContainerBuilder builder)
        {
            builder.Register<TrackFlow>(Lifetime.Singleton).AsImplementedInterfaces().AsSelf();
            builder.Register<CubeCollectionHandler>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<LostCubeHandler>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}