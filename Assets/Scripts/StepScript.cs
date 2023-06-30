using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;
public class StepScript : MonoBehaviour
{
    [Header("Passo atual")]
    [SerializeField] int currentStep = 0;
    [Header("Lista de passos")]
    [SerializeField] List<VasoStep> stepList;
    [Header("UI")]
    [SerializeField] TMP_Text textopasso;
    [SerializeField] TMP_Text textoDescricao;
    [Header("Referencias")]
    [SerializeField] GameObject vasoGameObject;
    [Header("Referencias cabos")]
    [SerializeField] GameObject esgotoGameObject;
    [SerializeField] GameObject hidraulicoGameObject;
    [SerializeField] GameObject arGameObject;
    [SerializeField] GameObject canGameObject;
    [Header("Referencias luz")]
    [SerializeField] GameObject luzGameObject;
    [SerializeField] Material greenLightMaterial;

    private void Start()
    {
        UpdateUI(0);
    }
    public void NextStep()
    {
        currentStep++;
        UpdateUI(currentStep);
    }
    void UpdateUI(int i)
    {
        textopasso.text = "Passo " + stepList[i].passo.ToString();
        textoDescricao.text = stepList[i].descricao;
    }

# region "Metodos para os passos para serem chamados nos eventos"       
    //Colocar o vaso no lugar
    public void VasoStep()
    {
        if (currentStep==0)
        {
            vasoGameObject.GetComponent<XRGrabInteractable>().enabled = false;
            NextStep();
        }
    }
    //Colocar o saida de esgoto
    public void EsgotoStep()
    {
        if (currentStep == 1)
        {
            esgotoGameObject.GetComponent<XRGrabInteractable>().enabled = false;
            NextStep();
        }
    }



    //Colocar os cabos
    public void CabosHidraulicoStep()
    {
        if (currentStep == 2)
        {
            hidraulicoGameObject.GetComponent<XRGrabInteractable>().enabled = false;
            NextStep();
        }
    }  
    public void CabosArStep()
    {
        if (currentStep == 3)
        {
            arGameObject.GetComponent<XRGrabInteractable>().enabled = false;
            NextStep();
        }
    }
    public void CabosCanStep()
    {
        if (currentStep == 4)
        {
            canGameObject.GetComponent<XRGrabInteractable>().enabled = false;
            NextStep();
        }
    }





    //Verificar PDC
    public void VerificarStep()
    {
        luzGameObject.GetComponent<MeshRenderer>().material = greenLightMaterial;
        if (currentStep == 5)
        {
            NextStep();
        }
    }
    //Verificar PDC
    public void ButtonStep()
    {
        if (currentStep == 6)
        {
            vasoGameObject.GetComponent<AudioSource>().Play();
        }
    }
# endregion

}
[System.Serializable]
public class VasoStep
{
    public int passo;
    public string descricao;
}