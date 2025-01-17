using System;
using Code.Gameplay;
using Code.UI.Screens;
using VContainer.Unity;

namespace Code.UI.Handlers
{
    public class UIGameEventsHandler : IInitializable, IDisposable
    {
        private readonly IGameEventSender _gameEventSender;
        private readonly StartScreen _startScreen;
        private readonly EndScreen _endScreen;

        public UIGameEventsHandler(IGameEventSender gameEventSender, StartScreen startScreen, EndScreen endScreen)
        {
            _gameEventSender = gameEventSender;
            _startScreen = startScreen;
            _endScreen = endScreen;
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
            _startScreen.Hide();

        private void OnGameEnded() => 
            _endScreen.Show();
    }
}