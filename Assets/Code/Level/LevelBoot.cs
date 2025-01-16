using System;
using Code.Core.SceneManagement;
using Code.Gameplay.Systems;
using VContainer.Unity;

namespace Code.Level
{
    public class LevelBoot : IPostInitializable, IDisposable
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly TrackSpawner _trackSpawner;
        private readonly GameInputActions _inputActions;

        public LevelBoot(
            ILoadingScreen loadingScreen, 
            TrackSpawner trackSpawner, 
            GameInputActions inputActions
            )
        {
            _loadingScreen = loadingScreen;
            _trackSpawner = trackSpawner;
            _inputActions = inputActions;
        }

        public void PostInitialize()
        {
            _trackSpawner.SpawnInitialTracks();
            _loadingScreen.Hide();
            _inputActions.Enable();
        }

        public void Dispose() => 
            _inputActions.Disable();
    }
}