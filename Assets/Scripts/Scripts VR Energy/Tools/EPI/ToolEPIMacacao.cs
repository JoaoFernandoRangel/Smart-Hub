using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolEPIMacacao : Tool, ITorsoSocket
{

    protected override void OnSelectEnteredListener(SelectEnterEventArgs interactor)
    {
        if (interactor.interactorObject is XRSocketInteractor)
        {
            gameObject.GetComponentInChildren<Renderer>().enabled = false;
            NovaAcao(gameObject.name, States.Colocar);
        }
    }
    protected override void OnSelectExitedListener(SelectExitEventArgs interactor)
    {
        if (interactor.interactorObject is XRSocketInteractor)
        {
            gameObject.GetComponentInChildren<Renderer>().enabled = true;
            NovaAcao(gameObject.name, States.Retirar);
        }
    }
}
