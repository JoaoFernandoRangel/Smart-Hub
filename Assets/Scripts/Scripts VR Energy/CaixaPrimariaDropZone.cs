using UnityEngine;

namespace VREnergy.PRO
{
    public class CaixaPrimariaDropZone : MonoBehaviour
    {
        [SerializeField] PROAsset caixaPrimariaAsset;

        private void Awake()
        {
            if (caixaPrimariaAsset == null)
            {
                caixaPrimariaAsset = GetComponentInParent<PROAsset>();
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.TryGetComponent(out IPROAsset asset))
            {
                FindObjectOfType<ProcedureStageHandler>()?.NewAction(
                    activator: asset.UnityId,
                    receptor: caixaPrimariaAsset.UnityId,
                    interaction: States.Dentro.ToString()
                );
            }
        }
    }
}

