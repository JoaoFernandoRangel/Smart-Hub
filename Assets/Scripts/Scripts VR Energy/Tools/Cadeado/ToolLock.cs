using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolLock : MonoBehaviour, ILock, IKey
{
    public LockType KeyType;
    public PainelSocket saida;
    public bool Trancado;
    [HideInInspector] public bool ConectouSaida;
    //public LockType outKeyType;

    //public SocketKey socket;    //chave
    public ToolKey MyKey;

    private Animator Anim;

    private bool _open = false;
    private bool _connected = false;
    private bool _canConnect = false;
    private XRBaseInteractable interactable;
    private bool _lockedOnSocket = false;
    private XRBaseInteractor currentInteractor;

    public event Action<bool> OnKeyActivation;
    public event Action onLock;
    public event Action onUnlock;
    public event Action OnConnect;
    public event Action OnDisconnect;
    public event Action onKeyIn;
    public event Action onKeyOut;
    
    public bool IsKeyPlaced { get; private set; }

    private void Start()
    {
        interactable = gameObject.GetComponent<XRGrabInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);

        MyKey.OnKeyActivation += MyKey_OnActivationLock;

        Anim = GetComponentInChildren<Animator>();
    }

    private void MyKey_OnActivationLock(bool obj)
    {
        _open = !_open;

        if (_open)
        {
            Unlock();
            _canConnect = true;
        }
        else
        {
            Lock();
            if(!_connected)
                _canConnect = false;
        }
    }

    private void OnHoverEnter(HoverEnterEventArgs interactor)
    {
        currentInteractor = (XRBaseInteractor)interactor.interactorObject;

        if (_lockedOnSocket && currentInteractor)
            currentInteractor.allowSelect = false;
    }

    private void OnHoverExit(HoverExitEventArgs interactor)
    {
        if (_lockedOnSocket && currentInteractor)
            currentInteractor.allowSelect = true;
    }    

    public void Lock()
    {
        if (Anim)
            Anim.SetInteger("AnimationID", 1);

        if (_connected)
        {
            _lockedOnSocket = true;
            Debug.LogError("LOCKED");
        }

        Trancado = true;
        onLock?.Invoke();
    }

    public void Unlock()
    {
        if (Anim)
            Anim.SetInteger("AnimationID", 0);

        _lockedOnSocket = false;

        Trancado = false;
        onUnlock?.Invoke();
    }
 
    public bool isOpen()
    {
        return _open;
    }

    public LockType GetLockType()
    {
        return KeyType;
    }

    public bool isLockedOnSocket()
    {
        return _lockedOnSocket;
    }

    public bool IsLockedOnSocket { get; set; }

    public void Connect(XRSocketInteractor connectedSocket)
    {
        OnConnect?.Invoke();
        _connected = true;
    }

    public void Disconnect()
    {
        OnDisconnect?.Invoke();
        _connected = false;
    }

    public bool CanConnect()
    {
        return _canConnect;
    }

    public bool PlaceKey()
    {
        IsKeyPlaced = true;
        return IsKeyPlaced;
    }

    public bool RemoveKey()
    {
        IsKeyPlaced = true;
        return IsKeyPlaced;
    }
}