using UnityEngine;
using VContainer;
using VContainer.Unity;

namespace Code.Core.DI.Base
{
    public abstract class MonoInstaller : MonoBehaviour, IInstaller
    {
        public abstract void Install(IContainerBuilder builder);
    }
}