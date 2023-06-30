using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RingMenuItem : MonoBehaviour
{
    [SerializeField]
    public bool onMenuRing=false;
    [SerializeField]
    Transform parent;

    Rigidbody rb;
    Collider col;
    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        col = GetComponent<Collider>();
    }
    private void Update()
    {
        if (!onMenuRing)
        {
            if (rb)
            {
                rb.useGravity = true;
            }
            
            transform.SetParent(parent);
            col.enabled = true;
        }
        else
        {
            col.enabled = false;
        }
    }
}
