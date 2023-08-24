using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistonForce : MonoBehaviour
{
    public float force = 1;
    [SerializeField]
    GameObject copo;
    [SerializeField]

    bool turnOn = false;
    private void Start()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {

        if (other.gameObject.CompareTag("Copo"))
        {
            copo = other.gameObject;

            if (turnOn)
            {
                // Assuming the other object has a Rigidbody component
                Rigidbody otherObjectRb = copo.GetComponent<Rigidbody>();

                // Apply the piston force on the other object
                print("Apply the piston force on the other object : " + gameObject.name);
                otherObjectRb.AddForce(transform.forward * force, ForceMode.Impulse);
                turnOn = false;
            }
        }


    }
    public void TurnOn()
    {

        turnOn = true;
    }
}
