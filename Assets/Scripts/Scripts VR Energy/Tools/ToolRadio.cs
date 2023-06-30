using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;
using VREnergy.PRO;

public class ToolRadio : MonoBehaviour
{
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private ClipForState[] clipsForStates = new []
    {
        new ClipForState { state = States.ProsseguirProcedimento },
        new ClipForState { state = States.EquipamentoDisponivel },
        new ClipForState { state = States.FinalizarProcedimento },
        new ClipForState { state = States.NaoUsar },
    };

    private Dictionary<States, AudioClip> clipsForStatesDictionary = new Dictionary<States, AudioClip>();
    private XRGrabInteractable grabInteractable;
    private States currentState;
    private ProcedureStageHandler _procedureStageHandler;
    private IPROAsset _proAsset;

    #region MONOBEHAVIOUR

    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        _procedureStageHandler = DependencyContainer.Instance.Get<ProcedureStageHandler>();
        _proAsset = GetComponent<IPROAsset>();
    }

    private void OnEnable()
    {
        CacheClipForState();
        grabInteractable.activated.AddListener(OnActivateListener);
    }

    private void OnDisable()
    {
        grabInteractable.activated.RemoveListener(OnActivateListener);
        ClearCacheClipForState();
    }

    #endregion

    private void OnActivateListener(ActivateEventArgs interactor)
    {
        try
        {
            StageAction currentActionPRO = (_procedureStageHandler.GetCurrentStage() as StageAction);         
            UpdateState((States)Enum.Parse(typeof(States), currentActionPRO.Action.Interaction));
        }
        catch
        {
            Debug.Log("Error");
            UpdateState(States.NaoUsar);
        }
    }
    public void UpdateState(States newState)
    {
        currentState = newState;
        _procedureStageHandler.NewAction(new PROAction
        {
            Activator = "Operador",
            Receptor = _proAsset.UnityId,
            Interaction = currentState.ToString()
        });

        switch (newState)
        {
            case States.ProsseguirProcedimento:
                audioSource.clip = clipsForStatesDictionary[States.ProsseguirProcedimento];
                break;
            case States.EquipamentoDisponivel:
                audioSource.clip = clipsForStatesDictionary[States.EquipamentoDisponivel];
                break;
            case States.FinalizarProcedimento:
                audioSource.clip = clipsForStatesDictionary[States.FinalizarProcedimento];
                break;
            case States.NaoUsar:
                audioSource.clip = clipsForStatesDictionary[States.NaoUsar];
                break;
        }
        audioSource.Play();
    }

    private void CacheClipForState()
    {
        foreach (var clipForState in clipsForStates)
        {
            clipsForStatesDictionary.Add(clipForState.state, clipForState.clip);
        }
    }
    
    private void ClearCacheClipForState()
    {
        clipsForStatesDictionary.Clear();
    }
    
    [Serializable]
    public struct ClipForState
    {
        public AudioClip clip;
        public States state;
    }
    
#if UNITY_EDITOR
    private void Reset()
    {
        audioSource = GetComponent<AudioSource>();
        clipsForStates = new []
        {
            new ClipForState { state = States.ProsseguirProcedimento },
            new ClipForState { state = States.EquipamentoDisponivel },
            new ClipForState { state = States.FinalizarProcedimento },
            new ClipForState { state = States.NaoUsar },
        };
    }
#endif
}
