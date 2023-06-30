using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRMacanetaPortaMeioPainel : MonoBehaviour, ILock
{
    [Header("Sockets")]
    public XRSocketInteractor portaMeioSocket;

    [Header("This Socket Key")]
    public ToolChavePainel MyKey;
    
    [Header("Dependencies")]
    public DOLocalRotation doLocalRotation;

    public event Action onLock;
    public event Action onUnlock;
    public event Action onKeyIn;
    public event Action onKeyOut;
    
    public bool IsKeyPlaced { get; private set; }
    
    private PortaPainel _portaPainel;
    private bool _locked = true;

    private void Awake()
    {
        doLocalRotation = GetComponent<DOLocalRotation>();
        _portaPainel = GetComponentInParent<PortaPainel>();
        if (MyKey == null)
        {
            MyKey = FindObjectOfType<ToolChavePainel>();
        }
    }

    private void Start()
    {
        MyKey.OnKeyActivation += RotateSocket;
        doLocalRotation.onStart.AddListener(OnStartRotation);
        doLocalRotation.onComplete.AddListener(OnCompleteRotation);
    }
    
    private void RotateSocket(bool _)
    {
        if (doLocalRotation.IsRotating) return;
        if (portaMeioSocket.GetOldestInteractableSelected() == null) return;
        if (portaMeioSocket.GetOldestInteractableSelected().transform.GetComponent<ToolChavePainel>() != MyKey) return;
        if (!_portaPainel.IsDoorClosed) return;
        
        doLocalRotation.PerformRotation();
    }
    
    public void OnStartRotation()
    {
        MyKey.GetComponent<IKey>().IsLockedOnSocket = true;
    }
    
    public void OnCompleteRotation()
    {
        if (doLocalRotation.IsInInitialState)
        {
            MyKey.GetComponent<IKey>().IsLockedOnSocket = false;
            Lock();
        }
        else
        {
            Unlock();
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
        _portaPainel.LockDoor();
    }

    public void Unlock()
    {
        _locked = false;
        onUnlock?.Invoke();
        _portaPainel.UnlockDoor();
    }
}
