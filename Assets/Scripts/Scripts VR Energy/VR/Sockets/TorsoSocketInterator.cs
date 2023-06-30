using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class TorsoSocketInterator : XRSocketInteractor
{
    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && IsTorso((XRBaseInteractable)interactable);
    }
    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && IsTorso((XRBaseInteractable)interactable);
    }
    private bool IsTorso(XRBaseInteractable interactable)
    {
        return (interactable.gameObject.GetComponent<Tool>() is ITorsoSocket);
    }
}
