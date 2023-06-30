using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPainelSocket : XRSocketInteractor
{
    public LockType tipoSocket;

    /// <summary>
    /// Diz se o socket está trancado por uma fechadura.
    /// </summary>
    public bool Locked { get; set; }
    
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        if (Locked) { return false; }

        Debug.Log($"Entrou {IsCompatibleKey((XRBaseInteractable)interactable)} {CanConnect((XRBaseInteractable)interactable)}");
        return base.CanHover(interactable) && IsCompatibleKey((XRBaseInteractable)interactable) && CanConnect((XRBaseInteractable)interactable);
    }
    
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        if (Locked) { return false; }
        
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
        if (interactable == null) { return false; }
        
        if (interactable.TryGetComponent(out IKey intera))
        {
            return intera.CanConnect();
        }

        return false;
    }
    
    protected override void OnSelectEntered(SelectEnterEventArgs interactable)
    {
        base.OnSelectEntered(interactable);
        if (interactable == null) { return; }
        if (interactable.interactableObject.transform.TryGetComponent(out IKey key))
        {
            key.Connect(this);
        }
    }
    
    protected override void OnSelectExited(SelectExitEventArgs interactable)
    {
        base.OnSelectExited(interactable);
        if (interactable == null) { return; }
        if (interactable.interactableObject.transform.TryGetComponent(out IKey key))
        {
            key.Disconnect();
        }
    }
}