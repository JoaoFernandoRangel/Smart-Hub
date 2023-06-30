using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VREnergy.VR.Hand;

public class LeftHandSocketInteractor : XRSocketInteractor
{
    [SerializeField] private HandPresence leftHandPresence;
    [SerializeField] private float scaleFactor = 0.2f;
    
    private XRBaseInteractable interactableHand;
    private Vector3 originalInteractableScale;

    protected override void OnSelectEntered(SelectEnterEventArgs interactable)
    {
        base.OnSelectEntered(interactable);
        StoreObjectScale(interactable);
        IHandSocket handSocket = interactable.interactableObject.transform.gameObject.GetComponent<IHandSocket>();
        leftHandPresence.ChangeHandModel(handSocket.HandModel);
        SetToTargetScale(interactable);
    }

    protected override void OnSelectExited(SelectExitEventArgs interactable)
    {
        base.OnSelectExited(interactable);
        leftHandPresence.RemoveHandModel();
        SetObjectToOriginalScale(interactable);
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && IsLeftHand((XRBaseInteractable)interactable);
    }
    
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && IsLeftHand((XRBaseInteractable)interactable);
    }
    
    private bool IsLeftHand(XRBaseInteractable interactable)
    {
        return (interactable.gameObject.GetComponent<ILeftHandSocket>() != null);
    }
    
    private void StoreObjectScale(SelectEnterEventArgs interactable)
    {
        originalInteractableScale = interactable.interactableObject.transform.localScale;
    }

    private void SetObjectToOriginalScale(SelectExitEventArgs interactable)
    {
        interactable.interactableObject.transform.localScale = originalInteractableScale;
    }

    private void SetToTargetScale(SelectEnterEventArgs interactable)
    {
        interactable.interactableObject.transform.localScale = originalInteractableScale * scaleFactor;
    }
}
