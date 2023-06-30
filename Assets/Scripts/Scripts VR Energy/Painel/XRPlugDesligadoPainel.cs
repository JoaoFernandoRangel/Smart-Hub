using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VREnergy.PRO;

namespace VREnergy.Painel
{
    public class XRPlugDesligadoPainel : MonoBehaviour, ILock
    {
        [Header("Sockets")]
        public XRSocketInteractor plugSocket;
        
        public event Action onLock;
        public event Action onUnlock;
        public event Action onKeyIn;
        public event Action onKeyOut;
        public bool IsKeyPlaced { get; private set; }
        
        private bool _locked;

        private void Awake()
        {
            plugSocket.selectEntered.AddListener(OnPlugConnected);
        }

        private void OnPlugConnected(SelectEnterEventArgs interactable)
        {
            if (interactable.interactableObject.transform.TryGetComponent(out PainelPlugKey plug))
            {
                plug.PlugState = States.Desligar;
                FindObjectOfType<ProcedureStageHandler>().NewAction(
                    activator: interactable.interactableObject.transform.GetComponent<IPROAsset>().UnityId,
                    receptor: GetComponent<IPROAsset>().UnityId,
                    interaction: States.Desligar.ToString()
                );
            }
        }

        public bool isOpen()
        {
            return !_locked;
        }

        public bool PlaceKey()
        {
            IsKeyPlaced = true;
            return true;
        }

        public bool RemoveKey()
        {
            IsKeyPlaced = false;
            return true;
        }

        public void Lock()
        {
            _locked = true;
            onLock?.Invoke();
        }

        public void Unlock()
        {
            _locked = false;
            onUnlock?.Invoke();
        }
    }
}