using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestObject : MonoBehaviour
{
    private bool isGazingUpon;

    private void Update()
    {
        if (isGazingUpon)
        {
            // Do anything you want here, we'll rotate for this demo
            transform.Rotate(0, 3, 0);
        }
    }

    public void GazingUpon()
    {
        isGazingUpon = true;
    }

    public void NotGazingUpon()
    {
        isGazingUpon = false;
    }
}

