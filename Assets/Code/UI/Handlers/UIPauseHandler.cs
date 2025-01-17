using System;
using Code.Core.Services.Pause;
using Code.UI.HeadUpDisplay;
using Code.UI.Screens;
using VContainer.Unity;

namespace Code.UI.Handlers
{
    public class UIPauseHandler : IInitializable, IDisposable
    {
        private readonly IPauseService _pauseService;
        private readonly HUD _hud;
        private readonly PauseScreen _pauseScreen;

        public UIPauseHandler(IPauseService pauseService, HUD hud, PauseScreen pauseScreen)
        {
            _pauseService = pauseService;
            _hud = hud;
            _pauseScreen = pauseScreen;
        }

        public void Initialize()
        {
            _pauseService.Paused += OnGamePaused;
            _pauseService.UnPaused += OnGameUnPaused;
        }

        public void Dispose()
        {
            _pauseService.Paused -= OnGamePaused;
            _pauseService.UnPaused -= OnGameUnPaused;
        }

        private void OnGamePaused()
        {
            _hud.Hide();
            _pauseScreen.Show();
        }

        private void OnGameUnPaused()
        {
            _hud.Show();
            _pauseScreen.Hide();
        }
    }
}