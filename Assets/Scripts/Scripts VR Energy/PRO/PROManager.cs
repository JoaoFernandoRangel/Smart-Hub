using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using VREnergy;
using VREnergy.Extensions;
using VREnergy.PRO;
using VREnergy.PRO.Model;
using VREnergy.SceneBuilder;

public class PROManager : MonoBehaviour
{
    public static PROManager main;

    [SerializeField] private ProcedureBuilder procedureBuilder;
    
    private Procedure currentProcedure;
    private Stage currentStageProcedure;
    private bool notSequenceConstraint;

    private IProcedureService _procedureService;
    private List<IPROAsset> _procedureAssets = new List<IPROAsset>();
    private List<IPROAsset> _activeProcedureAssets = new List<IPROAsset>();
    private Dictionary<string, string> _aliasTable = new Dictionary<string, string>();   // Used to swap the name of objects

    private void Awake()
    {
        if (main == null)
        {
            main = this;
        }
        else
        {
            Destroy(this.gameObject);
        }

        //_procedureService = DependencyContainerOld.ProcedureService;
    }

    public void InitializeProcedure(int procedureId)
    {
        InitializeProcedure(procedureId, false);
    }
    
    public void InitializeProcedure(int procedureId, bool notSequenceConstraint)
    {
        procedureBuilder.ConstructProcedure(procedureId, () =>
        {
            currentProcedure = _procedureService.GetProcedure(procedureId);
            if (currentProcedure == null) return;

            this.notSequenceConstraint = notSequenceConstraint;
            currentStageProcedure = currentProcedure.GetStage(notSequenceConstraint);
            currentStageProcedure.Status = StepStatus.Doing;

            _procedureAssets = GameObjectHelpers.FindObjectsOfInterface<IPROAsset>().ToList();
            AddInteractionEventToPROAssets();
        
            // Forçar o Stage selecionar o primeiro passo
            NewAction("ProcedureStart", "ProcedureStart", "ProcedureStart");
        
            AssetActivation();
        
            Debug.Log("Procedure start.");
        });
    }

    private void FinishProcedure()
    {
        RemoveInteractionEventFromPROAssets();
        DeactivateActiveAssets();
        Debug.Log("Procedure finished.");
    }
    
    public void NewAction(string activator, string receptor, string interaction)
    {
        Debug.Log($"Activator: {activator} , Receptor: {receptor}, Interaction: {interaction}");
        NewAction(new PROAction(activator, receptor, interaction));
    }

    public void NewAction(PROAction action)
    {
        if (currentStageProcedure == null)
            return;
        
        // Procedure stops if it's done
        if (currentStageProcedure.Status == StepStatus.Done)
            return;

        // Applying alias to objects
        if (_aliasTable.ContainsKey(action.Activator))
            action.Activator = _aliasTable[action.Activator];
        if (_aliasTable.ContainsKey(action.Receptor))
            action.Receptor = _aliasTable[action.Receptor];
        
        // Send action to current stage
        int actionResult = currentStageProcedure.NewAction(action);
        bool wasValidAction = actionResult != -1;

        if (currentStageProcedure.Status == StepStatus.Done)
        {
            FinishProcedure();
            return;
        }
        
        if (wasValidAction)
        {
            AssetActivation();
        }
    }

    public Stage GetCurrentStage()
    {
        return currentStageProcedure.CurrentStage();;
    }

    private void AssetActivation()
    {
        List<string> assetsId = GetStageAssetsId(GetCurrentStage(), true);

        DeactivateActiveAssets();

        foreach (var assetId in assetsId)
        {
            IPROAsset asset = _procedureAssets.FirstOrDefault(a => a.UnityId == assetId);
            if (asset != null)
            {
                asset.EnableAsset();
                _activeProcedureAssets.Add(asset);
            }
        }
    }

    public void DeactivateActiveAssets()
    {
        _activeProcedureAssets.ForEach(x => x.DisableAsset());
        _activeProcedureAssets.Clear();
    }

    private List<string> GetStageAssetsId(Stage stage, bool forceAdd = false)
    {
        var assetsId = new List<string>();
        var stageAction = stage as StageAction;
        var stageSet = stage as StageSet;
        
        if (forceAdd || stage.Status == StepStatus.Doing)
        {
            if (stageAction != null)
            {
                if (stageAction.Action.Receptor == "Painel")
                {
                    assetsId.Add(stageAction.Action.Interaction.Split('-')[0]);
                }
                else if (stageAction.Action.Receptor == "CaixaPrimaria")
                {
                    assetsId.Add(stageAction.Action.Receptor);
                    assetsId.Add(stageAction.Action.Interaction.Split('-')[0]);
                }
                else
                {
                    assetsId.Add(stageAction.Action.Activator);
                    assetsId.Add(stageAction.Action.Receptor);
                }
            }
            else
            {
                bool allChild = (stageSet.selectReqID == -1) && (stageSet is StageParallel);
                foreach (var substage in stageSet.Requirements)
                {
                    assetsId.AddRange(GetStageAssetsId(substage, allChild));
                }
            }    
        }

        return assetsId;
    }

    public void AddInteractionEventToPROAssets()
    {
        foreach (var proAsset in _procedureAssets)
        {
            proAsset.OnAssetInteraction += NewAction;
        }
    }
    
    public void RemoveInteractionEventFromPROAssets()
    {
        foreach (var proAsset in _procedureAssets)
        {
            proAsset.OnAssetInteraction -= NewAction;
        }
    }

    #region Alias
    public void AddAlias(string original, string alias)
    {
        _aliasTable.Add(original, alias);
    }
    public void RemoveAlias(string name, bool isOriginal = true)
    {
        try
        {
            if (isOriginal)
            {
                _aliasTable.Remove(name);
            }
            else
            {
                foreach (var key in _aliasTable.Keys)
                {
                    if (_aliasTable[key] == name)
                    {
                        _aliasTable.Remove(key);
                        break;
                    }
                }
            }
        }
        catch
        { }
    }
    #endregion
}
