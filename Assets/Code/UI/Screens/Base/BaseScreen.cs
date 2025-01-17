using UnityEngine;

namespace Code.UI.Screens.Base
{
    public abstract class BaseScreen : MonoBehaviour
    {
        public virtual void Show() => 
            gameObject.SetActive(true);
        
        public virtual void Hide() => 
            gameObject.SetActive(false);
    }
}