using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VREnergy.PRO
{
    /// <summary>
    /// Classe responsável por avisar o controlador de passos quando acontecer uma interação de equipar/desequipar EPI.
    /// </summary>
    public class PROEpiInteraction : MonoBehaviour
    {
        [SerializeField, Tooltip("Caso vazio, o XRBaseInteractor será pego automaticamente.")]
        private XRBaseInteractor socket;
        
        #region MONOBEHAVIOUR

        private void Awake()
        {
            if (socket == null)
            {
                socket = GetComponent<XRBaseInteractor>();
            }
        }

        private void OnEnable()
        {
            socket.selectEntered.AddListener(EquipEpiInteraction);
            socket.selectExited.AddListener(UnequipEpiInteraction);
        }

        private void OnDisable()
        {
            socket.selectEntered.RemoveListener(EquipEpiInteraction);
            socket.selectExited.RemoveListener(UnequipEpiInteraction);
        }

        #endregion

        private void EquipEpiInteraction(SelectEnterEventArgs interactable)
        {
            if (interactable == null) { return; }
            if (interactable.interactableObject.transform.TryGetComponent(out IPROAsset asset))
            {
                FindObjectOfType<ProcedureStageHandler>()?.NewAction(new PROAction
                {
                    Activator = "Operador",
                    Receptor = asset.UnityId,
                    Interaction = States.Colocar.ToString()
                });
            }
        }
        
        private void UnequipEpiInteraction(SelectExitEventArgs interactable)
        {
            if (interactable == null) { return; }
            if (interactable.interactableObject.transform.TryGetComponent(out IPROAsset asset))
            {
                FindObjectOfType<ProcedureStageHandler>()?.NewAction(new PROAction
                {
                    Activator = "Operador",
                    Receptor = asset.UnityId,
                    Interaction = States.Retirar.ToString()
                });
            }
        }
    }
}
