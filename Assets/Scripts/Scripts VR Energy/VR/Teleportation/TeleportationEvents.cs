using System;
using UnityEngine;
using UnityEngine.Events;

namespace VREnergy.VR.Teleportation
{
    public class TeleportationEvents : MonoBehaviour
    {
        [Header("MonoBehaviour Events")]
        [SerializeField] private UnityEvent onEnable = new UnityEvent();
        
        [Header("Teleportation Events")]
        [SerializeField] private UnityEvent onTeleportInputEnabled = new UnityEvent();
        [SerializeField] private UnityEvent onTeleportInputDisabled = new UnityEvent();

        private void OnEnable()
        {
            XRMasterController.Instance.OnTeleportEnabled += OnTeleportInputEnabled;
            XRMasterController.Instance.OnTeleportDisabled += OnTeleportInputDisabled;
            onEnable?.Invoke();
        }

        private void OnDisable()
        {
            XRMasterController.Instance.OnTeleportEnabled -= OnTeleportInputEnabled;
            XRMasterController.Instance.OnTeleportDisabled -= OnTeleportInputDisabled; 
        }

        private void OnTeleportInputEnabled() => onTeleportInputEnabled?.Invoke();

        private void OnTeleportInputDisabled() => onTeleportInputDisabled?.Invoke();
    }
}
