using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR;

public class VRInputController : MonoBehaviour
{
    [SerializeField]
    bool showController = false;
    [SerializeField]
    List<GameObject> controllerPrefabs;
    [SerializeField]
    InputDeviceCharacteristics controllerCharacteristics;
    [SerializeField]
    GameObject handModelPrefab;
    [SerializeField]
    InputDevice targetDevice;

    Animator handAnimator;

    GameObject spawnedController;
    GameObject spawnedHandModel;

    public Vector2 primaryControllerAxis;
    public float triggerButton;
    [Header("Eventos Botao primario")]
    [Tooltip("Event when the button starts being pressed")]
    public UnityEvent OnPress;

    [Tooltip("Event when the button is released")]
    public UnityEvent OnRelease;
    
    [Header("Eventos Grip")]
    [Tooltip("Event when the button starts being pressed")]
    public UnityEvent OnGrip;

    [Header("Eventos Botao secundario")]

    [Tooltip("Event when the button starts being pressed")]
    public UnityEvent OnPressSecondary;

    [Tooltip("Event when the button is released")]
    public UnityEvent OnReleaseSecondary;

    public static float GripAmount;

    bool buttonPress = true;
    private bool buttonPrimaryPress;

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
            GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
            if (prefab)
            {
                spawnedController = Instantiate(prefab, transform);
            }
            else
            {
                print(targetDevice.name);
                print("nao achei o modelo do controle");
                spawnedController = Instantiate(controllerPrefabs[0], transform);
            }

            spawnedHandModel = Instantiate(handModelPrefab, transform);
            handAnimator = spawnedHandModel.GetComponent<Animator>();
        }
    }
    void UpdateHandAnimator()
    {

        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            
            handAnimator.SetFloat("Trigger", triggerValue);
        }
        else
        {
            handAnimator.SetFloat("Trigger", 0);
        }



        if (targetDevice.TryGetFeatureValue(CommonUsages.primaryButton,  out bool button))
        {
            if (button && buttonPress)
            {
                buttonPress = false;

                print(button);
                OnPress.Invoke();
            }
            else if(!button && !buttonPress)
            {
                buttonPress = true;
                print(button);
                OnRelease.Invoke();
            }
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.secondaryButton, out bool buttonPrimary))
        {
            if (buttonPrimary && buttonPrimaryPress)
            {
                buttonPrimaryPress = false;

                OnPressSecondary.Invoke();
            }
            else if (!buttonPrimary && !buttonPrimaryPress)
            {
                buttonPrimaryPress = true;
                OnReleaseSecondary.Invoke();
            }
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripAmount))
        {
            gripAmount = GripAmount;
            if (gripAmount >0.1)
            {
 
                OnGrip.Invoke();
            }

        }

        if (targetDevice.TryGetFeatureValue(CommonUsages.grip, out float gripValue))
        {
            handAnimator.SetFloat("Grip", gripValue);
        }
        else
        {
            handAnimator.SetFloat("Grip", 0);
        }
    }



    void Update()
    {

        UpdateControllerValues();

        if (!targetDevice.isValid)
        {
            TryInitialize();
        }
        if (showController)
        {
            spawnedHandModel.SetActive(false);
            spawnedController.SetActive(true);
        }
        else
        {
            if (spawnedHandModel != null)
            {
                spawnedHandModel.SetActive(true);
                spawnedController.SetActive(false);
                UpdateHandAnimator();
                
            }
            else
            {
                //print("Controle desconectado");
            }

        }
    }



    private void UpdateControllerValues()
    {
        if (targetDevice.TryGetFeatureValue(CommonUsages.primary2DAxis, out Vector2 axisValue))
        {
            primaryControllerAxis = axisValue;
        }
        if (targetDevice.TryGetFeatureValue(CommonUsages.trigger, out float triggerValue))
        {
            triggerButton = triggerValue;
        }
    }
}
