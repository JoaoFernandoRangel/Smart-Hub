using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VREnergy.PRO;

namespace VREnergy
{
    public class XRCadeadoCaixaPrimariaSocket : SocketKey
    {
        [SerializeField] private PROAsset proAsset;
        
        private ILock cadeado;

        protected override void Awake()
        {
            base.Awake();
            if (proAsset == null)
            {
                proAsset = GetComponent<PROAsset>();
            }
        }

        protected override void OnSelectEntered(SelectEnterEventArgs interactable)
        {
            if (interactable == null) { return; }
            if (interactable.interactableObject.transform.TryGetComponent(out ILock cadeado))
            {
                cadeado.onLock += ActionTrancarCadeado;
            }
            
            ProcedureStageHandler.Instance.NewAction(
                activator: interactable.interactableObject.transform.GetComponent<IPROAsset>().UnityId,
                receptor: proAsset.UnityId,
                interaction: States.Dentro.ToString()
            );
            
            base.OnSelectEntered(interactable);
        }

        protected override void OnSelectExited(SelectExitEventArgs interactable)
        {
            if (interactable == null) { return; }
            if (interactable.interactableObject.transform.TryGetComponent(out ILock cadeado))
            {
                cadeado.onLock -= ActionTrancarCadeado;
            }
            
            ProcedureStageHandler.Instance.NewAction(
                activator: interactable.interactableObject.transform.GetComponent<IPROAsset>().UnityId,
                receptor: proAsset.UnityId,
                interaction: States.Fora.ToString()
            );
            
            base.OnSelectExited(interactable);
        }

        private void ActionTrancarCadeado()
        {
            ProcedureStageHandler.Instance.NewAction(
                activator: "Operador",
                receptor: proAsset.UnityId,
                interaction: States.Travar.ToString()
            );
        }
    }
}