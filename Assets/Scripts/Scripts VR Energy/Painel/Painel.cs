using UnityEngine;

public class Painel : MonoBehaviour
{
    [HideInInspector] public string Nome;
    [HideInInspector] public bool Verificado;
    public ManagerPainel painelManager;
    [HideInInspector] public States State;
    [HideInInspector] public States[] AllStates;

    public GameObject Modelo;
    public Collider ColisorDependente;

    protected Collider[] Colisores;
    protected ManagerSceneFree SceneIsFree;

    private bool _hover;

    public void Awake()
    {
        Nome = name;
        painelManager = GetComponentInParent<ManagerPainel>();
        if (ColisorDependente != null)
            Colisores = ColisorDependente.GetComponentsInChildren<Collider>();
    }

    private void Start()
    {
        SceneIsFree = FindObjectOfType<ManagerSceneFree>();
    }

    public void SendState()
    {
        painelManager.NovaAcao(Nome, State);
    }

    public void SendState(States state)
    {
        State = state;
        SendState();
    }

    public void HabilitarDependencias(bool Ativado)
    {
        foreach (Collider AtualColisor in Colisores)
        {
            AtualColisor.enabled = Ativado;
        }
    }

    public void HabilitarDependencias(bool Ativado, bool One)
    {
        ColisorDependente.enabled = Ativado;
    }
}