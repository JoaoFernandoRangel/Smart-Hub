using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VREnergy.PRO;

public class PainelPlugKey : MonoBehaviour, IKey
{
    public LockType tipoChave;

    private bool _hover = false;
    private XRBaseInteractable interactable;
    private bool _lockedOnSocket = true;
    private XRBaseInteractor currentInteractor;

    public bool PodeRetirar { get; internal set; }
    public bool IsLockedOnSocket { get; set; }

    public event Action<bool> OnKeyActivation;
    public event Action OnConnect;
    public event Action OnDisconnect;
    
    private readonly int animParamAnimationIDHash = Animator.StringToHash("AnimationID");
    private readonly int animClosingValue = 0;
    private readonly int animOpeningValue = 1;
    
    private Animator _animator;

    private States _plugState = States.Ligar; // TODO: Fazer uma inicialização para saber qual o estado inicial do plug.
    public States PlugState
    {
        get => _plugState;
        set => _plugState = value;
    }

    #region MONOBEHAVIOUR

    private void Start()
    {
        interactable = gameObject.GetComponent<XRGrabInteractable>();
        interactable.hoverEntered.AddListener(OnHoverEnter);
        interactable.hoverExited.AddListener(OnHoverExit);

        _animator = GetComponentInChildren<Animator>();
    }
    
    #endregion

    private void OnHoverEnter(HoverEnterEventArgs interactor)
    {
        currentInteractor = (XRBaseInteractor)interactor.interactorObject;
        _hover = true;

        if (_lockedOnSocket && currentInteractor)
            currentInteractor.allowSelect = false;

        //Debug.Log(_lockedOnSocket);
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
        if (_hover)
        {
            Activate();
        }
    }

    public void Activate()
    {
        _lockedOnSocket = !_lockedOnSocket;
        if (_lockedOnSocket && currentInteractor)
        {
            _animator.SetInteger(animParamAnimationIDHash, animClosingValue);
            currentInteractor.allowSelect = false;
        }
        else if (!_lockedOnSocket && currentInteractor)
        {
            _animator.SetInteger(animParamAnimationIDHash, animOpeningValue);
            currentInteractor.allowSelect = true;
        }
        
        FindObjectOfType<ProcedureStageHandler>()?.NewAction(
            activator: "Operador",
            receptor: GetComponent<IPROAsset>().UnityId,
            interaction: $"{_plugState}-{(_lockedOnSocket ? States.Travar : States.Destravar)}"
        );

        OnKeyActivation?.Invoke(_lockedOnSocket);

        Debug.Log($"KeyLocked: {_lockedOnSocket}, ATIVOU!!!!");
    }

    public bool isLockedOnSocket()
    {
        return _lockedOnSocket;
    }

    public void Connect(XRSocketInteractor connectedSocket)
    {
        TriggerListener.OnTriggetButtonPress += OnTriggetButtonPress;
        OnConnect?.Invoke();
    }
    
    public void Disconnect()
    {
        TriggerListener.OnTriggetButtonPress -= OnTriggetButtonPress;
        OnDisconnect?.Invoke();
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
