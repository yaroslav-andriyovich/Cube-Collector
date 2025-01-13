using System;
using Code.Core.SceneManagement;
using Code.Gameplay.Tracks;
using VContainer.Unity;

namespace Code.Level
{
    public class LevelBoot : IInitializable, IDisposable
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly TrackSpawner _trackSpawner;
        private readonly GameInputActions _inputActions;

        public LevelBoot(ILoadingScreen loadingScreen, TrackSpawner trackSpawner, GameInputActions inputActions)
        {
            _trackSpawner = trackSpawner;
            _loadingScreen = loadingScreen;
            _inputActions = inputActions;
        }

        public void Initialize()
        {
            _trackSpawner.Initialize();
            _loadingScreen.Hide();
            _inputActions.Enable();
        }

        public void Dispose() => 
            _inputActions.Disable();
    }
}