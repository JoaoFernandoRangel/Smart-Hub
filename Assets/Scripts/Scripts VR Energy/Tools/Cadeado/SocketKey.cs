using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class SocketKey : XRSocketInteractor
{
    public LockType tipoSocket;
    
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && IsCompatibleKey((XRBaseInteractable)interactable) && CanConnect((XRBaseInteractable)interactable);
    }
    
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && IsCompatibleKey((XRBaseInteractable)interactable) && CanConnect((XRBaseInteractable)interactable);
    }
    
    private bool IsCompatibleKey(XRBaseInteractable interactable)
    {
        var intera = interactable.gameObject.GetComponent<IKey>();
        return (intera != null) 
            && tipoSocket == intera.GetLockType();
    }
    
    private bool CanConnect(XRBaseInteractable interactable)
    {
        var intera = interactable.gameObject.GetComponent<IKey>();
        return intera.CanConnect();
    }
    
    protected override void OnSelectEntered(SelectEnterEventArgs interactable)
    {
        base.OnSelectEntered(interactable);
        if (interactable == null) { return; }
        interactable.interactableObject.transform.gameObject.GetComponent<IKey>().Connect(this);
    }
    
    protected override void OnSelectExited(SelectExitEventArgs interactable)
    {
        base.OnSelectExited(interactable);
        if (interactable == null) { return; }
        interactable.interactableObject.transform.gameObject.GetComponent<IKey>().Disconnect();
    }
}