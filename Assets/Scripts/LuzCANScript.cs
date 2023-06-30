using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LuzCANScript : MonoBehaviour
{
    [SerializeField]
    List<MeshRenderer> luzesMaterial;
    [SerializeField]
    Material verdeMaterial;
    public void AcenderLuz(int x)
    {
        luzesMaterial[x].material = verdeMaterial;
    } 
}
