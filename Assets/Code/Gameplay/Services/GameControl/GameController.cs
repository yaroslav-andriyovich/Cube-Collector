using System;
using Code.Core.Services.Input;
using Code.Core.Services.Loading;
using VContainer.Unity;

namespace Code.Gameplay.Services.GameControl
{
    public class GameController : IGameControl, IStartable
    {
        public event Action GameStarted;
        public event Action GameEnded;
        
        private readonly LevelLoader _levelLoader;
        private readonly IInputService _inputService;

        private bool _started;
        private bool _ended;

        public GameController(LevelLoader levelLoader, IInputService inputService)
        {
            _inputService = inputService;
            _levelLoader = levelLoader;
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
            _levelLoader.ReloadCurrent();
        }
    }
}