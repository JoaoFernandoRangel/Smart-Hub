using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolChaveBiela : MonoBehaviour, IKey
{
    [SerializeField] private LockType tipoChave = LockType.Socket_Chave_Biela;

    public event Action<bool> OnKeyActivation;
    public event Action OnConnect;
    public event Action OnDisconnect;

    public bool IsLockedOnSocket
    {
        get => _lockedOnSocket;
        set => _lockedOnSocket = value;
    }

    private XRBaseInteractable _interactable;
    private XRBaseInteractor _currentInteractor;
    private XRSocketInteractor _connectedSocket;
    private bool _lockedOnSocket;
    private bool _hover;

    #region MONOBEHAVIOUR

    private void Awake()
    {
        _interactable = GetComponent<XRBaseInteractable>();
    }

    private void Start()
    {
        _interactable.hoverEntered.AddListener(OnHoverEnter);
        _interactable.hoverExited.AddListener(OnHoverExit);
    }

    #endregion

    private void OnHoverEnter(HoverEnterEventArgs interactor)
    {
        _currentInteractor = (XRBaseInteractor)interactor.interactorObject;
        _hover = true;

        if (_lockedOnSocket && _currentInteractor)
            _currentInteractor.allowSelect = false;
    }
    private void OnHoverExit(HoverExitEventArgs interactor)
    {
        _hover = false;
        if (_currentInteractor)
            _currentInteractor.allowSelect = true;
        _currentInteractor = null;
    }

    public LockType GetLockType()
    {
        return tipoChave;
    }

    public bool isLockedOnSocket()
    {
        return _lockedOnSocket;
    }

    public void Connect(XRSocketInteractor socket)
    {
        _connectedSocket = socket;
        TriggerListener.OnTriggetButtonPress += OnTriggetButtonPress;
    }

    private void OnTriggetButtonPress(string obj)
    {
        if (_hover && _connectedSocket)
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

    public void Disconnect()
    {
        _connectedSocket = null;
        TriggerListener.OnTriggetButtonPress += OnTriggetButtonPress;
    }

    public bool CanConnect()
    {
        return true;
    }
}
