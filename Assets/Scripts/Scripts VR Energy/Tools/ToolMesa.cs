using UnityEngine;
using System.Collections.Generic;
using VREnergy.PRO;

public class ToolMesa : MonoBehaviour
{
    private List<IPROAsset> proAssets = new List<IPROAsset>();

    private void OnTriggerEnter(Collider other)
    {
        var parent = other.attachedRigidbody;
        if (other.TryGetComponent(out IPROAsset proAsset))
        {
            if (!proAssets.Contains(proAsset))
            {
                proAssets.Add(proAsset);
                proAsset.AssetInteraction(new PROAction
                {
                    Activator = proAsset.UnityId,
                    Receptor = GetComponent<IPROAsset>().UnityId,
                    Interaction = "Entrar"
                });
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        var parent = other.attachedRigidbody;
        if (other.TryGetComponent(out IPROAsset proAsset))
        {
            if (proAssets.Contains(proAsset))
            {
                proAssets.Remove(proAsset);
                proAsset.AssetInteraction(new PROAction
                {
                    Activator = proAsset.UnityId,
                    Receptor = GetComponent<IPROAsset>().UnityId,
                    Interaction = "Sair"
                });
            }
        }
    }
}