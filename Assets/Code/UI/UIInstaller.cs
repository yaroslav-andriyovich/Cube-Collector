using Code.Core.DI.Base;
using Code.UI.Handlers;
using Code.UI.HeadUpDisplay;
using Code.UI.Screens;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.UI
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private HUD _hud;
        [SerializeField] private StartScreen _startScreen;
        [SerializeField] private PauseScreen _pauseScreen;
        [SerializeField] private EndScreen _endScreen;

        public override void Install(IContainerBuilder builder)
        {
            builder.Register<UIGameEventsHandler>(Lifetime.Singleton).AsImplementedInterfaces();

            builder.RegisterComponent(_hud);
            builder.RegisterComponent(_startScreen);
            builder.RegisterComponent(_pauseScreen);
            builder.RegisterComponent(_endScreen);
            
            builder.Register<UIPauseHandler>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}