using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRSeccionadoraPainel : MonoBehaviour, ILock
{
    [Header("Sockets")]
    public XRSocketInteractor seccionadoraSocket;
    
    [Header("This Socket Key")]
    public ToolChaveBiela MyKey;
    
    [Header("Dependencies")]
    public DOLocalRotation doLocalRotation;

    public event Action onLock;
    public event Action onUnlock;
    public event Action onKeyIn;
    public event Action onKeyOut;

    public bool IsKeyPlaced { get; private set; }
    
    private bool _locked;

    #region MONOBEHAVIOUR

    private void Awake()
    {
        doLocalRotation = GetComponent<DOLocalRotation>();
        if (MyKey == null)
        {
            MyKey = FindObjectOfType<ToolChaveBiela>();
        }
    }

    private void Start()
    {
        MyKey.OnKeyActivation += RotateSocket;
        seccionadoraSocket.selectEntered.AddListener((xr) => PlaceKey());
        seccionadoraSocket.selectExited.AddListener((xr) => RemoveKey());
        doLocalRotation.onStart.AddListener(OnStartRotation);
        doLocalRotation.onComplete.AddListener(OnCompleteRotation);
    }

    #endregion
    
    private void RotateSocket(bool obj)
    {
        if (doLocalRotation.IsRotating) return;
        if (seccionadoraSocket.GetOldestInteractableSelected() == null) return;
        if (seccionadoraSocket.GetOldestInteractableSelected().transform.GetComponent<ToolChaveBiela>() != MyKey) return;
        
        doLocalRotation.PerformRotation();
    }

    private void OnStartRotation()
    {
        MyKey.GetComponent<IKey>().IsLockedOnSocket = true;
    }
    
    private void OnCompleteRotation()
    {
        MyKey.GetComponent<IKey>().IsLockedOnSocket = false;
        if (doLocalRotation.IsInInitialState)
        {
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
        onKeyIn?.Invoke();
        return true;
    }

    public bool RemoveKey()
    {
        IsKeyPlaced = false;
        onKeyOut?.Invoke();
        return true;
    }

    public void Lock()
    {
        //_locked = true;
        onLock?.Invoke();
    }

    public void Unlock()
    {
        //_locked = false;
        onUnlock?.Invoke();
    }
}
