using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolEPICapaceteMac : Tool, IHeadSocket
{
    public VolumeProfile VisaoCapacete;

    protected override void OnSelectEnteredListener(SelectEnterEventArgs interactor)
    {
        if (interactor.interactorObject is XRSocketInteractor)
        {
            //REFACTOR POINT
            var volumes = FindObjectsOfType<Volume>();
            if(volumes.Length > 0)
                volumes[0].profile = VisaoCapacete;

            gameObject.GetComponentInChildren<Renderer>().enabled = false;
            //NovaAcao(gameObject.name, States.Colocar);
        }
    }
    protected override void OnSelectExitedListener(SelectExitEventArgs interactor)
    {
        if (interactor.interactorObject is XRSocketInteractor)
        {
            var volumes = FindObjectsOfType<Volume>();
            if (volumes.Length > 0)
            {
                volumes[0].profile = null;
            }
            gameObject.GetComponentInChildren<Renderer>().enabled = true;
            //NovaAcao(gameObject.name, States.Retirar);
        }
    }
}
