using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSocketMenuItem : XRSocketInteractor
{
    protected override void OnSelectEntered(SelectEnterEventArgs interactable)
    {
        base.OnSelectEntered(interactable);

        interactable.interactableObject.transform.TryGetComponent<MenuRingItem>(out MenuRingItem menuItem);

        if (menuItem)
        {
            menuItem.OnPegar();

        }
    }

    protected override void OnSelectExited(SelectExitEventArgs interactable)
    {
        base.OnSelectExited(interactable);

        interactable.interactableObject.transform.TryGetComponent<MenuRingItem>(out MenuRingItem menuItem);

        if (menuItem)
        {
            menuItem.OnColocar();

        }
    }
}
