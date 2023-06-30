using System;
using UnityEngine;

namespace VREnergy.PRO
{
    public class PROOperadorInteraction : MonoBehaviour
    {
        [SerializeField] private PROAsset proAsset;
        [SerializeField] private States interaction;

        private void Awake()
        {
            if (proAsset == null)
            {
                proAsset = GetComponent<PROAsset>();
            }
        }

        public void Interaction()
        {
            FindObjectOfType<ProcedureStageHandler>().NewAction(
                activator: "Operador",
                receptor: proAsset.UnityId,
                interaction: interaction.ToString()
            );
        }
    }
}