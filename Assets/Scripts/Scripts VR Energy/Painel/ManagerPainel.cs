using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;

public class ManagerPainel : MonoBehaviour
{
    public int PainelID;
    public bool PainelContator;
    public bool PainelDisjuntor;
    public bool CaixaPrimaria;
    public Transform PosicaoComponente;
    public Text PainelNumberText;
    public string PainelName;

    [HideInInspector] public bool TreinamentoIniciado;
    public List<Painel> Componentes;

    private List<PainelSinalizador> Sinalizadores;
    private ManagerSceneFree SceneIsFree;

    private void Awake()
    {
        Componentes = new List<Painel>(GetComponentsInChildren<Painel>());
        Sinalizadores = new List<PainelSinalizador>(GetComponentsInChildren<PainelSinalizador>());
    }

    private void Start()
    {
        if (PainelContator)
        {
            PainelNumberText.text = "Contator\n" + PainelID;
        }
        else if (PainelDisjuntor)
        {
            PainelNumberText.text = "Disjuntor\n" + PainelID;
        }

        SceneIsFree = FindObjectOfType<ManagerSceneFree>();
        if (SceneIsFree == null)
            StartCoroutine(IniciandoPainel());
    }

    public bool UpdateObjectState(string Name, States State)
    {
        try
        {
            Componentes.Find(Objeto => Objeto.name == Name).SendMessage("UpdateState", State);
            return true;
        }
        catch
        {
            Debug.LogWarning(Name + " objeto do painel não encontrado.");
            return false;
        }
    }

    public void NovaAcao(string Nome, States State, bool sendAction = true)
    {
        if (SceneIsFree == null || (SceneIsFree != null && TreinamentoIniciado))
        {
            if (PainelContator)
            {
                PainelContatorConfig(Nome, State);
            }
            else if (PainelDisjuntor)
            {
                PainelDisjuntorConfig(Nome, State);
            }
            else if (CaixaPrimaria)
            {
                CaixaPrimariaConfig(Nome, State);
            }
        }


        if ((SceneIsFree == null) || (SceneIsFree != null && SceneIsFree.ActualPainelID == PainelID))
        {
            if (sendAction && TreinamentoIniciado)
            {
                PROManager.main.NewAction("Operator", PainelName, Nome + "-" + State);
            }
        }
    }

    public void NovaAcao(string Nome, States[] States)
    {
        NovaAcao(Nome, States[States.Length - 1], sendAction: false);
        string interaction = Nome;
        foreach (var state in States)
        {
            interaction += "-" + state;
        }
        PROManager.main.NewAction("Operator", PainelName, interaction);
    }

    public Type Componente<Type>(string Name) where Type : class
    {
        return (Componentes.Find(Objeto => Objeto.Nome == Name) as Type);
    }

    public void AtualizarSinalizadores(int[] SinalizadoresLigar, int[] SinalizadoresDesligar)
    {
        if (SinalizadoresLigar != null)
        {
            foreach (int ID in SinalizadoresLigar)
            {
                Sinalizadores.Find(AtualSinalizador => AtualSinalizador.ID == ID).State = States.Ligar;
            }
        }

        if (SinalizadoresDesligar != null)
        {
            foreach (int ID in SinalizadoresDesligar)
            {
                Sinalizadores.Find(AtualSinalizador => AtualSinalizador.ID == ID).State = States.Desligar;
            }
        }
    }

