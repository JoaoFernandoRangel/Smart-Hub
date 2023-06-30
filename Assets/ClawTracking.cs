using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClawTracking : MonoBehaviour
{
    [SerializeField]
    TrackingScript trackingScript;
    [SerializeField]
    Transform rightHandPos;
    [SerializeField]
    Animator anim;
    private void Update()
    {/*
        if (trackingScript.isTracking)
        {
            this.gameObject.transform.position = rightHandPos.position;
            anim.enabled = false;
        }
        else
        {
            anim.enabled = true ;

        }*/
    }
}
