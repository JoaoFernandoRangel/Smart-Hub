using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRWorkshop
{
    public class ActivateWithoutSelectInteractable : MonoBehaviour
    {
        [SerializeField]
        private ActivateEvent activated;
        public ActivateEvent Activated => activated;
        
        [SerializeField]
        private DeactivateEvent deactivated;
        public DeactivateEvent Deactivated => deactivated;

        [SerializeField]
        private XRBaseInteractable interactable;
        
        /// <summary>
        /// Lista das mãos que estão em hover com esse Interactable.
        /// </summary>
        public List<IXRHoverInteractor> HandInteractorsHovering { get; } = new List<IXRHoverInteractor>();
        
        /// <summary>
        /// Lista das mãos que estão ativando esse Interactable.
        /// </summary>
        public List<IXRActivateInteractor> HandInteractorsActivating { get; } = new List<IXRActivateInteractor>();

        public bool IsHoveredByController => HandInteractorsHovering.Count > 0;
        public bool IsActivatedByController => HandInteractorsActivating.Count > 0;
        
        private bool _isActivated;
        /// <summary>
        /// Diz se esse interactable já foi ativado.
        /// </summary>
        public bool IsActivated => _isActivated;

        private void Awake()
        {
            if (interactable == null)
            {
                interactable = GetComponent<XRBaseInteractable>();
            }
        }

        private void OnEnable()
        {
            interactable.hoverEntered.AddListener(OnHoverEntered);
            interactable.hoverExited.AddListener(OnHoverExited);
        }
        
        private void OnDisable()
        {
            interactable.hoverEntered.RemoveListener(OnHoverEntered);
            interactable.hoverExited.RemoveListener(OnHoverExited);
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactorObject.transform.TryGetComponent(out ActivateWithoutSelectInteractor interactor))
            {
                HandInteractorsHovering.Add(interactor.GetComponent<IXRHoverInteractor>());
            }
        }
        
        private void OnHoverExited(HoverExitEventArgs args)
        {
            if (args.interactorObject.transform.TryGetComponent(out ActivateWithoutSelectInteractor interactor))
            {
                HandInteractorsHovering.Remove(interactor.GetComponent<IXRHoverInteractor>());
            }
        }
        
        /// <summary>
        /// O <see cref="ActivateWithoutSelectInteractor"/> chama esse método quando
        /// começa uma ativação nesse Interactable.
        /// </summary>
        /// <seealso cref="OnDeactivated"/>
        public void OnActivated(ActivateEventArgs args)
        {
            if (args.interactorObject.transform.TryGetComponent(out ActivateWithoutSelectInteractor interactor))
            {
                HandInteractorsActivating.Add(interactor.GetComponent<IXRActivateInteractor>());
            }
            
            // Se estiver ativado, ignore
            if (_isActivated)
                return;

            _isActivated = true;
            activated?.Invoke(args);
        }

        /// <summary>
        /// O <see cref="ActivateWithoutSelectInteractor"/> chama esse método quando
        /// termina a ativação nesse Interactable.
        /// </summary>
        /// <seealso cref="OnDeactivated"/>
        public void OnDeactivated(DeactivateEventArgs args)
        {
            bool interactableRemoved = false;
            if (args.interactorObject.transform.TryGetComponent(out ActivateWithoutSelectInteractor interactor))
            {
                interactableRemoved = HandInteractorsActivating.Remove(interactor.GetComponent<IXRActivateInteractor>());
            }

            // Se não estiver ativado, ignore
            if (!_isActivated)
                return;
            
            if (!(HandInteractorsActivating.Count == 0 && interactableRemoved))
                return;

            _isActivated = false;
            deactivated?.Invoke(args);
        }
    }
}
