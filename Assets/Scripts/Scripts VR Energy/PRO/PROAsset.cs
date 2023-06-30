using System;
using UnityEngine;

namespace VREnergy.PRO
{
    public class PROAsset : MonoBehaviour, IPROAsset
    {
        [SerializeField]
        private string unityId;

        public string UnityId
        {
            get => unityId;
            set => unityId = value;
        }
        
        [SerializeField]
        private bool isAssetActive;
        
        public bool IsAssetActive
        {
            get => isAssetActive;
        }

        [SerializeField]
        private XRTintBlinkVisual xrTintBlinkVisual;
        
        public event Action<PROAction> OnAssetInteraction;

        #region MONOBEHAVIOUR

        private void Awake()
        {
            if (xrTintBlinkVisual != null)
            {
                return;
            }
            
            if (!TryGetComponent(out xrTintBlinkVisual))
            {
                Debug.LogError($"{nameof(XRTintBlinkVisual)} não encontrado", this);
            }
        }

        #endregion

        public void AssetInteraction(PROAction proAction)
        {
            OnAssetInteraction?.Invoke(proAction);
        }

        public void EnableAsset()
        {
            isAssetActive = true;
            SetAssetMaterialRender(isAssetActive);
        }

        public void DisableAsset()
        {
            isAssetActive = false;
            SetAssetMaterialRender(isAssetActive);
        }

        private void SetAssetMaterialRender(bool value)
        {
            if (xrTintBlinkVisual == null) return;
            
            xrTintBlinkVisual.ToggleTint(value);
        }
    }
}
