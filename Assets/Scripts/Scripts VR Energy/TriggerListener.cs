using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

public class TriggerListener : MonoBehaviour
{
    public static TriggerListener main;

    public float clickSensibility = 0.8f;
    public float releaseSensibility = 0.2f;

    private bool rightTriggerPressed = false;
    private bool leftTriggerPressed = false;
    private InputDevice leftInputDevice;
    private InputDevice rightInputDevice;

    public static event Action<string> OnTriggetButtonPress;

    private void Awake()
    {
        if (main == null)
        {
            main = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }
    
    private void Start()
    {
        var rightHandDevices = new List<InputDevice>();
        var leftHandDevices = new List<InputDevice>();

        InputDevices.GetDevicesAtXRNode(XRNode.RightHand, rightHandDevices);
        InputDevices.GetDevicesAtXRNode(XRNode.LeftHand, leftHandDevices);

        if (rightHandDevices.Count > 0)
            rightInputDevice = rightHandDevices[0];
        
        if (leftHandDevices.Count > 0)
            leftInputDevice = leftHandDevices[0];
    }
    
    private void OnEnable()
    {
        InputDevices.deviceConnected += RegisterDevices;
    }

    private void OnDisable()
    {
        InputDevices.deviceConnected -= RegisterDevices;
    }
    
    private void Update()
    {
        GetRightHandInput();
        GetLeftHandInput();
    }

    private void GetLeftHandInput()
    {
        if (leftInputDevice.isValid)
        {
            if (leftInputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                bool before = leftTriggerPressed;
                if (triggerValue > clickSensibility)
                {
                    leftTriggerPressed = true;
                }
                else if (triggerValue < releaseSensibility)
                {
                    leftTriggerPressed = false;
                }

                if ((before != leftTriggerPressed) && leftTriggerPressed)
                {
                    OnTriggetButtonPress?.Invoke("left");
                }
            }
        }
    }

    private void GetRightHandInput()
    {
        if (rightInputDevice.isValid)
        {
            if (rightInputDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
            {
                bool before = rightTriggerPressed;
                if (triggerValue > clickSensibility)
                {
                    rightTriggerPressed = true;
                }
                else if (triggerValue < releaseSensibility)
                {
                    rightTriggerPressed = false;
                }

                if ((before != rightTriggerPressed) && rightTriggerPressed)
                {
                    OnTriggetButtonPress?.Invoke("right");
                }
            }
        }
    }

    private void RegisterDevices(InputDevice connectedDevice)
    {
        if (!connectedDevice.isValid) return;
        
        if ((connectedDevice.characteristics & InputDeviceCharacteristics.HeldInHand) == InputDeviceCharacteristics.HeldInHand)
        {
            if ((connectedDevice.characteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left)
            {
                leftInputDevice = connectedDevice;
            }
            else if ((connectedDevice.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
            {
                rightInputDevice = connectedDevice;
            }
        }
    }
}
