using UnityEngine;
using UnityEngine.XR;

namespace VREnergy.VR.Hand
{
    public class DefaultDeviceBasedHandAnimator : DeviceBasedHandAnimator
    {
        private readonly int animParamGripHash    = Animator.StringToHash("Grip");
        private readonly int animParamTriggerHash = Animator.StringToHash("Trigger");

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
        }
    }
}