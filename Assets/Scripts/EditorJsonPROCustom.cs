using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

#if UNITY_EDITOR
using UnityEditor;
#endif

public class EditorJsonPROCustom : MonoBehaviour
{
    [SerializeField]
    public Proceduresa[] procedures;
}


#if UNITY_EDITOR
[CustomEditor(typeof(EditorJsonPROCustom))]
class EditorJsonPROCustomGUI : Editor
{
    SerializedProperty repeatProperty;
    void OnEnable()
    {
        repeatProperty = serializedObject.FindProperty("procedures");
    }
    public override void OnInspectorGUI()
    {
        serializedObject.Update();
        EditorGUILayout.PropertyField(repeatProperty);
        serializedObject.ApplyModifiedProperties();
        var thing = (EditorJsonPROCustom)target;
        if (thing==null)
        {
            return;
        }


        if (GUILayout.Button("Salvar como JSON"))
        {
            //chamar metodo salvar json
            SaveIntoJson();
        }
    }
    public void SaveIntoJson()
    {
        var outputString = JsonUtility.ToJson(repeatProperty);
        string filePath = Application.dataPath + "/teste.json";
        Debug.Log(Application.dataPath);
        File.WriteAllText(filePath, outputString);
    }
}
#endif

[System.Serializable]
public class Proceduresa{
    public int id;
    public string Name;
    public string description;
    public int sceneId;
    public Requirementsa1[] requirements;
}
[System.Serializable]
public class Requirementsa1
{
    public string id;
    public string description;
    public string executionType;
    public Requirementsa2[] requirements;
    public Actionca action;
}
[System.Serializable]
public class Requirementsa2
{
    public string id;
    public string description;
    public string executionType;
    public Actionca action;
}
[System.Serializable]
public class Actionca
{
    public string Activator;
    public string Receptor;
    public string Interaction;

}