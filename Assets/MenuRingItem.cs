using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class MenuRingItem : MonoBehaviour
{
   
    public bool estaNoMenu=false;

    public Transform parentToFollow;
    public bool pegou = false;
    [SerializeField]
    InputDeviceCharacteristics controllerCharacteristics;
    [SerializeField]
    InputDevice targetDevice;
    [SerializeField]
    public BoxCollider boxCollider;
    [SerializeField]
    public  Rigidbody rb;
    private void Start()
    {

    }
    private void Update()
    {
        if (!estaNoMenu)
        {
            transform.parent=null;
        }
        else
        {
            transform.parent = parentToFollow;
        }

        
    }
    private void OnTriggerEnter(Collider other)
    {

    }

    public void OnPegar()
    {
        pegou = true;
        estaNoMenu = false;
        boxCollider.isTrigger = false;
        rb.constraints = RigidbodyConstraints.None;
    }
    public void OnColocar()
    {
        pegou = false;
        estaNoMenu = true;
        boxCollider.isTrigger = true;
        rb.constraints = RigidbodyConstraints.FreezeAll; 
    }
}
