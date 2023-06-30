using UnityEngine;

public class PainelMesaSocket : Painel
{
    public string NomeSaida;

    [HideInInspector] public ToolMesaMovimentacao MesaConectada;
    [HideInInspector] public PainelMesaObjeto Objeto;

    public void PermitirEmpurrar(bool Permitir)
    {
        if (Objeto != null)
        {
            Objeto.Empurrar = Permitir;
        }
        else if (MesaConectada.Objeto != null)
        {
            MesaConectada.Objeto.Empurrar = Permitir;
        }
    }
}