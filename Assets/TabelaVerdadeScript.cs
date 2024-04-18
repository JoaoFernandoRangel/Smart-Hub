using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TabelaVerdadeScript : MonoBehaviour
{
    [Header("Referencias e Estados")]

    [SerializeField]
    TrackingScript trackingScript;
    [SerializeField]
    string statusDoSistema;
    [Header("RedMaterial")]
    [SerializeField]
    Material redMaterial;
    [SerializeField]
    Material redOverlayMaterial;
    [Header("GreenMaterial")]
    [SerializeField]
    Material greenMaterial;
    [SerializeField]
    Material greenOverlayMaterial;
    [Header("Modelo")]
    [SerializeField]
    MeshRenderer insideMeshRenderer;
    [SerializeField]
    MeshRenderer outsideMeshRenderer;
    private void Start()
    {
        //trackingScript.GarraValueChanged += TrackingScript_GarraValueChanged;
    }
    private void Update()
    {
        TrackingScript_GarraValueChanged();
    }
    private void TrackingScript_GarraValueChanged()
    {
        
        int valorPotInt = int.Parse(trackingScript.valorPotenciometroGarra);
        if (trackingScript.AberturaGarra == "GF" && valorPotInt > 3000)
        {
            statusDoSistema = "Falha";
            MainThreadDispatcher.Instance.Enqueue(() => RedMaterial());
        }
        if (trackingScript.AberturaGarra == "GA" && valorPotInt > 3000)
        {
            statusDoSistema = "Erro de leitura";
            MainThreadDispatcher.Instance.Enqueue(() => RedMaterial());

        }
        if (trackingScript.AberturaGarra == "GF" && valorPotInt < 100)
        {
            statusDoSistema = "Captura";
            MainThreadDispatcher.Instance.Enqueue(() => GreenMaterial());
        }
        if (trackingScript.AberturaGarra == "GA" && valorPotInt < 100)
        {
            statusDoSistema = "Soltou o Copo";
            MainThreadDispatcher.Instance.Enqueue(() => GreenMaterial());

        }
    }

    private void RedMaterial()
    {
        insideMeshRenderer.material = redMaterial;
        outsideMeshRenderer.material = redOverlayMaterial;
    }    
    private void GreenMaterial()
    {
        insideMeshRenderer.material = greenMaterial;
        outsideMeshRenderer.material = greenOverlayMaterial;
    }
}
