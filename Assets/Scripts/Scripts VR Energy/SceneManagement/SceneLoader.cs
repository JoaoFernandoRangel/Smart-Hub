using System;
using System.Collections;
using UnityEngine;
using UnityEngine.ResourceManagement.AsyncOperations;
using UnityEngine.ResourceManagement.ResourceProviders;
using UnityEngine.SceneManagement;

namespace VREnergy.SceneManagement
{
    public class SceneLoader : MonoBehaviour
    {
        public enum LoadingScreenMode
        {
            /// <summary>
            /// Realiza o Fade ao solicitar o carregamento de cena.
            /// </summary>
            Start,
            
            /// <summary>
            /// Realiza o Fade ao transitar para a nova cena.
            /// </summary>
            Transition
        }
        
        private AsyncOperationHandle<SceneInstance> _currentScene;
        private Coroutine loadSceneCoroutine = null;
        
        public bool IsSceneLoading => loadSceneCoroutine != null;
        public float Progress { get; private set; }

        public void LoadScene(string sceneKey, LoadingScreenMode loadingScreenMode, Action onComplete = null)
        {
            if (IsSceneLoading) return;
            
            loadSceneCoroutine = StartCoroutine(LoadSceneRoutine(sceneKey, loadingScreenMode, onComplete));
        }
        
        private IEnumerator LoadSceneRoutine(string sceneKey, LoadingScreenMode loadingScreenMode, Action onComplete = null)
        {
            AsyncOperation operation = SceneManager.LoadSceneAsync(sceneKey, LoadSceneMode.Single);

            operation.allowSceneActivation = false;
            
            if (loadingScreenMode == LoadingScreenMode.Start)
            {
                yield return new WaitForSeconds(FindObjectOfType<ScreenFade>().FadeIn());
            }

            while (!operation.isDone)
            {
                Progress = Mathf.Clamp01(operation.progress / .9f);
                
                if (operation.progress >= 0.9f)
                {
                    if (loadingScreenMode == LoadingScreenMode.Transition)
                    {
                        yield return new WaitForSeconds(FindObjectOfType<ScreenFade>().FadeIn());
                    }
                    operation.allowSceneActivation = true;
                }
                
                yield return null;
            }
            
            loadSceneCoroutine = null;
            Progress = 0f;
            
            yield return null;
            
            onComplete?.Invoke();
        }

#if UNITY_EDITOR
        [ContextMenu("Load Menu")]
        public void LoadMenu()
        {
            LoadScene("Scenes/Menu.unity", LoadingScreenMode.Start, () => Debug.Log("Menu Loaded."));
        }
#endif
    }
}
