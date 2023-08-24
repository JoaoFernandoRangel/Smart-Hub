using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Estados do pistao
public enum PistaoState
{ ParadoInicio = 1, EmMovimento = 2, FechadoFinal = 3, Erro = 4 };

public class AddForcePiston : MonoBehaviour
{
    //variaveis de forca

    [Header("Forca")]
    [SerializeField] float maxValue = 0.17f;
    [SerializeField] float minValue = 0;
    [SerializeField] float lowerLimit;
    [SerializeField] float stiffness;
    [SerializeField] float damping;
    [SerializeField] float forceLimit;

    ArticulationBody articulation;
    ArticulationDrive Ydrive;
    [Header("Tracking")]
    [SerializeField]
    int intPistaoState;
    [SerializeField]
    PistaoState pistaoState;

    public PistaoState PistaoState { get => pistaoState; set => pistaoState = value; }
    public int IntPistaoState { get => intPistaoState; set => intPistaoState = value; }

    private void Start()
    {
        articulation = GetComponent<ArticulationBody>();

        Ydrive.upperLimit = maxValue;
        Ydrive.lowerLimit = lowerLimit;
        Ydrive.stiffness = stiffness;
        Ydrive.damping = damping;
        Ydrive.lowerLimit = lowerLimit;
        Ydrive.forceLimit = forceLimit;

        articulation.yDrive = Ydrive;
    }
    private void Update()
    {
        //

        if (pistaoState == PistaoState.ParadoInicio)
        {
            Ydrive.target = 0;
            articulation.yDrive = Ydrive;
        }
        else if (pistaoState == PistaoState.FechadoFinal)
        {
            Ydrive.target = 1;
            articulation.yDrive = Ydrive;
        }

    } //chamar esse metodo para sincronizar e mudar o estado

    public void ChangePistaoState(int intPistaoState)
    {
        if (intPistaoState == 1)
        {
            pistaoState = PistaoState.ParadoInicio;
        }
        if (intPistaoState == 3)
        {
            pistaoState = PistaoState.FechadoFinal;
        }
    }
    
}
