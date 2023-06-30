using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolChavePainel : MonoBehaviour, IKey
{
    public LockType tipoChave;
    XRSocketInteractor connectedSocket;
    private bool _hover = false;
    private XRBaseInteractable interactable;
    private bool _lockedOnSocket = false;
    private XRBaseInteractor currentInteractor;

    public bool IsLockedOnSocket
    {
        get => _lockedOnSocket;
        set
        {
            _lockedOnSocket = value;
            if (_lockedOnSocket && currentInteractor)
                currentInteractor.allowSelect = false;
        }
    }

    public event Action<bool> OnKeyActivation;
    public event Action OnConnect;
    public event Action OnDisconnect;

    private void Start()
    {
        interactable = gameObject.GetComponent<XRGrabInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);
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
        if (currentInteractor)
            currentInteractor.allowSelect = true;
        currentInteractor = null;
        //Debug.Log("Exited");
    }

    private void OnTriggetButtonPress(string obj)
    {
        if (_hover && connectedSocket)
        {
            Activate();
        }
    }

    public void Activate()
    {
        //_lockedOnSocket = !_lockedOnSocket;
        /*if (_lockedOnSocket && currentInteractor)
            currentInteractor.allowSelect = false;
        else if (!_lockedOnSocket && currentInteractor)
            currentInteractor.allowSelect = true;*/

        OnKeyActivation?.Invoke(_lockedOnSocket);

        Debug.Log($"KeyLocked: {_lockedOnSocket}");
    }

    public bool isLockedOnSocket()
    {
        return _lockedOnSocket;
    }

    public void Connect(XRSocketInteractor socket)
    {
        connectedSocket = socket;
        TriggerListener.OnTriggetButtonPress += OnTriggetButtonPress;
    }
    
    public void Disconnect()
    {
        connectedSocket = null;
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
