using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRActivateOnSelectWithTriggerInteractable : XRSimpleInteractable
{

    protected override void OnHoverEntered(HoverEnterEventArgs interactor)
    {        
        interactor.interactorObject.transform.GetComponent<XRController>().selectUsage = InputHelpers.Button.Trigger;
        interactor.interactorObject.transform.GetComponent<XRController>().activateUsage = InputHelpers.Button.Grip;
        base.OnHoverEntered(interactor);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {     
        base.OnSelectEntered(interactor);
    }

    protected override void OnHoverExited(HoverExitEventArgs interactor)
    {
        interactor.interactorObject.transform.GetComponent<XRController>().selectUsage = InputHelpers.Button.Grip;
        interactor.interactorObject.transform.GetComponent<XRController>().activateUsage = InputHelpers.Button.Trigger;        
        base.OnHoverExited(interactor);
    }
    
}
