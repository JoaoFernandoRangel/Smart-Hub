using UnityEngine.XR.Interaction.Toolkit;

namespace VREnergy.VR
{
    public class XRMesaMovimentacaoSocket : XRSocketInteractor
    {
        public override bool CanHover(IXRHoverInteractable interactable)
        {
            return base.CanHover(interactable) && IsMesaMovimentacaoInteractable((XRBaseInteractable)interactable);
        }

        public override bool CanSelect(IXRSelectInteractable interactable)
        {
            return base.CanSelect(interactable) && IsMesaMovimentacaoInteractable((XRBaseInteractable)interactable);
        }

        private bool IsMesaMovimentacaoInteractable(XRBaseInteractable interactable)
        {
            return interactable is MesaMovimentacaoInteractable;
        }
    }
}