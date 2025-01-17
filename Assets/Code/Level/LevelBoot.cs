using System;
using Code.Core.SceneManagement;
using Code.Gameplay;
using Code.Gameplay.Systems;
using Code.PlayerLogic;
using Code.UI;
using Code.UI.HeadUpDisplay;
using Code.VFX;
using VContainer.Unity;

namespace Code.Level
{
    public class LevelBoot : IInitializable, IPostInitializable, IDisposable
    {
        private readonly ILoadingScreen _loadingScreen;
        private readonly IGameEventSender _gameEventSender;
        private readonly GameInputActions _inputActions;
        private readonly TrackFlow _trackFlow;
        private readonly Player _player;
        private readonly WarpEffect _warpEffect;
        private readonly HUD _hud;

        public LevelBoot(
            ILoadingScreen loadingScreen, 
            IGameEventSender gameEventSender, 
            GameInputActions inputActions,
            TrackFlow trackFlow,
            Player player,
            WarpEffect warpEffect,
            HUD hud
            )
        {
            _loadingScreen = loadingScreen;
            _trackFlow = trackFlow;
            _inputActions = inputActions;
            _gameEventSender = gameEventSender;
            _player = player;
            _warpEffect = warpEffect;
            _hud = hud;
        }

        public void Initialize()
        {
            _gameEventSender.GameStarted += OnGameStarted;
            _gameEventSender.GameEnded += _warpEffect.Disable;
        }

        public void PostInitialize()
        {
            _trackFlow.Initialize();
            _warpEffect.SetTarget(_player.transform);
            _loadingScreen.Hide();
            _inputActions.Enable();
        }

        public void Dispose()
        {
            _gameEventSender.GameStarted -= OnGameStarted;
            _gameEventSender.GameEnded -= _warpEffect.Disable;
            
            _inputActions.Disable();
        }

        private void OnGameStarted()
        {
            _warpEffect.Enable();
            _hud.Show();
        }
    }
}