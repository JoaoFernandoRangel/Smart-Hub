using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VREnergy.PRO;

public class ActivatePROAction : MonoBehaviour
{
    [SerializeField]
    string activator;
    [SerializeField]
    string receptor;
    [SerializeField]
    States states;
    [SerializeField]
    bool receptorIsObject = true;

    public void Ativar()
    {

        if (receptorIsObject)
        {
            FindObjectOfType<ProcedureStageHandler>().NewAction(new PROAction
            {
                Activator = activator,
                Receptor = GetComponent<PROAsset>().UnityId,
                Interaction = states.ToString()
            });
        }
        else
        {
            FindObjectOfType<ProcedureStageHandler>().NewAction(new PROAction
            {
                Activator = activator,
                Receptor = receptor,
                Interaction = states.ToString()
            });
        }


    }
}
