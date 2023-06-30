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
    Rigidbody rb;
    bool empurrando = false;
    public float Velocidade { get => velocidade; set => velocidade = value; }

    private void OnTriggerStay(Collider other)
    {
        if (empurrando)
        {
            rb.MovePosition(rb.position + direction.forward * sliderEsteiraUI.velocidade * Time.deltaTime);
            
        
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
        if (other.gameObject.CompareTag("Copo"))
        {
            rb.MovePosition(rb.transform.position);

            rb = null;
            empurrando = false;
        }
    }

}
