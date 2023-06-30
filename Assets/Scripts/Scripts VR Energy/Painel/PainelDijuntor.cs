using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PainelDijuntor : MonoBehaviour, ILock
{
    public event Action onLock;
    public event Action onUnlock;
    public event Action onKeyIn;
    public event Action onKeyOut;

    public LockType inKeyType;
    public ToolChaveManobra MyKey;

    private bool _open = false;                 //Saber se esta aberta ou fechada
    private bool _connected = false;            //tem algo conectada a ela
    private bool _locked = false;               //a caixa esta trancada
    
    public bool IsKeyPlaced { get; private set; }

    private void Start()
    {
        MyKey = FindObjectOfType<ToolChaveManobra>();
        //MyKey.OnKeyActivation += MyKey_OnActivationLock;
        MyKey.OnConnect += MyKey_OnConnect;
        MyKey.OnDisconnect += MyKey_OnDisconnect;
    }

    private void MyKey_OnActivationLock(bool newStatus)
    {
        _locked = newStatus;
        if (newStatus == false)
        {//Dijuntor pula para frente
//            gameObject.transform -.87f;
 
            transform.Translate(0, 0, .07f);
        }
        else
        {//Dijuntor trava atras
            transform.Translate(0, 0, -.07f);
        }
    }

    private void MyKey_OnConnect()
    {
        _connected = true;
    }
    
    private void MyKey_OnDisconnect()
    {
        _connected = false;
    }

    public bool isOpen()
    {
        throw new NotImplementedException();
    }

    public void Lock()
    {
        throw new NotImplementedException();
    }

    public bool PlaceKey()
    {
        throw new NotImplementedException();
    }

    public bool RemoveKey()
    {
        throw new NotImplementedException();
    }

    public void Unlock()
    {
        throw new NotImplementedException();
    }
}
