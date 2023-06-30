using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MaterialControl : MonoBehaviour
{
    [Header("Referencias")]
    public Material redMaterial;
    public Material blueMaterial;
    MeshRenderer m_MeshRenderer;
    private void Start()
    {
        m_MeshRenderer = GetComponent<MeshRenderer>();  
    }
    public void ChangeMaterial(string color)
    {
        if (color == "red")
        {
            m_MeshRenderer.material = redMaterial;
        }
        if (color == "blue")
        {
            m_MeshRenderer.material = blueMaterial;
        }
    }
}
