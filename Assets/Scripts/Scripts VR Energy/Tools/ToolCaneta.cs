using System;
using UnityEngine;

public class ToolCaneta : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent(out ToolEtiqueta etiqueta))
        {
            if (!etiqueta.EtiquetaPreenchida)
            {
                etiqueta.Escrever(this);
            }
        }
    }
}
