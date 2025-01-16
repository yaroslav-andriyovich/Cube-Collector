using System;
using Code.Core.SceneManagement;
using Code.Gameplay.Systems;
using VContainer.Unity;

namespace Code.Level
{
    public class LevelBoot : IPostInitializable, IDisposable
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly TrackFlow _trackFlow;
        private readonly GameInputActions _inputActions;

        public LevelBoot(
            ILoadingScreen loadingScreen, 
            TrackFlow trackFlow, 
            GameInputActions inputActions
            )
        {
            _loadingScreen = loadingScreen;
            _trackFlow = trackFlow;
            _inputActions = inputActions;
        }

        public void PostInitialize()
        {
            _trackFlow.Initialize();
            _loadingScreen.Hide();
            _inputActions.Enable();
        }

        public void Dispose() => 
            _inputActions.Disable();
    }
}