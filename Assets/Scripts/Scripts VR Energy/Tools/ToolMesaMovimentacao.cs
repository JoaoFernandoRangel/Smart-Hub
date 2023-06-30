using UnityEngine;

public class ToolMesaMovimentacao : Tool
{
    public string NomeEntrada;
    public Transform PosicaoMesaObjeto;

    [HideInInspector] public bool Empurrando;
    [HideInInspector] public PainelMesaObjeto Objeto;
    [HideInInspector] public PainelMesaSocket SocketConectado;

    [SerializeField] private Transform PosicaoMao;

    private bool ConectouSocket;
    //private ManagerHand MaoAtual;
    private bool isTeleporting = false;
    private Vector3 offsetTeleport;

    private void Update()
    {
        /*if (MaoAtual != null)
        {
            if (!Empurrando && MaoAtual.PressionouTrigger)
            {
                ComecouEmpurrar();
            }
            else if (Empurrando)
            {
                if (!MaoAtual.PressionouTrigger || MaoAtual.transform.position.y > transform.position.y + 1)
                {
                    ParouEmpurrar();
                }

                //// Travando no teleporte
                //if (MaoAtual.PressionouAnalogico)
                //{
                //    if (!isTeleporting)
                //    {
                //        isTeleporting = true;
                //    }
                //}

                if (MaoAtual.Grabber.grabbedObject == null)
                {
                    Vector3 novaPosicao;
                    float distanciaDepois;
                    float distaciaAntes = Vector3.Distance(PosicaoMao.position, MaoAtual.transform.position);
                    float novaRotatao = AnglePositions(transform.position, MaoAtual.transform.position, PosicaoMao.position, new Vector3(1, 0, 1));
                    transform.Rotate(0, novaRotatao, 0);
                    distanciaDepois = Vector3.Distance(PosicaoMao.position, MaoAtual.transform.position);
                    if (distanciaDepois > distaciaAntes)
                        transform.Rotate(0, -2 * novaRotatao, 0);

                    novaPosicao = MaoAtual.transform.position + (transform.position - PosicaoMao.position);
                    transform.position = new Vector3(novaPosicao.x, transform.position.y, novaPosicao.z);
                }
            }
            else if (!Empurrando)
            {
                if (SocketConectado != null)
                {
                    transform.position = new Vector3(SocketConectado.transform.position.x, transform.position.y, SocketConectado.transform.position.z);
                    transform.rotation = SocketConectado.transform.rotation;
                    SocketConectado.PermitirEmpurrar(true);

                    if (!ConectouSocket)
                    {
                        if (MaoAtual != null)
                            MaoAtual.HandFeedback();

                        NovaAcao(name, SocketConectado.Nome, States.Conectar);

                        ConectouSocket = true;
                    }
                }
                else if (ConectouSocket)
                {
                    ConectouSocket = false;
                }
            }
        }*/
    }

    private void OnTriggerEnter(Collider Outro)
    {
        /*PainelMesaSocket SocketEncontrado = Outro.GetComponent<PainelMesaSocket>();

        if (SocketEncontrado != null && NomeEntrada == SocketEncontrado.NomeSaida)
        {
            SocketConectado = SocketEncontrado;
            SocketConectado.MesaConectada = this;
        }*/
    }

    private void OnTriggerStay(Collider Outro)
    {
        /*ManagerHand MaoEncontrada = Outro.GetComponent<ManagerHand>();
        bool ObjetoMovendo = ((SocketConectado == null) || (SocketConectado != null && ((SocketConectado.Objeto != null && !SocketConectado.Objeto.Empurrando) || (Objeto != null && !Objeto.Empurrando) || (Objeto == null && SocketConectado.Objeto == null))));

        if (MaoEncontrada != null && MaoEncontrada != MaoAtual && MaoEncontrada.PressionouTrigger && ObjetoMovendo)
        {
            MaoAtual = MaoEncontrada;
            ComecouEmpurrar();
        }*/
    }

    private void OnTriggerExit(Collider Outro)
    {
        /*ManagerHand MaoEncontrada = Outro.GetComponent<ManagerHand>();
        PainelMesaSocket SocketEncontrado = Outro.GetComponent<PainelMesaSocket>();

        if (SocketEncontrado != null && SocketEncontrado == SocketConectado && Empurrando)
        {
            NovaAcao(name, SocketConectado.Nome, States.Desconectar);
            SocketConectado.PermitirEmpurrar(false);
            SocketConectado.MesaConectada = null;
            SocketConectado = null;
        }
        else if (MaoEncontrada == MaoAtual)
        {
            MaoAtual = null;
        }*/
    }

    private void ComecouEmpurrar()
    {
        /*Empurrando = true;
        PosicaoMao.position = MaoAtual.transform.position;*/
    }

    private void ParouEmpurrar()
    {
        Empurrando = false;
    }

    private float AnglePositions(Vector3 Ancora, Vector3 PontoA, Vector3 PontoB, Vector3 Peso)
    {
        Vector3 Posicao1 = PontoA - Ancora;
        Vector3 Posicao2 = PontoB - Ancora;
        Posicao1 = new Vector3(Posicao1.x * Peso.x, Posicao1.y * Peso.y, Posicao1.z * Peso.z);
        Posicao2 = new Vector3(Posicao2.x * Peso.x, Posicao2.y * Peso.y, Posicao2.z * Peso.z);
        return Vector3.Angle(Posicao1, Posicao2);
    }
}