using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRGrabInteractableMenuItem : XRGrabInteractable
{

    MenuRingItem menuRingItem;
    private void Start()
    {
        menuRingItem = GetComponent<MenuRingItem>();
    }
    protected override void OnSelectEntered(SelectEnterEventArgs interactor)
    {
        base.OnSelectEntered(interactor);

        if (IsHandInteractor((XRBaseInteractor)interactor.interactorObject))
        {
            menuRingItem.OnPegar();
        }
    }


    private bool IsHandInteractor(XRBaseInteractor interactor)
    {
        return interactor is XRDirectInteractor;
    }
}
