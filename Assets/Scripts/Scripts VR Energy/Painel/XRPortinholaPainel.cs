using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XRPortinholaPainel : MonoBehaviour, IPanelConfigurableComponent, ILock
{
    public event Action onLock;
    public event Action onUnlock;
    public event Action onKeyIn;
    public event Action onKeyOut;

    public bool _locked = true;

    public bool IsKeyPlaced { get; private set; }

    [Header("Sockets")]
    public XRSocketInteractor portinholaSocket;
    public XRSocketInteractor dijuntorExtratorSocket;

    [Header("This Socket Key")]
    public ToolChavePainel MyKey;
    
    [Header("Dependencies")]
    public DOLocalRotation doLocalRotation;

    private void Awake()
    {
        if (MyKey == null)
        {
            MyKey = FindObjectOfType<ToolChavePainel>();
        }
    }

    private void Start()
    {
        //dijuntorExtratorSocket.onSelectEntered.AddListener();
        dijuntorExtratorSocket.gameObject.SetActive(false);
        //MyKey.OnKeyActivation += MyKey_OnActivationLock;
        MyKey.OnKeyActivation += RotateSocket;
        doLocalRotation.onStart.AddListener(OnStartRotation);
        doLocalRotation.onComplete.AddListener(OnCompleteRotation);
    }

    private void MyKey_OnActivationLock(bool obj)
    {
        _locked = !_locked;
        dijuntorExtratorSocket.gameObject.SetActive(!_locked);
        Debug.Log($"LOCKED = {_locked}");
    }

    public void RotateSocket(bool _)
    {
        if (doLocalRotation.IsRotating) return;
        if (portinholaSocket.GetOldestInteractableSelected() == null) return;
        if (portinholaSocket.GetOldestInteractableSelected().transform.GetComponent<ToolChavePainel>() != MyKey) return;
        if (dijuntorExtratorSocket.GetComponentInParent<ILock>().IsKeyPlaced) return;

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

    public void SetInitialState(string json)
    {
        throw new System.NotImplementedException();
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
        //desabilita o socket para a chavel de extração
        dijuntorExtratorSocket.gameObject.SetActive(false);
    }
    
    public void Unlock()
    {
        _locked = false;
        onUnlock?.Invoke();
        dijuntorExtratorSocket.gameObject.SetActive(true);
        //habilita o socket para a chavel de extração
    }
}