    private void PainelContatorConfig(string Nome, States State)
    {
        if (Nome == "Plug")
        {
            switch (State)
            {
                case States.Ligar:
                    AtualizarSinalizadores(new int[] { 1 }, new int[] { 0 });
                    break;
                case States.Desligar:
                    AtualizarSinalizadores(new int[] { 0 }, new int[] { 1 });
                    break;
            }
        }
        else if (Nome == "MacanetaPortaCimaSocket")
        {
            switch (State)
            {
                case States.Abrir:
                    Componente<Painel>("PortaCima").HabilitarDependencias(true);
                    break;
                case States.Fechar:
                    Componente<Painel>("PortaCima").HabilitarDependencias(false);
                    break;
            }
        }
        else if (Nome == "MacanetaPortaMeioSocket")
        {
            switch (State)
            {
                case States.Abrir:
                    Componente<Painel>("PortaMeio").HabilitarDependencias(true);
                    Componente<Painel>("TravaContatorSocket").HabilitarDependencias(false);
                    break;
                case States.Fechar:
                    Componente<Painel>("PortaMeio").HabilitarDependencias(false);
                    if (Componente<Painel>("PortinholaSocket").State == States.Abrir)
                    {
                        Componente<Painel>("TravaContatorSocket").HabilitarDependencias(true);
                    }
                    else
                    {
                        Componente<Painel>("TravaContatorSocket").HabilitarDependencias(false);
                    }
                    break;
            }
        }
        else if (Nome == "MacanetaPortaBaixoSocket01" || Nome == "MacanetaPortaBaixoSocket02" || Nome == "MacanetaPortaBaixoSocket03")
        {
            if (Componente<Painel>("MacanetaPortaBaixoSocket01").State == States.Abrir && Componente<Painel>("MacanetaPortaBaixoSocket02").State == States.Abrir && Componente<Painel>("MacanetaPortaBaixoSocket03").State == States.Abrir)
            {
                Componente<Painel>("PortaBaixo").HabilitarDependencias(true);
            }
            else
            {
                Componente<Painel>("PortaBaixo").HabilitarDependencias(false);
            }
        }
        else if (Nome == "PortinholaSocket")
        {
            if (Componente<Painel>("PortaMeio").State == States.Fechar && Componente<Painel>("Contator") != null)
            {
                switch (State)
                {
                    case States.Abrir:
                        Componente<Painel>("TravaContatorSocket").HabilitarDependencias(true);
                        break;
                    case States.Fechar:
                        Componente<Painel>("TravaContatorSocket").HabilitarDependencias(false);
                        break;
                    case States.Desconectar:
                        if (Componente<PainelChaveSocket>("PortinholaSocket").State == States.Abrir)
                            Componente<PainelChaveSocket>("PortinholaSocket").Girar = true;
                        break;
                }
            }
            else
            {
                Componente<Painel>("TravaContatorSocket").HabilitarDependencias(false);
            }
        }
        else if (Nome == "TravaContatorSocket")
        {
            switch (State)
            {
                case States.Abrir:
                    Componente<Painel>("Contator").HabilitarDependencias(true, true);
                    break;
                case States.Fechar:
                    Componente<Painel>("Contator").HabilitarDependencias(false, true);
                    break;
                case States.Travar:
                    if (Componente<Painel>("TravaContatorSocket").State == States.Abrir)
                        Componente<PainelMesaObjeto>("Contator").DestravarObjeto(false);
                    else if (Componente<Painel>("TravaContatorSocket").State == States.Fechar)
                        Componente<PainelMesaObjeto>("Contator").DestravarObjeto(true);
                    break;
                case States.Conectar:
                    Componente<PainelChaveSocket>("PortinholaSocket").PodeGirar = false;
                    Componente<PainelChaveSocket>("PortinholaSocket").ChaveConectada.PodeRetirar = false;
                    break;
                case States.Desconectar:
                    Componente<PainelChaveSocket>("PortinholaSocket").PodeGirar = true;
                    Componente<PainelChaveSocket>("PortinholaSocket").ChaveConectada.PodeRetirar = true;
                    break;
            }
        }
        else if (Nome == "PortaMeio")
        {
            switch (State)
            {
                case States.Abrir:

                    Componente<Painel>("Plug").HabilitarDependencias(true);
                    Componente<Painel>("MesaSocket").HabilitarDependencias(true);
                    Componente<Painel>("TravaContatorSocket").HabilitarDependencias(false);

                    if (Componente<Painel>("Contator") != null)
                        Componente<Painel>("Contator").HabilitarDependencias(true, true);

                    break;
                case States.Fechar:
                    Componente<Painel>("Plug").HabilitarDependencias(false);
                    Componente<Painel>("MesaSocket").HabilitarDependencias(false);

                    if (Componente<Painel>("Contator") != null)
                        Componente<Painel>("Contator").HabilitarDependencias(false, true);

                    if (Componente<Painel>("PortinholaSocket").State == States.Abrir)
                    {
                        Componente<Painel>("TravaContatorSocket").HabilitarDependencias(true);
                    }
                    else
                    {
                        Componente<Painel>("TravaContatorSocket").HabilitarDependencias(false);
                    }
                    break;
            }
        }
        else if (Nome == "CadeadoMacanetaSocket")
        {
            switch (State)
            {
                case States.Dentro:
                    Componente<Painel>("MacanetaPortaMeioSocket").HabilitarDependencias(false);
                    break;
                case States.Fora:
                    Componente<Painel>("MacanetaPortaMeioSocket").HabilitarDependencias(true);
                    break;
            }
        }
    }

