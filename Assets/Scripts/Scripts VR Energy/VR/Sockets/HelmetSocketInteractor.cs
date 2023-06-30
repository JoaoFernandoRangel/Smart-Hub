using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class HelmetSocketInteractor : XRSocketInteractor
{
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && IsHelmet((XRBaseInteractable)interactable);
    }
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && IsHelmet((XRBaseInteractable)interactable);
    }    

    private bool IsHelmet(XRBaseInteractable interactable)
    {
        return (interactable.gameObject.GetComponent<Tool>() is IHeadSocket);
    }
}
