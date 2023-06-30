using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VREnergy.PRO;

public class LaptopScript : MonoBehaviour
{
    [SerializeField]
    List<Material> materials = new List<Material>();

    [SerializeField]
    MeshRenderer materialMeshRenderer;
    [SerializeField]
    int index=0;

    public void PassarMaterial()
    {
        if (GetComponent<PROAsset>().IsAssetActive)
        {
            index++;
            materialMeshRenderer.material = materials[index];
        }
   
    }
}
