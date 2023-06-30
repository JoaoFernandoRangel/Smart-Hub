using UnityEngine;

[System.Serializable]
public class PROAction
{
    public string Activator { get; set; }
    public string Receptor { get; set; }
    public string Interaction { get; set; }

    public PROAction() { }

    public PROAction(string activator, string receptor, string interaction)
    {
        this.Activator = activator;
        this.Receptor = receptor;
        this.Interaction = interaction;
    }

    public bool Equals(PROAction action)
    {
        try
        {
            //Se os activatores forem diferentes, retorna falso
            if (!action.Activator.Like(Activator))
            {
                return false;
            }

            //Se os receptores forem diferentes, retorna falso
            if (!action.Receptor.Like(Receptor))
            {
                return false;
            }

            //Se os receptores forem diferentes, retorna falso
            if (!action.Interaction.Like(Interaction))
            {
                return false;
            }

            //Retorna verdadeiro se as ações forem compatíveis
            return true;
        }
        catch
        {
            return false;
        }
    }

    public bool IsInverse(PROAction action)
    {
        return false;
        //return (action.activator == activator &&
        //        action.receptor == receptor &&
        //        action.interaction == undoInteraction);
    }

}
