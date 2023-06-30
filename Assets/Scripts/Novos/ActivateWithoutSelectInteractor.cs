using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;
using UnityEngine.XR.Interaction.Toolkit;

namespace VRWorkshop
{
    public class ActivateWithoutSelectInteractor : MonoBehaviour
    {
        [SerializeField]
        private XRDirectInteractor xrDirectInteractor;

        private List<ActivateWithoutSelectInteractable> _currentHoveredInteractables = new List<ActivateWithoutSelectInteractable>();
        private ActionBasedController _actionBasedController;

        private readonly float activateThreshold = 0.7f;

        private void Awake()
        {
            _actionBasedController = GetComponent<ActionBasedController>();
            if (xrDirectInteractor == null)
            {
                xrDirectInteractor = GetComponent<XRDirectInteractor>();
            }
        }

        private void OnEnable()
        {
            xrDirectInteractor.hoverEntered.AddListener(OnHoverEntered);
            xrDirectInteractor.hoverExited.AddListener(OnHoverExited);
            
            _actionBasedController.activateAction.action.performed += OnActivatePerformed;
            _actionBasedController.activateAction.action.canceled += OnActivateCanceled;
        }
        
        private void OnDisable()
        {
            xrDirectInteractor.hoverEntered.RemoveListener(OnHoverEntered);
            xrDirectInteractor.hoverExited.RemoveListener(OnHoverExited);
            
            _actionBasedController.activateAction.action.performed -= OnActivatePerformed;
            _actionBasedController.activateAction.action.canceled -= OnActivateCanceled;
        }

        private void OnActivatePerformed(InputAction.CallbackContext callbackContext)
        {
            float value = ReadValue(callbackContext.action);
            if (value >= activateThreshold)
            {
                ActivateAllInteractables();
            }
        }
        
        private void OnActivateCanceled(InputAction.CallbackContext callbackContext)
        {
            DeactivateAllInteractables();
        }

        private void ActivateAllInteractables()
        {
            foreach (var interactable in _currentHoveredInteractables)
            {
                interactable.OnActivated(new ActivateEventArgs
                {
                    interactableObject = interactable.GetComponent<IXRActivateInteractable>(),
                    interactorObject = GetComponent<IXRActivateInteractor>()
                });
            }
        }
        
        private void DeactivateAllInteractables()
        {
            foreach (var interactable in _currentHoveredInteractables)
            {
                interactable.OnDeactivated(new DeactivateEventArgs
                {
                    interactableObject = interactable.GetComponent<IXRActivateInteractable>(),
                    interactorObject = GetComponent<IXRActivateInteractor>()
                });
            }
        }

        private void OnHoverEntered(HoverEnterEventArgs args)
        {
            if (args.interactableObject.transform.TryGetComponent(out ActivateWithoutSelectInteractable interactable))
            {
                _currentHoveredInteractables.Add(interactable);
            }
        }
        
        private void OnHoverExited(HoverExitEventArgs args)
        {
            if (args.interactableObject.transform.TryGetComponent(out ActivateWithoutSelectInteractable interactable))
            {
                _currentHoveredInteractables.Remove(interactable);

                // Caso o Interactor estiver ativando o Interactable, tente desativar.
                if (IsInteractorActivatingInteractable(interactable))
                {
                    interactable.OnDeactivated(new DeactivateEventArgs
                    {
                        interactableObject = interactable.GetComponent<IXRActivateInteractable>(),
                        interactorObject = GetComponent<IXRActivateInteractor>()
                    });
                }
            }
        }

        private bool IsInteractorActivatingInteractable(ActivateWithoutSelectInteractable interactable)
        {
            return interactable.HandInteractorsActivating.Contains(GetComponent<IXRActivateInteractor>());
        }
        
        protected virtual bool IsPressed(InputAction action)
        {
            if (action == null)
                return false;
            
            return action.phase == InputActionPhase.Performed;
        }
        
        protected virtual float ReadValue(InputAction action)
        {
            if (action == null)
                return default;

            if (action.activeControl is AxisControl)
                return action.ReadValue<float>();

            if (action.activeControl is Vector2Control)
                return action.ReadValue<Vector2>().magnitude;

            return IsPressed(action) ? 1f : 0f;
        }
    }
}
