using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
public class SceneSelectorMenu : MonoBehaviour
{
    [SerializeField]
    List<Object> scenesList = new List<Object>();
    [SerializeField]
    TMP_Dropdown dropdown;

    
    private void Start()
    {
        for (int i = 0; i < scenesList.Count; i++)
        {
            dropdown.options.Add(new TMP_Dropdown.OptionData(scenesList[i].name));
        }
    }


    public void StartScene()
    {
        SceneManager.LoadScene(dropdown.options[dropdown.value].text);
        Debug.Log("sceneName to load: " + dropdown.options[dropdown.value].text);
    }
}
