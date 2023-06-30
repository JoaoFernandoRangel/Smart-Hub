using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using VREnergy.PRO;
using VREnergy.PRO.Model;
using VREnergy.SceneManagement;
using Action = System.Action;

namespace VREnergy.SceneBuilder
{
    public class ProcedureBuilder : MonoBehaviour
    {
        [Header("Dependencies")]
        [SerializeField] private SceneLoader sceneLoader;
        [SerializeField] private ProcedureStageHandler procedureStageHandler;

        private IProcedureService _service;

        private void Awake()
        {
            _service = DependencyContainer.Instance.Get<IProcedureService>();

            if (sceneLoader == null)
                sceneLoader = FindObjectOfType<SceneLoader>();

            if (procedureStageHandler == null)
                procedureStageHandler = FindObjectOfType<ProcedureStageHandler>();
        }

        public void ConstructProcedure(int procedureId, Action onSceneReady = null)
        {
            Procedure procedure = _service.GetProcedure(procedureId);

            if (procedure == null)
            {
                Debug.LogError("Procedure not found.", this);
                return;
            }

            sceneLoader.LoadScene(procedure.Scene.AddressableKey, SceneLoader.LoadingScreenMode.Transition, async () =>
            {
                await BuildAssetsAsync(procedure);

                procedureStageHandler.InitializeStage(procedureId);

                onSceneReady?.Invoke();
            });
        }

        public async Task BuildAssetsAsync(Procedure procedure)
        {
            var instantiateTasks = new List<Task>();
            var scene = procedure.Scene;

            foreach (var sceneObject in scene.SceneObjects)
            {
                AsyncOperationHandle<GameObject> handle = Addressables.InstantiateAsync(
                    sceneObject.AddressableKey,
                    sceneObject.Position,
                    Quaternion.Euler(sceneObject.Rotation)
                );

                handle.Completed += h => AssetInitializationCompleted(h, sceneObject);
                instantiateTasks.Add(handle.Task);
            }

            await Task.WhenAll(instantiateTasks);
            Debug.Log("All tasks completed.");
        }

        private void AssetInitializationCompleted(AsyncOperationHandle<GameObject> handle, SceneObject sceneObject)
        {
            Debug.Log("Asset loaded");
            if (handle.Status == AsyncOperationStatus.Succeeded)
            {
                GameObject asset = handle.Result;
                asset.transform.localScale = sceneObject.Scale;

                if (asset.TryGetComponent(out IPROAsset proAsset))
                {
                    proAsset.UnityId = sceneObject.UnityId;
                }
            }
            else
            {
                Debug.LogWarning("Failed to load the asset.", this);
            }
        }

#if UNITY_EDITOR
        [ContextMenu("Load Procedure with ID 3")]
        public void LoadProcedure3()
        {
            ConstructProcedure(3, null);
        }
#endif
    }
}
