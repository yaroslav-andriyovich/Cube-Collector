using System;
using Code.Core.SceneManagement;
using Code.Gameplay.Systems;
using VContainer.Unity;

namespace Code.Level
{
    public class LevelBoot : IInitializable, IDisposable
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly TrackCubesSpawner _trackCubesSpawner;
        private readonly TrackSpawner _trackSpawner;
        private readonly GameInputActions _inputActions;

        public LevelBoot(
            ILoadingScreen loadingScreen, 
            TrackCubesSpawner trackCubesSpawner, 
            TrackSpawner trackSpawner, 
            GameInputActions inputActions
            )
        {
            _loadingScreen = loadingScreen;
            _trackCubesSpawner = trackCubesSpawner;
            _trackSpawner = trackSpawner;
            _inputActions = inputActions;
        }

        public void Initialize()
        {
            _trackCubesSpawner.Initialize();
            _trackSpawner.Initialize();
            _loadingScreen.Hide();
            _inputActions.Enable();
        }

        public void Dispose() => 
            _inputActions.Disable();
    }
}