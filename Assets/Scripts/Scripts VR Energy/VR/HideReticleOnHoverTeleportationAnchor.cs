using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VREnergy.VR
{
    public class HideReticleOnHoverTeleportationAnchor : MonoBehaviour
    {
        [SerializeField] private GameObject reticleRender;
    
        private XRRayInteractor _xrRayInteractor;

        private void Awake()
        {
            _xrRayInteractor = GetComponent<XRRayInteractor>();
        }

        private void OnEnable()
        {
            _xrRayInteractor.hoverEntered.AddListener(OnHoverEntered);
            _xrRayInteractor.hoverExited.AddListener(OnHoverExited);
        }

        private void OnDisable()
        {
            _xrRayInteractor.hoverEntered.RemoveListener(OnHoverEntered);
            _xrRayInteractor.hoverExited.RemoveListener(OnHoverExited);
        }

        private void OnHoverEntered(HoverEnterEventArgs interactable)
        {
            if (interactable.interactableObject is TeleportationAnchor)
            {
                reticleRender.SetActive(false);
            }
        }
    
        private void OnHoverExited(HoverExitEventArgs interactable)
        {
            if (interactable.interactableObject is TeleportationAnchor)
            {
                reticleRender.SetActive(true);
            }
        }
    }

}