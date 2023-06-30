using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoltarObjeto : MonoBehaviour
{


    private Rigidbody rb;
    private Vector3 targetPosition;
    private Quaternion targetRotation;
    private Vector3 targetScale;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        StoreObjectTransform();
    }


    public void StoreObjectTransform()
    {
        targetPosition = transform.position;
        targetRotation = transform.rotation;
        targetScale = transform.localScale;
    }

    public void SetObjectTransform()
    {
        rb.velocity = Vector3.zero;
        rb.angularVelocity = Vector3.zero;
        transform.position = targetPosition;
        transform.rotation = targetRotation;
        transform.localScale = targetScale;
    }

}
