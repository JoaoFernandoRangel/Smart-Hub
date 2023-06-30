using System;
using System.Collections.Generic;
using Unity.XR.CoreUtils;
using UnityEngine;
using UnityEngine.XR;
using UnityEngine.XR.Interaction.Toolkit;
using VREnergy.PRO;

namespace VREnergy.VR
{
    /// <summary>
    /// Controlador global do XRRig, irá lidar com a leitura de inputs para ativar ações especiais como o teleport.
    /// </summary>
    [DefaultExecutionOrder(-50)]
    public class XRMasterController : MonoBehaviour
    {
        private static XRMasterController s_instance = null;
        public static XRMasterController Instance => s_instance;

        private XROrigin _rig;
        public XROrigin Rig => _rig;

        //[Header("Setup")]
        //public bool disableSetupForDebug = false;
        //public Transform startingPosition;
        //public GameObject teleporterParent;
    
        [Header("Teleport Settings")]
        [SerializeField] 
        private float teleportActivationThresholdOculus = .7f;
        [SerializeField] 
        InputHelpers.Button teleportActivationButtonOculus = InputHelpers.Button.PrimaryAxis2DUp;
    
        [SerializeField] 
        private float teleportActivationThresholdVive = .5f;
        [SerializeField] 
        InputHelpers.Button teleportActivationButtonVive = InputHelpers.Button.Primary2DAxisClick;
        
        [Space]
        public XRRayInteractor rightTeleportInteractor;
        public XRRayInteractor leftTeleportInteractor;

        //public XRDirectInteractor rightDirectInteractor;
        //public XRDirectInteractor leftDirectInteractor;
        
        public event Action OnTeleportEnabled;
        public event Action OnTeleportDisabled;
        public event Action<XRBaseInteractable> OnTeleport;

        private InputDevice _leftInputDevice;
        private InputDevice _rightInputDevice;
        private VRControllerModel _leftInputDeviceType;
        private VRControllerModel _rightInputDeviceType;
    
        private Vector2 _teleportAxisInput;
        private bool _enableTeleport;
        private bool _wasTeleportInputPressedThisFrame = false;
        private bool _isTeleportActive;
#if UNITY_ANDROID
        private bool skipButtonPressedThisFrame;
#endif

        #region MONOBEHAVIOUR
    
        private void Awake()
        {
            s_instance = this;
            _rig = GetComponent<XROrigin>();
        }

        private void OnEnable()
        {
            InputDevices.deviceConnected += RegisterDevices;
        }

        private void OnDisable()
        {
            InputDevices.deviceConnected -= RegisterDevices;
        }

        private void Start()
        {
            rightTeleportInteractor.gameObject.SetActive(false);
            leftTeleportInteractor.gameObject.SetActive(false);

            if (_rig.CurrentTrackingOriginMode != TrackingOriginModeFlags.Floor)
                _rig.CameraYOffset = 1.8f;
            
            List<InputDevice> foundControllers = new List<InputDevice>();
        
            InputDeviceCharacteristics leftTrackedControllerFilter = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Left;
            InputDevices.GetDevicesWithCharacteristics(leftTrackedControllerFilter, foundControllers);
            if (foundControllers.Count > 0)
                RegisterDevices(foundControllers[0]);
        
            InputDeviceCharacteristics rightTrackedControllerFilter = InputDeviceCharacteristics.HeldInHand | InputDeviceCharacteristics.Right;
            InputDevices.GetDevicesWithCharacteristics(rightTrackedControllerFilter, foundControllers);
            if (foundControllers.Count > 0)
                RegisterDevices(foundControllers[0]);
        }
    
