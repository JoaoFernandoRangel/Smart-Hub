using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class PainelPlug : Painel
{
    public string Entrada;
    public PainelSocket saida;

    [HideInInspector] public bool PodeConectar = true;

    [SerializeField] private PainelMesaObjeto ObjetoMesclado;
    [SerializeField] private PainelSocket PlugDesligadoSocket;

    private Animator Anim;
    //private ManagerHand Mao;
    private bool ConectouSaida;
    private bool Travado = true;
    private bool PodeTravar = true;
    private XRGrabInteractable Grabbable;
    private States TravaState = States.Travar;

    private void Start()
    {
        AllStates = new States[] { States.Ligar, States.Desligar, States.Desconectar };
        Anim = GetComponentInChildren<Animator>();
        Grabbable = GetComponentInChildren<XRGrabInteractable>();
    }

    private void Update()
    {
        /*painelManager = ObjetoMesclado.painelManager;

        if (Grabbable.grabbedBy != null)
            Mao = Grabbable.grabbedBy.GetComponent<ManagerHand>();
        else
            Mao = null;

        if (painelManager != null)
        {
            if ((saida != null && Mao == null) || Travado)
            {
                transform.position = saida.transform.position;
                transform.rotation = saida.transform.rotation;

                if (!ConectouSaida)
                {
                    if (PlugDesligadoSocket == saida)
                    {
                        UpdateState(States.Desligar);
                    }
                    else
                    {
                        UpdateState(States.Ligar);
                    }

                    ConectouSaida = true;
                }
            }
            else if (ConectouSaida)
            {
                ConectouSaida = false;
                Mao.HandFeedback();
                UpdateState(States.Desconectar);
            }
        }
        else
        {
            transform.position = saida.transform.position;
            transform.rotation = saida.transform.rotation;
        }*/
    }

    private void OnTriggerEnter(Collider Outro)
    {
        PainelSocket SaidaEncontrada = Outro.GetComponent<PainelSocket>();

        if (PodeConectar && SaidaEncontrada != null && Entrada == SaidaEncontrada.Entrada && TravaState != States.Travar)
        {
            saida = SaidaEncontrada;
            saida.PlugConectado = this;
        }
    }

    private void OnTriggerStay(Collider Outro)
    {
        /*ManagerHand MaoEncontrada = Outro.GetComponent<ManagerHand>();

        if (MaoEncontrada != null && MaoEncontrada.PressionouBotao && PodeTravar && painelManager != null)
        {
            Travado = !Travado;
            if (Travado)
            {
                Anim.SetInteger("AnimationID", 0);
                TravaState = States.Travar;
            }
            else
            {
                Anim.SetInteger("AnimationID", 1);
                TravaState = States.Destravar;
            }
            painelManager.NovaAcao(name, new States[] { State, TravaState });
            PodeTravar = false;
            Invoke("PermitirTravar", 0.5f);
        }*/
    }

    private void PermitirTravar()
    {
        PodeTravar = true;
    }

    public void UpdateState(States NewState)
    {
        State = NewState;
        SendState();

        switch (NewState)
        {
            case States.Desligar:
                saida = PlugDesligadoSocket;
                break;
        }
    }
}