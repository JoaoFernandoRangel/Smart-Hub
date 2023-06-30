using UnityEngine;

public class PainelSocket : Painel
{
    public string Entrada;

    [HideInInspector] public ToolKey ChaveConectada;
    [HideInInspector] public PainelPlug PlugConectado;
    public ToolLock CadeadoConectado;

    private bool estavaTrancado;

    private void Update()
    {
        if (!estavaTrancado && CadeadoConectado != null)
        {
            SendState(States.Dentro);
            estavaTrancado = true;
        }
        else if (estavaTrancado && CadeadoConectado == null)
        {
            SendState(States.Fora);
            estavaTrancado = false;
        }
    }

    public void UpdateState(States NewState)
    {
        switch (NewState)
        {
            case States.Dentro:
                foreach (ToolLock CadeadoEncontrado in FindObjectsOfType<ToolLock>())
                {
                    if (CadeadoEncontrado.saida == null)
                    {
                        CadeadoConectado = CadeadoEncontrado;
                        CadeadoConectado.saida = this;
                        CadeadoConectado.Trancado = true;
                        CadeadoConectado.ConectouSaida = true;

                        string NomeEtiqueta = string.Empty;
                        switch (CadeadoConectado.saida.Nome)
                        {
                            case "CadeadoMacanetaSocket":
                                NomeEtiqueta = "Etiqueta01";
                                break;
                            case "CadeadoSeccionadoraSocket":
                                NomeEtiqueta = "Etiqueta01";
                                break;
                            case "CadeadoCaixaPrimariaSocket":
                                NomeEtiqueta = "Etiqueta02";
                                break;
                        }

                        foreach (ToolEtiqueta EtiquetaEncontrada in FindObjectsOfType<ToolEtiqueta>())
                        {
                            if (EtiquetaEncontrada.name == NomeEtiqueta)
                            {
                                //EtiquetaEncontrada.Escrever();
                                //EtiquetaEncontrada.Conectar(CadeadoConectado);
                                return;
                            }
                        }
                    }
                }
                break;
        }
    }

    public void Desconectar()
    {
        CadeadoConectado = null;
    }
}