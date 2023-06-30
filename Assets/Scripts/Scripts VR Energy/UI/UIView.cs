using UnityEngine;

namespace VREnergy.UI
{
    /// <summary>
    /// Representação de uma página na UI. Essa classe pode ser herdada para criar comportamentos diferentes para cada página.
    /// </summary>
    public class UIView : MonoBehaviour
    {
        protected UIViewManager _uiViewManager;
    
        public virtual void Initialize(UIViewManager context)
        {
            _uiViewManager = context;
        }
    
        public virtual void Show()
        {
            gameObject.SetActive(true);
        }

        public virtual void Hide()
        {
            gameObject.SetActive(false);
        }
    }
}
