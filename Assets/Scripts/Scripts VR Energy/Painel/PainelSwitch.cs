using System;
using System.Text;
using DG.Tweening;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VREnergy.PRO;

public class PainelSwitch : MonoBehaviour
{
    public int[] LuzesLigado;
    public int[] LuzesDesligado;
    [SerializeField] private Transform switchModel;
    [SerializeField] private SwitchState initialSwitchState;
    
    [Header("Switch Animation")] 
    [SerializeField] [Range(0f, 1f)] private float switchRotationDuration = .5f;
    
    private bool _hover;
    private States _currentState = States.Desligar;
    private AudioSource _audioSource;
    private XRBaseInteractable _interactable;

    #region MONOBEHAVIOUR

    private void Awake()
    {
        Init();
    }

    private void Start()
    {
        _audioSource = GetComponent<AudioSource>();
        _interactable = GetComponent<XRBaseInteractable>();
        _interactable.hoverEntered.AddListener(OnHoverEnter);
        _interactable.hoverExited.AddListener(OnHoverExit);
    }

    private void OnEnable()
    {
        TriggerListener.OnTriggetButtonPress += OnTriggetButtonPress;
    }

    private void OnDisable()
    {
        TriggerListener.OnTriggetButtonPress -= OnTriggetButtonPress;
    }

    #endregion

    private void OnHoverEnter(HoverEnterEventArgs interactor)
    {
        _hover = true;
    }
    
    private void OnHoverExit(HoverExitEventArgs interactor)
    {
        _hover = false;
    }
    
    private void OnTriggetButtonPress(string obj)
    {
        if (_hover)
        {
            Activate();
        }
    }

    private void Activate()
    {
        ChangeSwitch();
    }
    
    public void ChangeSwitch()
    {
        if (_currentState == States.Desligar)
        {
            _currentState = States.Ligar;
            switchModel.DOLocalRotate(SwitchRotations.switchOnEndRotation, switchRotationDuration);
            NotifyInteraction();
        }
        else if (_currentState == States.Ligar)
        {
            _currentState = States.Desligar;
            switchModel.DOLocalRotate(SwitchRotations.switchOffEndRotation, switchRotationDuration);
            NotifyInteraction();
        }

        _audioSource.Play();
    }

    private void NotifyInteraction()
    {
        FindObjectOfType<ProcedureStageHandler>()?.NewAction(
            activator: "Operador", 
            receptor: GetComponent<IPROAsset>().UnityId, 
            interaction: _currentState.ToString()
        );
    }

    private void Init()
    {
        switch (initialSwitchState)
        {
            case SwitchState.On:
                _currentState = States.Ligar;
                switchModel.localRotation = Quaternion.Euler(SwitchRotations.switchOnEndRotation);
                break;
            case SwitchState.Off:
                _currentState = States.Desligar;
                switchModel.localRotation = Quaternion.Euler(SwitchRotations.switchOffEndRotation);
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    struct SwitchRotations
    {
        public static readonly Vector3 switchOnEndRotation = new Vector3(0, -25, 0);
        public static readonly Vector3 switchOffEndRotation = new Vector3(0, 25, 0);
    }

    private enum SwitchState
    {
        Off,
        On
    }

#if UNITY_EDITOR
    private void OnValidate()
    {
        // Atualiza a rotação do switch quando o valor for mudado no inspector
        if (initialSwitchState == SwitchState.On)
        {
            switchModel.localEulerAngles = SwitchRotations.switchOnEndRotation;
        }
        else
        {
            switchModel.localEulerAngles = SwitchRotations.switchOffEndRotation;
        }
    }
#endif
    
}
