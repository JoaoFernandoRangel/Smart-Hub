using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectFollowScript : MonoBehaviour
{
    [SerializeField]
    Transform objectToFollow;
    [SerializeField]
    Transform transformObjectPlace;
    [SerializeField]
    GameObject objectInGrab;
    [SerializeField]
    bool isGrabbing = true;
    [SerializeField]
    Vector3 offset ;
    Rigidbody rbCopo;
    [SerializeField]

    Animator anCopo;
    private void Update()
    {
        this.transform.position = objectToFollow.transform.position + offset;

        if (isGrabbing)
        {
            objectInGrab.transform.position = transformObjectPlace.position;
        }
       
    }






    public void FireTriggerInCopo()
    {
        anCopo.SetTrigger("add");
    }
    public void IsGrabbingTrue()
    {
        isGrabbing = true;
        rbCopo.isKinematic = true;
    }   
    public void IsGrabbingFalse()
    {
        
        rbCopo.isKinematic = false;
        isGrabbing = false;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Copo"))
        {
            other.TryGetComponent<Rigidbody>(out rbCopo);
            other.TryGetComponent<Animator>(out anCopo);

            objectInGrab = other.gameObject;
        }
       
    }
    private void OnTriggerExit(Collider other)
    {
        objectInGrab = null;
        if (other.CompareTag("Copo"))
        {
            rbCopo = null;
        }
        
    }

}
