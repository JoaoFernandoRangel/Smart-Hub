using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VREnergy.PRO;

public class MenuRingScript : MonoBehaviour
{
    [SerializeField]
    List<GameObject> ferramentas = new List<GameObject>();
    [SerializeField]
    List<GameObject> pontosDeSpawn = new List<GameObject>();

    [Header("O que o object vai seguir")]
    [SerializeField]
    GameObject objectASeguir;
    [SerializeField]
    bool vaiSeguirAlgo;
    [SerializeField]
    Vector3 offset;

    private void Start()
    {
        for (int i = 0; i < ferramentas.Count; i++)
        {
            ferramentas[i].TryGetComponent<MenuRingItem>(out MenuRingItem menuItem);
            
            if (menuItem)
            {
                menuItem.estaNoMenu = true;
                menuItem.boxCollider.isTrigger = true;
                menuItem.rb.constraints = RigidbodyConstraints.FreezeAll;
                menuItem.parentToFollow = pontosDeSpawn[i].transform;

            }

            Instantiate(ferramentas[i], pontosDeSpawn[i].transform.position, transform.rotation, pontosDeSpawn[i].transform);


            ferramentas[i].transform.position = new Vector3(0, 0, 0);
        }
    }

    private void Update()
    {
        if (vaiSeguirAlgo)
        {
            transform.position = objectASeguir.transform.position + offset;
        }
        
    }


}




