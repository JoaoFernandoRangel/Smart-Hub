using System.Collections;
using UnityEngine;

public class PainelMesaObjeto : Painel
{
    //[HideInInspector]
    public bool Empurrar;
    //[HideInInspector]
    public bool Empurrando;
    //[HideInInspector]
    public ToolMesaMovimentacao AtualMesa;

    [SerializeField] private Transform PosicaoMao;
    [SerializeField] private Transform PosicaoObjeto;

    private bool Colidiu;
    private PainelPlug Plug;
    private ToolMesaMovimentacao UltimaMesa;
    //private ManagerHand MaoAtual;
    private float ContadorMovimentacao = 1f;

    private void Start()
    {
        AllStates = new States[] { States.Dentro, States.Fora };
        Plug = GetComponentInChildren<PainelPlug>();
        //DestravarObjeto(true);
    }

    private void Update()
    {
        /*if (Empurrar && MaoAtual != null && !Colidiu)
        {
            if (!Empurrando && MaoAtual.PressionouTrigger)
            {
                ComecouEmpurrar();
            }
            else if (Empurrando)
            {
                if (MaoAtual.Grabber.grabbedObject == null && Plug.State == States.Desligar)
                {
                    Vector3 novaPosicao = MaoAtual.transform.position + (transform.position - PosicaoMao.position);
                    transform.position = new Vector3(novaPosicao.x, transform.position.y, novaPosicao.z);
                }
                else
                {
                    Empurrando = false;
                }

                if (!MaoAtual.PressionouTrigger)
                {
                    ParouEmpurrar();
                }
            }
        }
        else
        {
            Empurrando = false;
        }*/
    }

    private void OnTriggerStay(Collider Outro)
    {
        /*ToolMesaMovimentacao MesaEncontrada = Outro.GetComponent<ToolMesaMovimentacao>();
        ManagerHand MaoEncontrada = Outro.GetComponent<ManagerHand>();
        PainelMesaObjeto ObjetoEncontrado = Outro.GetComponent<PainelMesaObjeto>();

        if (ObjetoEncontrado != null)
        {
            Colidiu = true;
            ParouEmpurrar();
        }
        else if (Empurrar && MaoEncontrada != null && MaoEncontrada != MaoAtual && MaoEncontrada.PressionouTrigger)
        {
            MaoAtual = MaoEncontrada;
            ComecouEmpurrar();
        }
        else if (MesaEncontrada != null && MesaEncontrada.SocketConectado != null && MesaEncontrada.Objeto == null && !Colidiu && Empurrando)
        {
            AtualMesa = MesaEncontrada;
            UpdateState(States.Fora);
        }*/
    }

    private void OnTriggerExit(Collider Outro)
    {
        /*ToolMesaMovimentacao MesaEncontrada = Outro.GetComponent<ToolMesaMovimentacao>();
        ManagerHand MaoEncontrada = Outro.GetComponent<ManagerHand>();
        PainelMesaObjeto ObjetoEncontrado = Outro.GetComponent<PainelMesaObjeto>();

        if (MesaEncontrada != null && MesaEncontrada.SocketConectado != null)
        {
            UltimaMesa = AtualMesa;
            AtualMesa = null;
            UpdateState(States.Dentro);
        }
        else if (MaoEncontrada != null)
        {
            MaoAtual = null;
            ParouEmpurrar();
        }
        else if (ObjetoEncontrado != null)
        {
            Colidiu = false;
        }*/
    }

