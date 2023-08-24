using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EsteiraScript : MonoBehaviour
{
    [SerializeField]
    float velocidade = 10;
    [SerializeField]
    Transform direction;

    [SerializeField]
    SliderEsteiraUI sliderEsteiraUI;
    [SerializeField]

    Rigidbody rb;
    bool empurrando = false;
    public float Velocidade { get => velocidade; set => velocidade = value; }
    private Vector3 directionz;

    private void OnTriggerStay(Collider other)
    {
        if (empurrando)
        {

            if (other.gameObject.CompareTag("Copo"))
            {
                other.TryGetComponent<Rigidbody>(out rb);
                rb.MovePosition(rb.position + direction.forward * sliderEsteiraUI.velocidade * Time.deltaTime);

                empurrando = true;
            }
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Copo"))
        {
            other.TryGetComponent<Rigidbody>(out rb);
            
            empurrando = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Copo") )
        {
            rb.MovePosition(rb.transform.position);

            rb = null;
            empurrando = false;
        }
    }
    private void OnCollisionStay(Collision otherThing)
    {
        // Get the direction of the conveyor belt 
        // (transform.forward is a built in Vector3 
        // which is used to get the forward facing direction)
        // * Remember Vector3's can used for position AND direction AND rotation
        directionz = transform.forward;
        directionz = directionz * velocidade;

        // Add a WORLD force to the other objects
        // Ignore the mass of the other objects so they all go the same speed (ForceMode.Acceleration)
        otherThing.rigidbody.AddForce(directionz, ForceMode.Acceleration);
    }

}
