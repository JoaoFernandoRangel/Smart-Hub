using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarraSuperiorController : MonoBehaviour
{
    [SerializeField]
    TrackingScript trackingScript;
    [SerializeField]

    Animator animator;

    private void Start()
    {
        trackingScript.GarraValueChanged += TrackingScript_GarraValueChanged;
    }

    private void TrackingScript_GarraValueChanged(string arg1, string arg2, string arg3, string arg4)
    {
        //print(trackingScript.AberturaGarra);
        if (trackingScript.AberturaGarra == "GA")
        {
            MainThreadDispatcher.Instance.Enqueue(() => GarraFechada(false));
        }
        if (trackingScript.AberturaGarra == "GF")
        {
            MainThreadDispatcher.Instance.Enqueue(() => GarraFechada(true));

        }
    }

    private void GarraFechada(bool toma)
    {
        animator.SetBool("garraFechada?", toma);
    }
}
