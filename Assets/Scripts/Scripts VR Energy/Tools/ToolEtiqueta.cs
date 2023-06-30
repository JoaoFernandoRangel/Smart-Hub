using System;
using UnityEngine;
using VREnergy.PRO;

public class ToolEtiqueta : MonoBehaviour
{
    public Transform ParentTransform;
    public bool ColocarUrna;
    public Material MaterialPreenchido;
    [SerializeField] private MeshRenderer meshRenderer;

    [HideInInspector] public bool EtiquetaPreenchida;

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

    public void Escrever(ToolCaneta caneta)
    {
        if (!EtiquetaPreenchida)
        {
            meshRenderer.material = MaterialPreenchido;
            EtiquetaPreenchida = true;
        }

        FindObjectOfType<ProcedureStageHandler>()?.NewAction(
            activator: caneta.GetComponent<PROAsset>().UnityId,
            receptor: GetComponent<PROAsset>().UnityId,
            interaction: States.Escrever.ToString()
        );
    }

    internal void Conectar(GameObject objetoConectado)
    {
        transform.SetParent(null);

        if (gameObject.TryGetComponent(out FixedJoint fixedJoint))
        {
            Destroy(fixedJoint);
        }

        ProcedureStageHandler.Instance.NewAction(
            activator: GetComponent<PROAsset>().UnityId,
            receptor: objetoConectado.GetComponent<PROAsset>().UnityId,
            interaction: States.Conectar.ToString()
        );
    }
}
