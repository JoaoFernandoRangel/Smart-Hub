using UnityEngine;

public class PainelChaveSocket : Painel
{
    public string Entrada;
    public Direcoes Direcao;
    public int Angulo;

    [HideInInspector] public bool Girar;
    [HideInInspector] public bool PodeGirar = true;
    [HideInInspector] public ToolKey ChaveConectada;

    private int AtualDirecao;
    private int ContadorAngulo;
    private Direcoes DirecaoInicial;
    private AudioSource AtivadoAudio;

    private bool Conectou;
    private bool ConectouOneTime;
    private bool GirouOneTime;

    private void Start()
    {
        AllStates = new States[] { States.Abrir, States.Fechar };
        AtivadoAudio = GetComponent<AudioSource>();
        DirecaoInicial = Direcao;

        if (Direcao == Direcoes.Direito)
        {
            AtualDirecao = 1;
        }
        else if (Direcao == Direcoes.Esquerdo)
        {
            AtualDirecao = -1;
        }
    }

    private void Update()
    {
        if (Girar && PodeGirar)
        {
            Girando();
        }
        else
        {
            Girar = false;
        }

        if (ChaveConectada != null)
        {
            Conectou = true;
        }
        else
        {
            Conectou = false;
        }

        if (Conectou && !ConectouOneTime)
        {
            ConectouOneTime = true;
            painelManager.NovaAcao(Nome, States.Conectar);
        }
        else if (!Conectou && ConectouOneTime)
        {
            ConectouOneTime = false;
            painelManager.NovaAcao(Nome, States.Desconectar);
        }
    }

    private void Girando()
    {
        if (!GirouOneTime)
        {
            GirouOneTime = true;
            painelManager.NovaAcao(Nome, States.Travar);
        }

        if (ContadorAngulo < Angulo)
        {
            transform.Rotate(0, 0, AtualDirecao);
            ContadorAngulo++;
        }
        else
        {
            if (Direcao == Direcoes.Direito)
            {
                Direcao = Direcoes.Esquerdo;
            }
            else if (Direcao == Direcoes.Esquerdo)
            {
                Direcao = Direcoes.Direito;
            }

            if (Direcao == DirecaoInicial)
            {
                State = States.Fechar;
            }
            else
            {
                State = States.Abrir;
            }

            AtualDirecao = -AtualDirecao;
            AtivadoAudio.Play();
            ContadorAngulo = 0;
            Girar = false;
            GirouOneTime = false;
            SendState();
        }
    }

    public void UpdateState(States NewState)
    {
        State = NewState;
        SendState();

        switch (NewState)
        {
            case States.Abrir:
                Girar = true;
                break;
            case States.Fechar:
                Girar = false;
                break;
        }
    }
}