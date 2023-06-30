using UnityEngine;
using VREnergy.SceneManagement;

public class LoadSceneBehaviour : MonoBehaviour
{
    [SerializeField] private string _sceneKey;
    
    private SceneLoader _sceneLoader;

    private void Awake()
    {
        _sceneLoader = DependencyContainer.Instance.Get<SceneLoader>();
    }

    public void LoadScene()
    {
        if (_sceneLoader.IsSceneLoading)
        {
            return;
        }
        _sceneLoader.LoadScene(_sceneKey, SceneLoader.LoadingScreenMode.Start);
    }
}
