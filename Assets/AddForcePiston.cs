using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Estados do pistao
public enum PistaoState
{ ParadoInicio = 1, EmMovimento = 2, FechadoFinal = 3, Erro = 4 };
public enum Drive
{ XDrive = 1, YDrive = 2, ZDrive = 3};
public enum TipoPistao
{ Horizontal = 1, Vertical = 2 };
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
    [SerializeField]
    Drive driveState;
    ArticulationBody articulation;
    ArticulationDrive drive;
    [Header("Tracking")]
    [SerializeField]
    char nomeDoPistao;
    [SerializeField]
    TipoPistao tipoPistao;
    [SerializeField]
    int intPistaoState;
    [SerializeField]
    PistaoState pistaoState;
    [SerializeField]
    TrackingScript trackingScript;

    public PistaoState PistaoState { get => pistaoState; set => pistaoState = value; }
    public int IntPistaoState { get => intPistaoState; set => intPistaoState = value; }

    private void Start()
    {
        articulation = GetComponent<ArticulationBody>();

        drive.upperLimit = maxValue;
        drive.lowerLimit = lowerLimit;
        drive.stiffness = stiffness;
        drive.damping = damping;
        drive.lowerLimit = lowerLimit;
        drive.forceLimit = forceLimit;

        CheckTheArticulationDrive();


        trackingScript.PistaoValueChanged += TrackingScript_PistaoValueChanged;

    }

    private void TrackingScript_PistaoValueChanged(char arg1, string arg2)
    {
        if (tipoPistao== TipoPistao.Horizontal)
        {
            if (arg1 == nomeDoPistao)
            {
                if (arg2 == "01")
                {
                    pistaoState = PistaoState.ParadoInicio;
                }
                else if (arg2 == "10")
                {
                    pistaoState = PistaoState.FechadoFinal;
                }
            }
        }
      
    }

    private void CheckTheArticulationDrive()
    {
        if (driveState == Drive.XDrive)
        {
            articulation.xDrive = drive;
        }

        if (driveState == Drive.YDrive)
        {
            articulation.yDrive = drive;
        }
        if (driveState == Drive.ZDrive)
        {
            articulation.zDrive = drive;
        }
    }

    private void Update()
    {
        //

        if (pistaoState == PistaoState.ParadoInicio)
        {
            drive.target = 0;
            CheckTheArticulationDrive();
        }
        else if (pistaoState == PistaoState.FechadoFinal)
        {
            drive.target = 1;
            CheckTheArticulationDrive();
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
