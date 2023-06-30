using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GrabThings : MonoBehaviour
{
    [SerializeField]
    UnityEvent grab;
    [SerializeField]
    UnityEvent drop;

    [SerializeField]
    UnityEvent dropCap;


    [SerializeField]
    Transform claw;
    [SerializeField]
    Transform redCap1;
    [SerializeField]
    Transform blueCap1;
    [SerializeField]
    Transform Cap;

    bool grabRed = false;
    bool grabBlue = false;
    bool grabCap = false;
    public void Grab()
    {
        grab.Invoke();
    }
    public void Drop()
    {
        drop.Invoke();
    }
    public void DropCapEvent()
    {
        dropCap.Invoke();
    }


    public void GrabRedCap()
    {
        grabRed = true;
    }

    public void GrabBlueCap()
    {
        grabBlue = true;

    }
    public void GrabCap()
    {
        grabCap = true;

    }
    public void DropCap()
    {
        grabRed = false;
        grabBlue = false;
        grabCap = false;

    }
    private void Update()
    {
        if (grabRed)
        {
            redCap1.position = claw.position;
        }
        if(grabBlue)
        {
            blueCap1.position = claw.position;
        }
        if (grabCap)
        {
            Cap.position = claw.position;
        }
    }
}
