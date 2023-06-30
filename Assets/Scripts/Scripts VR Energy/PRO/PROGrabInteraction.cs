using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VREnergy.PRO
{
    /// <summary>
    /// Classe responsável por avisar o controlador de passos quando acontecer uma interação de pegar.
    /// </summary>
    public class PROGrabInteraction : MonoBehaviour
    {
        [SerializeField, Tooltip("Caso vazio, o XRBaseInteractor será pego automaticamente.")]
        private XRBaseInteractor handInteractor;

        #region MONOBEHAVIOUR

        private void Awake()
        {
            if (handInteractor == null)
            {
                handInteractor = GetComponent<XRBaseInteractor>();
            }
        }

        private void OnEnable()
        {
            handInteractor.selectEntered.AddListener(GrabInteraction);
        }

        private void OnDisable()
        {
            handInteractor.selectEntered.RemoveListener(GrabInteraction);
        }

        #endregion

        private void GrabInteraction(SelectEnterEventArgs interactable)
        {
            if (interactable.interactableObject.transform.TryGetComponent(out IPROAsset asset))
            {
                FindObjectOfType<ProcedureStageHandler>()?.NewAction(new PROAction
                {
                    Activator = "Operador",
                    Receptor = asset.UnityId,
                    Interaction = States.Pegar.ToString()
                });
            }
        }
    }
}
