using System;
using Code.Core.SceneManagement;
using Code.Core.Services.Input;
using VContainer.Unity;

namespace Code.Gameplay.Services.GameControl
{
    public class GameController : IGameControl, IStartable
    {
        public event Action GameStarted;
        public event Action GameEnded;
        
        private readonly SceneLoader _sceneLoader;
        private readonly IInputService _inputService;

        private bool _started;
        private bool _ended;

        public GameController(SceneLoader sceneLoader, IInputService inputService)
        {
            _inputService = inputService;
            _sceneLoader = sceneLoader;
        }

        public void Start() => 
            _inputService.EnableInput();

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
            
            _inputService.DisableInput();
            GameEnded?.Invoke();
        }

        public void RestartGame()
        {
            _inputService.DisableInput();
            _sceneLoader.ReloadCurrent();
        }
    }
}