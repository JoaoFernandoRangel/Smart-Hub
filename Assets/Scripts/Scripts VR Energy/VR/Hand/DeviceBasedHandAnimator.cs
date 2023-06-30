using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.XR;

namespace VREnergy.VR.Hand
{
    [RequireComponent(typeof(Animator))]
    public abstract class DeviceBasedHandAnimator : MonoBehaviour
    {
        [SerializeField] protected Animator animator;
        [SerializeField] protected InputDeviceCharacteristics controllerCharacteristics;
        protected InputDevice device;
        
        protected virtual void Awake()
        {
            if (animator == null)
            {
                animator = GetComponent<Animator>();
            }

            InitDevice();
        }

        protected virtual void OnEnable()
        {
            InputDevices.deviceConnected += RegisterDevice;
        }

        protected virtual void OnDisable()
        {
            InputDevices.deviceConnected -= RegisterDevice;
        }

        protected virtual void Update()
        {
            if (!device.isValid) return;
            AnimateHand();
        }

        private void RegisterDevice(InputDevice connectedDevice)
        {
            if (connectedDevice.isValid)
            {
                if ((connectedDevice.characteristics & controllerCharacteristics) == controllerCharacteristics)
                {
                    device = connectedDevice;
                }
            }
        }

        private void InitDevice()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);
            device = devices.FirstOrDefault();
        }

        protected abstract void AnimateHand();
    }
}