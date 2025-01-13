using System;
using Code.Core.SceneManagement;

namespace Code.Gameplay
{
    public class GameController : IGameControl
    {
        public event Action GameStarted;
        public event Action GameEnded;
        
        private readonly SceneLoader _sceneLoader;
        private readonly ILoadingScreen _loadingScreen;

        private bool _started;
        private bool _ended;

        public GameController(SceneLoader sceneLoader, ILoadingScreen loadingScreen)
        {
            _sceneLoader = sceneLoader;
            _loadingScreen = loadingScreen;
        }

        public void StartGame()
        {
            if (_started)
                return;

            _started = true;

            GameStarted?.Invoke();
        }

        public void EndGame()
        {
            if (!_started || _ended)
                return;

            _ended = true;
            
            GameEnded?.Invoke();
        }

        public void RestartGame()
        {
            _loadingScreen.Show();
            _sceneLoader.ReloadCurrent();
        }
    }
}