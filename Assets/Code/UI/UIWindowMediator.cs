using System;
using Code.Gameplay;
using VContainer.Unity;

namespace Code.UI
{
    public class UIWindowMediator : IInitializable, IDisposable
    {
        private readonly UIView _uiView;
        private readonly IGameEventSender _gameEventSender;

        public UIWindowMediator(UIView uiView, IGameEventSender gameEventSender)
        {
            _uiView = uiView;
            _gameEventSender = gameEventSender;
        }

        public void Initialize()
        {
            _gameEventSender.GameStarted += OnGameStarted;
            _gameEventSender.GameEnded += OnGameEnded;
        }

        public void Dispose()
        {
            _gameEventSender.GameStarted -= OnGameStarted;
            _gameEventSender.GameEnded -= OnGameEnded;
        }

        private void OnGameStarted() => 
            _uiView.HideStartWindow();

        private void OnGameEnded() => 
            _uiView.ShowRestartWindow();
    }
}