using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR;

namespace VREnergy.VR.Hand
{
    public class HandPresence : MonoBehaviour
    {
        public bool showController = false;

        public InputDeviceCharacteristics controllerCharacteristics;
        public List<GameObject> controllerPrefabs;
        public GameObject handModelPrefab;

        private InputDevice targetDevice;
        private GameObject spawnedController;
        private GameObject spawnedHandModel;
        private GameObject defaultHandModel;

        void Start()
        {
            TryInitialize();
        }

        void TryInitialize()
        {
            List<InputDevice> devices = new List<InputDevice>();
            InputDevices.GetDevicesWithCharacteristics(controllerCharacteristics, devices);

            foreach (var device in devices)
            {
                Debug.Log(device.name + device.characteristics);
            }

            if (devices.Count > 0 && !spawnedController)
            {
                targetDevice = devices[0];
                GameObject prefab = controllerPrefabs.Find(controller => controller.name == targetDevice.name);
                if (prefab)
                {
                    spawnedController = Instantiate(prefab, transform);
                }
                else
                {
                    Debug.LogError("Did not Find corresponding controller model");
                    spawnedController = Instantiate(controllerPrefabs[0], transform);
                }
            }
            if (!spawnedHandModel)
            {
                spawnedHandModel = Instantiate(handModelPrefab, transform);
                defaultHandModel = spawnedHandModel;
            }
        }
    
        void Update()
        {
            if (!targetDevice.isValid)
            {
                TryInitialize();
                //Debug.LogError("A Device Was Not Recognized");
            }
            else
            {
                if (showController)
                {
                    spawnedController.SetActive(true);
                    spawnedHandModel.SetActive(false);
                }
                else
                {
                    spawnedController.SetActive(false);
                    spawnedHandModel.SetActive(true);
                }
            }
        }

        public void ChangeHandModel(GameObject newHandModel)
        {
            if (spawnedHandModel == defaultHandModel)
            {
                spawnedHandModel = Instantiate(newHandModel, transform);
                defaultHandModel.SetActive(false);
            }
        }

        public void RemoveHandModel()
        {
            if (spawnedHandModel == defaultHandModel) return;
        
            Destroy(spawnedHandModel);
            spawnedHandModel = defaultHandModel;
            defaultHandModel.SetActive(true);
        }
    }
}