        private void Update()
        {

            if (Input.GetKeyDown(KeyCode.Escape))
                Application.Quit();
        
            _wasTeleportInputPressedThisFrame = false;
            
            RightTeleportUpdate();
            LeftTeleportUpdate();
            TeleportActiveCheck(_wasTeleportInputPressedThisFrame);
            
#if UNITY_STANDALONE
            if (Input.GetKeyDown(KeyCode.Space))
            {
                ProcedureStageHandler.Instance.PularPasso();
            }
#elif UNITY_ANDROID
             _leftInputDevice.TryGetFeatureValue(CommonUsages.menuButton, out bool menuButtonValue);
            
            if (menuButtonValue && !skipButtonPressedThisFrame)
            {
                ProcedureStageHandler.Instance.PularPasso();
                skipButtonPressedThisFrame = true;
            }
            else if(!menuButtonValue)
            {
                skipButtonPressedThisFrame = false;
            }
#endif
        }
        #endregion
    
        private void RightTeleportUpdate()
        {
            _enableTeleport = GetTeleportInput(_rightInputDevice, _rightInputDeviceType);
            rightTeleportInteractor.gameObject.SetActive(_enableTeleport);

            if (_enableTeleport) 
                _wasTeleportInputPressedThisFrame = true;
        }
    
        private void LeftTeleportUpdate()
        {
            _enableTeleport = GetTeleportInput(_leftInputDevice, _leftInputDeviceType);
            leftTeleportInteractor.gameObject.SetActive(_enableTeleport);
            
            if (_enableTeleport) 
                _wasTeleportInputPressedThisFrame = true;
        }
    
        private void RegisterDevices(InputDevice connectedDevice)
        {
            if (connectedDevice.isValid)
            {
                if ((connectedDevice.characteristics & InputDeviceCharacteristics.HeldInHand) == InputDeviceCharacteristics.HeldInHand)
                {
                    if ((connectedDevice.characteristics & InputDeviceCharacteristics.Left) == InputDeviceCharacteristics.Left)
                    {
                        _leftInputDevice = connectedDevice;
                        _leftInputDeviceType = XRHelpers.GetInputDeviceControllerModel(_leftInputDevice);
                        leftTeleportInteractor.GetComponent<XRController>().selectUsage =
                            GetTeleportActivationButton(_leftInputDeviceType);
                    
                    }
                    else if ((connectedDevice.characteristics & InputDeviceCharacteristics.Right) == InputDeviceCharacteristics.Right)
                    {
                        _rightInputDevice = connectedDevice;
                        _rightInputDeviceType = XRHelpers.GetInputDeviceControllerModel(_rightInputDevice);
                        rightTeleportInteractor.GetComponent<XRController>().selectUsage =
                            GetTeleportActivationButton(_rightInputDeviceType);
                    }
                }
            }
        }

        private InputHelpers.Button GetTeleportActivationButton(VRControllerModel controllerType)
        {
            switch (controllerType)
            {
                case VRControllerModel.Vive:
                    return teleportActivationButtonVive;
                case VRControllerModel.Oculus:
                    return teleportActivationButtonOculus;
                default:
                    return teleportActivationButtonOculus;
            }
        }

        private bool GetTeleportInput(InputDevice device, VRControllerModel controllerType)
        {
            bool enableTeleport;
            device.TryGetFeatureValue(CommonUsages.primary2DAxis, out _teleportAxisInput);
        
            if (controllerType == VRControllerModel.Vive)
            {
                device.TryGetFeatureValue(CommonUsages.primary2DAxisClick, out bool axisClick);
                enableTeleport = _teleportAxisInput.y > teleportActivationThresholdVive && axisClick;
            }
            else
            {
                enableTeleport = _teleportAxisInput.y > teleportActivationThresholdOculus;
            }

            return enableTeleport;
        }
        
        public void TeleportActiveCheck(bool hasInput)
        {
            if (hasInput && !_isTeleportActive)
            {
                _isTeleportActive = true;
                OnTeleportEnabled?.Invoke();
            }
            else if (!hasInput && _isTeleportActive)
            {
                _isTeleportActive = false;
                OnTeleportDisabled?.Invoke();
            }
        }
    }
}