    public void UpdateState(States NewState)
    {
        State = NewState;
        if (painelManager != null)
            SendState();

        switch (NewState)
        {
            case States.Fora:
                GetComponent<BoxCollider>().enabled = true;
                if (AtualMesa == null)
                {
                    var mesas = FindObjectsOfType<ToolMesaMovimentacao>();
                    if (mesas.Length == 1)
                    {
                        AtualMesa = mesas[0];
                    }
                    else
                    {
                        // Pegando a mesa mais próxima
                        float minDistancia = Mathf.Infinity;
                        foreach (var mesa in mesas)
                        {
                            float distancia = Vector3.Distance(transform.position, mesa.transform.position);
                            if (distancia < minDistancia)
                            {
                                minDistancia = distancia;
                                AtualMesa = mesa;
                            }
                        }

                    }
                }

                AtualMesa.Objeto = this;

                if (AtualMesa.SocketConectado != null)
                    AtualMesa.SocketConectado.Objeto = null;

                transform.position = AtualMesa.PosicaoMesaObjeto.position;
                transform.rotation = AtualMesa.PosicaoMesaObjeto.rotation;
                transform.parent = AtualMesa.transform;
                StartCoroutine(RemoveObject());
                break;
            case States.Dentro:
                if (UltimaMesa != null && UltimaMesa.SocketConectado != null)
                {
                    UltimaMesa.SocketConectado.Objeto = this;
                    painelManager = UltimaMesa.SocketConectado.painelManager;
                }

                if (UltimaMesa != null)
                    UltimaMesa.Objeto = null;

                transform.parent = painelManager.transform;
                if (!painelManager.Componentes.Find(Objeto => Objeto == this as Painel))
                    painelManager.Componentes.Add(this as Painel);

                transform.position = painelManager.PosicaoComponente.position;
                transform.rotation = painelManager.PosicaoComponente.rotation;
                (painelManager.Componentes.Find(Objeto => Objeto.Nome == "MesaSocket") as PainelMesaSocket).Objeto = this;
                break;
        }
    }

    public void DestravarObjeto(bool Travou)
    {
        StartCoroutine(Destravando(Travou));
    }

    IEnumerator Destravando(bool Destravou)
    {
        yield return new WaitForEndOfFrame();
        if (Destravou)
        {
            ContadorMovimentacao -= 0.002f;
            Debug.Log(ContadorMovimentacao);
            transform.Translate(0, 0, -0.0001f);
        }
        else
        {
            ContadorMovimentacao += 0.002f;
            transform.Translate(0, 0, 0.0001f);
        }

        if (ContadorMovimentacao < 1f && ContadorMovimentacao > 0f)
        {
            StartCoroutine(Destravando(Destravou));
        }
        else
        {
            StopCoroutine(Destravando(Destravou));
        }
    }

    private void ComecouEmpurrar()
    {
        Empurrando = true;
        //PosicaoMao.position = MaoAtual.transform.position;
    }

    private void ParouEmpurrar()
    {
        Empurrando = false;

        if (Colidiu && State == States.Dentro)
        {
            transform.position = painelManager.PosicaoComponente.position;
            transform.rotation = painelManager.PosicaoComponente.rotation;
        }
        else if (Colidiu && State == States.Fora)
        {
            transform.position = AtualMesa.PosicaoMesaObjeto.position;
            transform.rotation = AtualMesa.PosicaoMesaObjeto.rotation;
        }
        else
        {
            if (AtualMesa != null && AtualMesa.SocketConectado != null)
            {
                UpdateState(States.Fora);
            }
            else
            {
                if (UltimaMesa != null && UltimaMesa.SocketConectado != null)
                {
                    UltimaMesa.SocketConectado.Objeto = this;
                    painelManager = UltimaMesa.SocketConectado.painelManager;
                }

                if (painelManager != null)
                {
                    UpdateState(States.Dentro);
                }
            }
        }
    }

    private IEnumerator RemoveObject()
    {
        yield return new WaitForFixedUpdate();
        if (painelManager != null && painelManager.TreinamentoIniciado)
        {
            painelManager?.Componentes.Remove(painelManager.Componentes.Find(Objeto => Objeto == this as Painel));
            painelManager = null;
            StopAllCoroutines();
        }
        else if (painelManager != null)
        {
            StartCoroutine(RemoveObject());
        }
        else
        {
            StopAllCoroutines();
        }
    }
}