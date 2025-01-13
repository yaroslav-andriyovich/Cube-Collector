using Code.Core.DI.Base;
using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.UI
{
    public class UIInstaller : MonoInstaller
    {
        [SerializeField] private UIView _uiView;
        
        public override void Install(IContainerBuilder builder)
        {
            builder.RegisterComponent(_uiView);
            builder.Register<UIClickMediator>(Lifetime.Singleton).AsImplementedInterfaces();
            builder.Register<UIWindowMediator>(Lifetime.Singleton).AsImplementedInterfaces();
        }
    }
}