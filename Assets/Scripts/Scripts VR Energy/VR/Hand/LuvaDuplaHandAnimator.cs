using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.XR.Interaction.Toolkit.Inputs;

namespace VREnergy.VR.Hand
{
    public class LuvaDuplaHandAnimator : HandAnimator
    {
        [SerializeField] private InputActionProperty handGripAction;
        [SerializeField] private InputActionProperty handTriggerAction;
        [SerializeField] private InputActionProperty handThumbAction;
        [SerializeField] private float thumbMoveSpeed = 7f;
        
        private float _gripValue;
        private float _triggerValue;
        private float _thumbValue;
        
        private float _thumbLerpValue;
        
        private readonly int animParamGripHash    = Animator.StringToHash("Grip");
        private readonly int animParamTriggerHash = Animator.StringToHash("Trigger");
        private readonly int animParamThumbHash   = Animator.StringToHash("Thumb");

        private void OnEnable()
        {
            handGripAction.EnableDirectAction();
            handTriggerAction.EnableDirectAction();
            handThumbAction.EnableDirectAction();

            handGripAction.action.performed += UpdateGripValue;
            handTriggerAction.action.performed += UpdateTriggerValue;
            handThumbAction.action.performed += UpdateThumbValue;
            handThumbAction.action.canceled += UpdateThumbValue;
        }

        private void OnDisable()
        {
            handGripAction.action.performed -= UpdateGripValue;
            handTriggerAction.action.performed -= UpdateTriggerValue;
            handThumbAction.action.performed -= UpdateThumbValue;
            handThumbAction.action.canceled -= UpdateThumbValue;
            
            handGripAction.DisableDirectAction();
            handTriggerAction.DisableDirectAction();
            handThumbAction.DisableDirectAction();
        }

        protected override void AnimateHand()
        {
            base.AnimateHand();
            
            LerpThumbValue();
            
            animator.SetFloat(animParamGripHash, _gripValue);
            animator.SetFloat(animParamTriggerHash, _triggerValue);
            animator.SetFloat(animParamThumbHash, _thumbLerpValue);
        }

        private void UpdateGripValue(InputAction.CallbackContext context)
        {
            _gripValue = context.ReadValue<float>();
        }

        private void UpdateTriggerValue(InputAction.CallbackContext context)
        {
            _triggerValue = context.ReadValue<float>();
        }

        private void UpdateThumbValue(InputAction.CallbackContext context)
        {
            _thumbValue = context.ReadValue<float>();
            Debug.Log(context);
            Debug.Log(context.ReadValue<float>());
        }

        private void LerpThumbValue()
        {
            
            if (_thumbValue > 0)
                _thumbLerpValue += thumbMoveSpeed * Time.deltaTime;
            else
                _thumbLerpValue -= thumbMoveSpeed * Time.deltaTime;
        
            //float thumbValue = Mathf.Lerp(0f, 1f, _thumbLerpValue);
            _thumbLerpValue = Mathf.Clamp(_thumbLerpValue, 0f, 1f);
        }
    }
}