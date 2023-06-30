using UnityEngine;

namespace VREnergy.PRO
{
    public class PROKeySocketInteraction : MonoBehaviour
    {
        private IPROAsset proAsset;
        private ILock socketLock;

        private void Awake()
        {
            proAsset = GetComponent<IPROAsset>();
            socketLock = GetComponent<ILock>();
            socketLock.onLock += OnLock;
            socketLock.onUnlock += OnUnlock;
        }

        private void OnUnlock()
        {
            FindObjectOfType<ProcedureStageHandler>().NewAction(
                activator: "Operador",
                receptor: proAsset.UnityId,
                interaction: States.Abrir.ToString()
            );
        }

        private void OnLock()
        {
            FindObjectOfType<ProcedureStageHandler>().NewAction(
                activator: "Operador",
                receptor: proAsset.UnityId,
                interaction: States.Fechar.ToString()
            );
        }
    }
}