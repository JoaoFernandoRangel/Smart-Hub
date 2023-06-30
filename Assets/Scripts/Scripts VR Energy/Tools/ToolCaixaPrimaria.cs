using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VREnergy.PRO;

public class ToolCaixaPrimaria : MonoBehaviour, ILock
{
    public LockType inKeyType;
    public ToolLock MyKey;

    [SerializeField] private PROAsset asset;

    private Animator animator;
    private bool _open = false;                 //Saber se esta aberta ou fechada
    private bool _connected = false;            //tem algo conectada a ela
    private bool _locked = false;               //a caixa esta trancada
    private XRBaseInteractable interactable;
    private XRBaseInteractor currentInteractor;

    public event Action<bool> OnKeyActivation;
    public event Action onLock;
    public event Action onUnlock;
    public event Action onKeyIn;
    public event Action onKeyOut;
    
    public bool IsKeyPlaced { get; private set; }

    private readonly int animParamAnimationIDHash = Animator.StringToHash("AnimationID");
    private readonly int animClosingValue = 0;
    private readonly int animOpeningValue = 1;

    private void Awake()
    {
        interactable = gameObject.GetComponent<XRActivateOnSelectWithTriggerInteractable>();
        animator = GetComponentInChildren<Animator>();
        
        if (asset == null)
        {
            asset = GetComponent<PROAsset>();
        }
    }

    private void Start()
    {
        interactable.selectEntered.AddListener(OnActiveSelect);
        MyKey.OnKeyActivation += MyKey_OnActivationLock;
        MyKey.OnConnect += MyKey_OnConnect;
        MyKey.OnDisconnect += MyKey_OnDisconnect;
    }

    private void MyKey_OnConnect()
    {
        _connected = true;
    }
    
    private void MyKey_OnDisconnect()
    {
        _connected = false;
    }

    private void OnActiveSelect(SelectEnterEventArgs interactor)
    {
        if (!_connected)
        {
            Ativar();
        }
    }

    public void Ativar()
    {
        _open = !_open;
        if (_open)
        {
            if (animator)
                animator.SetInteger(animParamAnimationIDHash, animOpeningValue);
            
            ProcedureStageHandler.Instance.NewAction(
                activator: "Operador",
                receptor: GetComponent<PROAsset>().UnityId,
                interaction: States.Abrir.ToString()
            );
        }
        else
        {
            if (animator)
                animator.SetInteger(animParamAnimationIDHash, animClosingValue);
            
            ProcedureStageHandler.Instance.NewAction(
                activator: "Operador",
                receptor: GetComponent<PROAsset>().UnityId,
                interaction: States.Fechar.ToString()
            );
        }
    }

    private void MyKey_OnActivationLock(bool obj)
    {
        _locked = obj;
    }

    public bool isOpen()
    {
        return _open;
    }

    public void Lock()
    {
        onLock?.Invoke();
    }

    public void Unlock()
    {
        onUnlock?.Invoke();
    }

    public bool PlaceKey()
    {
        IsKeyPlaced = true;
        return IsKeyPlaced;
    }

    public bool RemoveKey()
    {
        IsKeyPlaced = false;
        return IsKeyPlaced;
    }
}
