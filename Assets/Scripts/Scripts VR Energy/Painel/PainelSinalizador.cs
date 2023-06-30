using UnityEngine;

public class PainelSinalizador : Painel
{
    public int ID;
    [SerializeField] private Texture TexturaLigado;
    [SerializeField] private Texture TexturaDesligado;

    private Material material;

    private void Start()
    {
        AllStates = new States[] { States.Ligar, States.Desligar };
        material = Modelo.GetComponent<Renderer>().material;
        State = States.Desligar;
    }

    private void Update()
    {
        if (State == States.Ligar)
        {
            material.mainTexture = TexturaLigado;
        }
        else if (State == States.Desligar)
        {
            material.mainTexture = TexturaDesligado;
        }
    }

    public void EstadoInicial(States Estado)
    {

    }
}