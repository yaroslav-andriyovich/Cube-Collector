using System;
using Code.Gameplay.Services.GameControl;
using VContainer.Unity;

namespace Code.UI
{
    public class UIClickMediator : IInitializable, IDisposable
    {
        private readonly IGameControl _gameControl;
        private readonly UIView _uiView;

        public UIClickMediator(UIView uiView, IGameControl gameControl)
        {
            _uiView = uiView;
            _gameControl = gameControl;
        }

        public void Initialize() => 
            _uiView.RestartButtonClick += OnRestartButton;

        public void Dispose() => 
            _uiView.RestartButtonClick -= OnRestartButton;

        private void OnRestartButton() => 
            _gameControl.RestartGame();
    }
}