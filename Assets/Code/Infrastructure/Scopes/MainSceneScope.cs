using Code.Gameplay.Environment;
using Code.Infrastructure.Services.Pools;
using Code.StaticData;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Infrastructure.Scopes
{
    public class MainSceneScope : LifetimeScope
    {
        [SerializeField] private CameraConfig _cameraConfig;
        
        protected override void Configure(IContainerBuilder builder)
        {
            builder.RegisterInstance(_cameraConfig);
            builder.Register<CameraShaker>(Lifetime.Singleton);
            builder.Register<PoolService>(Lifetime.Singleton);
        }
    }
}