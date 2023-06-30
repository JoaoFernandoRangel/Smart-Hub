using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class ButtonController : MonoBehaviour
{

    [SerializeField]
    InputDeviceCharacteristics controllerCharacteristics;
    [SerializeField]
    GameObject handModelPrefab;
    [SerializeField]
    InputDevice targetDevice;

    [Tooltip("Event when the button starts being pressed")]
    public UnityEvent OnPress;

    [Tooltip("Event when the button is released")]
    public UnityEvent OnRelease;

    public bool IsPressed { get; private set; }



    void TryInitialize()
    {
        List<InputDevice> devices = new List<InputDevice>();

        InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

        foreach (var item in devices)
        {
            print(item.name + item.characteristics);
        }

        if (devices.Count > 0)
        {
            targetDevice = devices[0];
            print(devices[0].name);
        }
    }
    void UpdateHandAnimator()
    {

        if (targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool triggerValue))
        {
            print(triggerValue);
        }

    }
        


    void Update()
    {

        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        UpdateHandAnimator();
    }
}