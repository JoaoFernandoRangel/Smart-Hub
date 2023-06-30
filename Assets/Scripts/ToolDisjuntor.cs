using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using VREnergy.PRO;

public class ToolDisjuntor : MonoBehaviour
{
    public Transform ParentTransform;
    public bool ColocarUrna;
    public Material MaterialPreenchido;

    public bool EtiquetaPreenchida;

    public UnityEvent OnComplete;
    /*private void OnTriggerEnter(Collider other)
    {
        ToolCadeado CadeadoEncontrado = other.GetComponent<ToolCadeado>();
        if (CadeadoEncontrado != null && CadeadoEncontrado.EtiquetaEquipada == null)
        {
            Conectar(CadeadoEncontrado);
        }
    }

    public void Conectar(ToolCadeado CadeadoEncontrado)
    {
        ParentTransform = null;
        CadeadoEncontrado.EtiquetaEquipada = this;
        transform.parent = CadeadoEncontrado.transform;
        transform.localPosition = Vector3.zero;
        transform.localRotation = Quaternion.Euler(Vector3.zero);
        Colisor.enabled = false;
        NovaAcao(name, CadeadoEncontrado.name, States.Conectar);

        // Dizendo ao PROManager que agora o cadeado tem a etiqueta
        CadeadoEncontrado.GetComponent<ToolCadeado>().name = "Cadeado" + name;
        //CadeadoEncontrado.Nome = "Cadeado" + Nome;
    }*/

    public void Escrever()
    {
        if (!EtiquetaPreenchida)
        {
            Renderer rend = GetComponent<Renderer>();
            rend.material = MaterialPreenchido;
            EtiquetaPreenchida = true;
        }
        FindObjectOfType<ProcedureStageHandler>().NewAction(new PROAction
        {
            Activator = "Operador",
            Receptor = GetComponent<PROAsset>().UnityId,
            Interaction = States.Apertar.ToString()
        });
        OnComplete?.Invoke();   
    }


}
