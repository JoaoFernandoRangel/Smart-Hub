using UnityEngine;
using UnityEngine.XR;

namespace VREnergy.VR.Hand
{
    public class LuvaDuplaDeviceBasedHandAnimator : DeviceBasedHandAnimator
    {
        [SerializeField] private float thumbMoveSpeed = 7f;
        private float thumbLerpValue;
        
        private readonly int animParamGripHash    = Animator.StringToHash("Grip");
        private readonly int animParamTriggerHash = Animator.StringToHash("Trigger");
        private readonly int animParamThumbHash   = Animator.StringToHash("Thumb");

        protected override void AnimateHand()
        {
            if (device.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                animator.SetFloat(animParamTriggerHash, triggerValue);
            }
            else
            {
                animator.SetFloat(animParamTriggerHash, 0.0f);
            }

            if (device.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
            {
                animator.SetFloat(animParamGripHash, gripValue);
            }
            else
            {
                animator.SetFloat(animParamGripHash, 0.0f);
            }
        
            if (device.TryGetFeatureValue(CommonUsages.primary2DAxisTouch, out bool axisTouchValue))
            {
                float thumbValue = Mathf.Lerp(0f, 1f, thumbLerpValue);
                if (axisTouchValue)
                {
                    thumbLerpValue += thumbMoveSpeed * Time.deltaTime;
                }
                else
                {
                    thumbLerpValue -= thumbMoveSpeed * Time.deltaTime;
                }

                thumbLerpValue = Mathf.Clamp(thumbLerpValue, 0f, 1f);
                animator.SetFloat(animParamThumbHash, thumbValue);
            }
            else
            {
                animator.SetFloat(animParamThumbHash, 0f);
            }
        }
    }
}