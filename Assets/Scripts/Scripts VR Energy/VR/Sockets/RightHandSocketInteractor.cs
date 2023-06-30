using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VREnergy.VR.Hand;

public class RightHandSocketInteractor : XRSocketInteractor
{
    [SerializeField] private HandPresence rightHandPresence;
    [SerializeField] private float scaleFactor = 0.2f;
    
    private XRBaseInteractable interactableHand;
    private Vector3 originalInteractableScale;

    protected override void OnSelectEntered(SelectEnterEventArgs interactable)
    {
        base.OnSelectEntered(interactable);
        StoreObjectScale(interactable);
        IHandSocket handSocket = interactable.interactableObject.transform.gameObject.GetComponent<IHandSocket>();
        rightHandPresence.ChangeHandModel(handSocket.HandModel);
        SetToTargetScale(interactable);
    }

    protected override void OnSelectExited(SelectExitEventArgs interactable)
    {
        base.OnSelectExited(interactable);
        rightHandPresence.RemoveHandModel();
        SetObjectToOriginalScale(interactable);
    }
    
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && IsRightHand((XRBaseInteractable)interactable);
    }
    
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && IsRightHand((XRBaseInteractable)interactable);
    }
    
    private bool IsRightHand(XRBaseInteractable interactable)
    {
        return (interactable.gameObject.GetComponent<IRightHandSocket>() != null);
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
