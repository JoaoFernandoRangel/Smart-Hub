using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VREnergy.PRO;

public class ToolChaveAllen : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ToolDisjuntor etiqueta))
        {
            if (other.TryGetComponent(out PROAsset asset))
            {
                if (asset.IsAssetActive)
                {
                    if (!etiqueta.EtiquetaPreenchida)
                    {
                        etiqueta.Escrever();
                    }
                }
            }
        }
    }
}
