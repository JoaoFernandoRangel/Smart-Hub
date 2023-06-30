using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using VREnergy.Extensions;
using VREnergy.PRO.Model;
using Action = System.Action;
using Scene = UnityEngine.SceneManagement.Scene;

namespace VREnergy.PRO
{
    /// <summary>
    /// Classe responsável por controlar os passos e a ativação dos assets do procedimento.
    /// </summary>
    public class ProcedureStageHandler : MonoBehaviour
    {
        private static ProcedureStageHandler s_instance;

        public static ProcedureStageHandler Instance
        {
            get
            {
                if (s_instance == null)
                {
                    s_instance = FindObjectOfType<ProcedureStageHandler>();

                    if (s_instance == null)
                    {
                        Debug.LogError($"{nameof(ProcedureStageHandler)} não foi encontrado na cena.");
                    }
                }

                return s_instance;
            }
        }

        private bool _notSequenceConstraint;
        private Procedure _currentProcedure;
        private Stage _currentStageProcedure;
        private IProcedureService _procedureService;
        private Dictionary<string, IPROAsset> _procedureAssets = new Dictionary<string, IPROAsset>();
        private List<IPROAsset> _activeProcedureAssets = new List<IPROAsset>();

        public event Action<Stage> OnStageChanged;
        public event Action OnProcedureStart;
        public event Action OnProcedureFinish;

        public Stage StageProcedure => _currentStageProcedure;

        #region MONOBEHAVIOUR

        private void Awake()
        {
            _procedureService = DependencyContainer.Instance.Get<IProcedureService>();
        }

        private void Start()
        {
            DEBUG();
            // InitializeStage(9);
        }

        private void OnEnable()
        {
            SceneManager.sceneLoaded += OnMenuLoadedClearCache;
        }

        private void OnDisable()
        {
            SceneManager.sceneLoaded -= OnMenuLoadedClearCache;
        }

        #endregion

        private void OnMenuLoadedClearCache(Scene scene, LoadSceneMode mode)
        {
            if (scene.name == "Menu")
            {
                ClearCache();
            }
        }

        public void InitializeStage(int procedureId, bool notSequenceConstraint = false)
        {
            _currentProcedure = _procedureService.GetProcedure(procedureId);
            if (_currentProcedure == null)
            {
                Debug.LogError($"Procedure com o id: {procedureId} não foi encontrado.");
                return;
            }

            // Obter todos os assets da cena
            _procedureAssets = GetSceneAssets();

            this._notSequenceConstraint = notSequenceConstraint;
            _currentStageProcedure = _currentProcedure.GetStage(notSequenceConstraint);
            _currentStageProcedure.Status = StepStatus.Doing;

            // Forçar o Stage selecionar o primeiro passo
            NewAction("ProcedureStart", "ProcedureStart", "ProcedureStart");

            OnStageChanged?.Invoke(GetCurrentStage());

            // Ativar assets
            AssetActivation();

            OnProcedureStart?.Invoke();

            Debug.Log("Procedure start.");
        }

        private Dictionary<string, IPROAsset> GetSceneAssets()
        {
            var procedureAssets = new Dictionary<string, IPROAsset>();
            List<IPROAsset> sceneAssets = GameObjectHelpers.FindObjectsOfInterface<IPROAsset>().ToList();

            foreach (IPROAsset asset in sceneAssets)
            {
                if (string.IsNullOrEmpty(asset.UnityId))
                {
                    continue;
                }

                if (!procedureAssets.ContainsKey(asset.UnityId))
                {
                    procedureAssets.Add(asset.UnityId, asset);
                    asset.OnAssetInteraction += NewAction;
                }
                else
                {
                    Debug.LogError($"Existe mais de um asset chamado {asset.UnityId} na cena.");
                }
            }

            return procedureAssets;
        }

        public void NewAction(string activator, string receptor, string interaction)
        {
            Debug.Log($"Activator: {activator}, Receptor: {receptor}, Interaction: {interaction}");
            NewAction(new PROAction(activator, receptor, interaction));
        }

        public void NewAction(PROAction action)
        {
            if (_currentStageProcedure == null)
                return;

            // Procedure stops if it's done
            if (_currentStageProcedure.Status == StepStatus.Done)
                return;

            // Send action to current stage
            int actionResult = _currentStageProcedure.NewAction(action);
            bool wasValidAction = actionResult != -1;

            if (_currentStageProcedure.Status == StepStatus.Done)
            {
                FinishProcedure();
                return;
            }

            if (wasValidAction)
            {
                OnStageChanged?.Invoke(GetCurrentStage());
                AssetActivation();
            }
        }

        private void FinishProcedure()
        {
            DeactivateActiveAssets();
            OnProcedureFinish?.Invoke();
            ClearCache();
            Debug.Log("Procedure finished.");
        }

        private void ClearCache()
        {
            _currentProcedure = null;
            _currentStageProcedure = null;
        }

        private void AssetActivation()
        {
            List<string> assetsId = GetStageAssetsId(GetCurrentStage(), true);

            DeactivateActiveAssets();

            foreach (var assetId in assetsId)
            {
                if (_procedureAssets.TryGetValue(assetId, out IPROAsset asset))
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
                    // if (stageAction.Action.Receptor == "Painel")
                    // {
                    //     assetsId.Add(stageAction.Action.Interaction.Split('-')[0]);
                    // }
                    // if (stageAction.Action.Receptor == "CaixaPrimaria")
                    // {
                    //     assetsId.Add(stageAction.Action.Receptor);
                    //     assetsId.Add(stageAction.Action.Interaction.Split('-')[0]);
                    // }
                    //else
                    //{
                    assetsId.Add(stageAction.Action.Activator);
                    assetsId.Add(stageAction.Action.Receptor);
                    //}
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

        public void PularPasso()
        {
            try
            {
                StageSequential proSequential = (StageSequential)_currentStageProcedure;
                proSequential.JumpRequirement();

                AssetActivation();
                OnStageChanged?.Invoke(GetCurrentStage());
            }
            catch
            {
                Debug.LogWarning("Não foi possível pular passo, pois o procedimentonão é sequencial.");
            }
        }

        public Stage GetCurrentStage()
        {
            return _currentStageProcedure?.CurrentStage(); ;
        }

        [ContextMenu("Load Stage")]
        public void DEBUG()
        {
            InitializeStage(2);
        }
    }
}
