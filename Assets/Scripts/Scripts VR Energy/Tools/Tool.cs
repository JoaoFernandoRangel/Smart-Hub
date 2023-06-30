using System;
using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public abstract class Tool : MonoBehaviour
{
    public bool VoltaPosicao = true;

    [HideInInspector] public bool Verificado;
    [HideInInspector] public AudioSource AudioColisao;
    [HideInInspector] public bool TreinamentoIniciado;

    protected XRGrabInteractable Grabbable;
    protected Collider[] Colisores;
    protected new Rigidbody rigidbody;
    protected Vector3 PosicaoInicial;
    protected Quaternion RotacaoInicial;
    protected ManagerSceneFree SceneIsFree;

    public void Start()
    {
        //SceneIsFree = FindObjectOfType<ManagerSceneFree>();
        //if (SceneIsFree != null)
        //{
            Init();
        //}
    }

    public void Init()
    {
        PosicaoInicial = transform.position;
        RotacaoInicial = transform.rotation;
        AudioColisao = GetComponent<AudioSource>();
        rigidbody = GetComponent<Rigidbody>();
        Colisores = GetComponentsInChildren<Collider>();
        Grabbable = GetComponent<XRGrabInteractable>();

        Grabbable.selectEntered.AddListener(OnSelectEnteredListener);
        Grabbable.selectExited.AddListener(OnSelectExitedListener);
        Grabbable.activated.AddListener(OnActivateListener);

        TreinamentoIniciado = true;
    }

    protected virtual void OnActivateListener(ActivateEventArgs interactor) { }
    protected virtual void OnSelectEnteredListener(SelectEnterEventArgs interactor) { }
    protected virtual void OnSelectExitedListener(SelectExitEventArgs interactor) { }

    public void NovaAcao(string Ativador, string Nome, States Acao)
    {
        try
        {
            PROManager.main.NewAction(Ativador, Nome, Acao.ToString());
        }
        catch { }
    }
    public void NovaAcao(string Nome, States Acao)
    {
        NovaAcao("Operator", Nome, Acao);
    }

    public void HabilitarObjeto(bool Ativado)
    {
        for (int c = 0; c < Colisores.Length; c++)
        {
            Colisores[c].enabled = Ativado;
        }
    }

    private void OnCollisionEnter(Collision Outro)
    {
        if (Outro.gameObject.CompareTag("Ground"))
        {
            if (VoltaPosicao)
            {
                transform.position = PosicaoInicial;
                transform.rotation = RotacaoInicial;
                rigidbody.velocity = Vector3.zero;
            }

            if (AudioColisao != null)
            {
                AudioColisao.Play();
            }
        }
    }
}