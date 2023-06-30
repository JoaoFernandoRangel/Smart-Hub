using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VREnergy.PRO;

public class GazePanel : MonoBehaviour
{
    public bool isGazingUpon =false;



    public void GazingUpon()
    {
        isGazingUpon = true;

        FindObjectOfType<ProcedureStageHandler>().NewAction(new PROAction {
            Activator = "Operador",
            Receptor = GetComponent<PROAsset>().UnityId,
            Interaction = States.Olhar.ToString()
        });
    }

    public void NotGazingUpon()
    {
        isGazingUpon = false;
    }
}

