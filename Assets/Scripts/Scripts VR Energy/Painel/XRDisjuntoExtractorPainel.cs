using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRDisjuntoExtractorPainel : MonoBehaviour, IPanelConfigurableComponent, ILock
{
    public bool _locked = true;

    public event Action onLock;
    public event Action onUnlock;
    public event Action onKeyIn;
    public event Action onKeyOut;
    
    public bool IsKeyPlaced { get; private set; }
    
    [Header("Sockets")]
    public XRSocketInteractor dijuntorExtratorSocket;
    
    [Header("This Socket Key")]
    public ToolChaveManobra MyKey;
    
    [Header("Dependencies")]
    public DOLocalRotation doLocalRotation;

    private void Awake()
    {
        doLocalRotation = GetComponent<DOLocalRotation>();
    }

    private void Start()
    {
        MyKey.OnKeyActivation += RotateSocket;
        dijuntorExtratorSocket.selectEntered.AddListener((xr) => PlaceKey());
        dijuntorExtratorSocket.selectExited.AddListener((xr) => RemoveKey());
        doLocalRotation.onStart.AddListener(OnStartRotation);
        doLocalRotation.onComplete.AddListener(OnCompleteRotation);
    }

    private void RotateSocket(bool _)
    {
        if (doLocalRotation.IsRotating) return;
        if (dijuntorExtratorSocket.GetOldestInteractableSelected() == null) return;
        if (dijuntorExtratorSocket.GetOldestInteractableSelected().transform.GetComponent<ToolChaveManobra>() != MyKey) return;
        
        doLocalRotation.PerformRotation();
    }
    
    public void OnStartRotation()
    {
        MyKey.GetComponent<IKey>().IsLockedOnSocket = true;
    }
    
    public void OnCompleteRotation()
    {
        MyKey.GetComponent<IKey>().IsLockedOnSocket = false;
        if (doLocalRotation.IsInInitialState)
        {
            Unlock();
        }
        else
        {
            Lock();
        }
    }

    public void SetInitialState(string json)
    {
        throw new NotImplementedException();
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
    }

    public void Unlock()
    {
        _locked = false;
    }
}
