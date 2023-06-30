using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolKey : MonoBehaviour, IKey
{
    public LockType tipoChave;    

    private bool _hover = false;
    private XRBaseInteractable interactable;
    private bool _lockedOnSocket = false;
    private XRBaseInteractor currentInteractor;

    public bool PodeRetirar { get; internal set; }

    public event Action<bool> OnKeyActivation;
    public event Action OnConnect;
    public event Action OnDisconnect;

    private void Awake()
    {
        interactable = gameObject.GetComponent<XRGrabInteractable>();
    }

    private void OnEnable()
    {
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
    }

    private void OnDisable()
    {
        interactable.hoverEntered.RemoveListener(OnHoverEnter);
        interactable.hoverExited.RemoveListener(OnHoverExit);
    }

    private void OnHoverEnter(HoverEnterEventArgs interactor)
    {
        currentInteractor = (XRBaseInteractor)interactor.interactorObject;
        _hover = true;

        if (_lockedOnSocket && currentInteractor)
            currentInteractor.allowSelect = false;
        
        Debug.Log(_lockedOnSocket);
    }
    
    private void OnHoverExit(HoverExitEventArgs interactor)
    {
        _hover = false;        
        if(currentInteractor)
            currentInteractor.allowSelect = true;
        currentInteractor = null;
        Debug.Log("Exited");
    }

    private void OnTriggetButtonPress(string obj)
    {
        if (_hover)
        {
            Activate();            
        }        
    }

    public void Activate()
    {
        _lockedOnSocket = !_lockedOnSocket;
        if (_lockedOnSocket && currentInteractor)
            currentInteractor.allowSelect = false;
        else if(!_lockedOnSocket && currentInteractor)
            currentInteractor.allowSelect = true;

        OnKeyActivation?.Invoke(_lockedOnSocket);

        Debug.Log($"KeyLocked: {_lockedOnSocket}");
    }
    
    public bool isLockedOnSocket()
    {
        return _lockedOnSocket;
    }

    public bool IsLockedOnSocket { get; set; }

    public void Connect(XRSocketInteractor connectedSocket)
    {
        TriggerListener.OnTriggetButtonPress += OnTriggetButtonPress;
    }
    
    public void Disconnect()
    {
        TriggerListener.OnTriggetButtonPress -= OnTriggetButtonPress;
    }

    public LockType GetLockType()
    {
        return tipoChave;
    }
    
    public bool CanConnect()
    {
        return true;
    }
}
