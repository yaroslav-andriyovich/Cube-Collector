using System;
using Code.Core.Services.Input;
using Code.Gameplay.Services.GameControl;
using VContainer.Unity;

namespace Code.Gameplay
{
    public class GameLauncher : IInitializable, IDisposable
    {
        private readonly IGameControl _gameControl;
        private readonly IInputService _inputService;

        public GameLauncher(IGameControl gameControl, IInputService inputService)
        {
            _gameControl = gameControl;
            _inputService = inputService;
        }
        
        public void Initialize() => 
            _inputService.Touch += OnPlayerStartGame;

        public void Dispose() => 
            _inputService.Touch -= OnPlayerStartGame;
        
        private void OnPlayerStartGame()
        {
            _inputService.Touch -= OnPlayerStartGame;

            _gameControl.StartGame();
        }
    }
}