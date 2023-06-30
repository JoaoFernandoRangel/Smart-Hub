using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCopoScript : MonoBehaviour
{
    [SerializeField]
    GameObject SpawnPoint;
    [SerializeField]
    GameObject Copo;

    [ContextMenu("Criar copo")]
    public void CriarCopo()
    {
        Instantiate(Copo,SpawnPoint.transform.position, Copo.transform.rotation);
    }
}