    private void PainelDisjuntorConfig(string Nome, States State)
    {
        if (Nome == "Plug")
        {
            switch (State)
            {
                case States.Ligar:
                    //Componente<PainelSwitch>("SwitchOnOff").ChangeSwitch(States.Ligar);
                    break;
                case States.Desligar:
                    //Componente<PainelSwitch>("SwitchOnOff").ChangeSwitch(States.Desligar);
                    break;
            }
        }
        else if (Nome == "MacanetaPortaCimaSocket")
        {
            switch (State)
            {
                case States.Abrir:
                    Componente<Painel>("PortaCima").HabilitarDependencias(true);
                    break;
                case States.Fechar:
                    Componente<Painel>("PortaCima").HabilitarDependencias(false);
                    break;
            }
        }
        else if (Nome == "MacanetaPortaMeioSocket")
        {
            switch (State)
            {
                case States.Abrir:
                    Componente<Painel>("PortaMeio").HabilitarDependencias(true);
                    Componente<Painel>("TravaDisjuntorSocket").HabilitarDependencias(false);
                    break;
                case States.Fechar:
                    Componente<Painel>("PortaMeio").HabilitarDependencias(false);
                    if (Componente<Painel>("PortinholaSocket").State == States.Abrir)
                    {
                        Componente<Painel>("TravaDisjuntorSocket").HabilitarDependencias(true);
                    }
                    else
                    {
                        Componente<Painel>("TravaDisjuntorSocket").HabilitarDependencias(false);
                    }
                    break;
            }
        }
        else if (Nome == "MacanetaPortaBaixoSocket")
        {
            switch (State)
            {
                case States.Abrir:
                    Componente<Painel>("PortaBaixo").HabilitarDependencias(true);
                    break;
                case States.Fechar:
                    Componente<Painel>("PortaBaixo").HabilitarDependencias(false);
                    break;
            }
        }
        else if (Nome == "PortinholaSocket")
        {
            if (Componente<Painel>("PortaMeio").State == States.Fechar && Componente<Painel>("Disjuntor") != null)
            {
                switch (State)
                {
                    case States.Abrir:
                        Componente<Painel>("TravaDisjuntorSocket").HabilitarDependencias(true);
                        break;
                    case States.Fechar:
                        Componente<Painel>("TravaDisjuntorSocket").HabilitarDependencias(false);
                        break;
                    case States.Desconectar:
                        if (Componente<PainelChaveSocket>("PortinholaSocket").State == States.Abrir)
                            Componente<PainelChaveSocket>("PortinholaSocket").Girar = true;
                        break;
                }
            }
            else
            {
                Componente<Painel>("TravaDisjuntorSocket").HabilitarDependencias(false);
            }
        }
        else if (Nome == "TravaDisjuntorSocket")
        {
            switch (State)
            {
                case States.Abrir:
                    Componente<Painel>("Disjuntor")?.HabilitarDependencias(true, true);
                    break;
                case States.Fechar:
                    Componente<Painel>("Disjuntor")?.HabilitarDependencias(false, true);
                    break;
                case States.Travar:
                    if (Componente<Painel>("TravaDisjuntorSocket").State == States.Abrir)
                        Componente<PainelMesaObjeto>("Disjuntor").DestravarObjeto(false);
                    else if (Componente<Painel>("TravaDisjuntorSocket").State == States.Fechar)
                        Componente<PainelMesaObjeto>("Disjuntor").DestravarObjeto(true);
                    break;
                case States.Conectar:
                    Componente<PainelChaveSocket>("PortinholaSocket").PodeGirar = false;
                    Componente<PainelChaveSocket>("PortinholaSocket").ChaveConectada.PodeRetirar = false;
                    break;
                case States.Desconectar:
                    Componente<PainelChaveSocket>("PortinholaSocket").PodeGirar = true;
                    Componente<PainelChaveSocket>("PortinholaSocket").ChaveConectada.PodeRetirar = true;
                    break;
            }
        }
        else if (Nome == "PortaMeio")
        {
            switch (State)
            {
                case States.Abrir:

                    Componente<Painel>("Plug").HabilitarDependencias(true);
                    Componente<Painel>("MesaSocket").HabilitarDependencias(true);
                    Componente<Painel>("TravaDisjuntorSocket").HabilitarDependencias(false);

                    if (Componente<Painel>("Disjuntor") != null)
                        Componente<Painel>("Disjuntor").HabilitarDependencias(true, true);

                    break;
                case States.Fechar:
                    Componente<Painel>("Plug").HabilitarDependencias(false);
                    Componente<Painel>("MesaSocket").HabilitarDependencias(false);

                    if (Componente<Painel>("Disjuntor") != null)
                        Componente<Painel>("Disjuntor").HabilitarDependencias(false, true);

                    if (Componente<Painel>("PortinholaSocket").State == States.Abrir)
                    {
                        Componente<Painel>("TravaDisjuntorSocket").HabilitarDependencias(true);
                    }
                    else
                    {
                        Componente<Painel>("TravaDisjuntorSocket").HabilitarDependencias(false);
                    }
                    break;
            }
        }
        else if (Nome == "CadeadoSeccionadoraSocket")
        {
            switch (State)
            {
                case States.Dentro:
                    Componente<Painel>("SeccionadoraSocket").HabilitarDependencias(false);
                    break;
                case States.Fora:
                    Componente<Painel>("SeccionadoraSocket").HabilitarDependencias(true);
                    break;
            }
        }
    }

    private void CaixaPrimariaConfig(string Nome, States State)
    {
        if (Nome == "CadeadoCaixaPrimariaSocket")
        {
            switch (State)
            {
                case States.Dentro:
                    Componente<Painel>("TampaCaixa").HabilitarDependencias(false);
                    break;
                case States.Fora:
                    Componente<Painel>("TampaCaixa").HabilitarDependencias(true);
                    break;
            }
        }
    }

    private IEnumerator IniciandoPainel()
    {
        yield return new WaitForSeconds(3f);
        TreinamentoIniciado = true;
    }
}