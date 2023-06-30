using UnityEngine;
using UnityEngine.Events;
using VREnergy.PRO;

public class PortaPainel : MonoBehaviour
{
    //angle threshold to trigger if we reached limit
    public float angleBetweenThreshold = 0.5f;
    //State of the hinge joint : either reached min or max or none if in between
    public HingeJointState hingeJointState = HingeJointState.Min;
    
    //Event called on min reached
    public UnityEvent onMinLimitReached;
    //Event called on max reached
    public UnityEvent onMaxLimitReached;

    public bool IsDoorClosed => hingeJointState == HingeJointState.Min;
    
    public enum HingeJointState { Min, Max, None }
    private HingeJoint _hingeJoint;
    private Rigidbody _rigidbody;
    private float _angleWithMinLimit;
    private float _angleWithMaxLimit;

    private void Awake()
    {
        _hingeJoint = GetComponent<HingeJoint>();
        _rigidbody = GetComponent<Rigidbody>();
        
        onMaxLimitReached.AddListener(DoorFullyOpen);
        onMinLimitReached.AddListener(DoorFullyClosed);
    }

    private void FixedUpdate()
    {
        _angleWithMinLimit = Mathf.Abs(_hingeJoint.angle - _hingeJoint.limits.min);
        _angleWithMaxLimit = Mathf.Abs(_hingeJoint.angle - _hingeJoint.limits.max);
        
        //Reached Min
        if(_angleWithMinLimit < angleBetweenThreshold)
        {
            if (hingeJointState != HingeJointState.Min)
                onMinLimitReached.Invoke();

            hingeJointState = HingeJointState.Min;
        }
        //Reached Max
        else if (_angleWithMaxLimit < angleBetweenThreshold)
        {
            if (hingeJointState != HingeJointState.Max)
                onMaxLimitReached.Invoke();

            hingeJointState = HingeJointState.Max;
        }
        //No Limit reached
        else
        {
            hingeJointState = HingeJointState.None;
        }
    }

    public void LockDoor()
    {
        _rigidbody.isKinematic = true;
    }

    public void UnlockDoor()
    {
        _rigidbody.isKinematic = false;
    }

    private void DoorFullyOpen()
    {
        FindObjectOfType<ProcedureStageHandler>()?.NewAction(
            activator: "Operador",
            receptor: GetComponent<IPROAsset>().UnityId,
            interaction: States.Abrir.ToString()
        );
    }
    
    private void DoorFullyClosed()
    {
        FindObjectOfType<ProcedureStageHandler>()?.NewAction(
            activator: "Operador",
            receptor: GetComponent<IPROAsset>().UnityId,
            interaction: States.Fechar.ToString()
        );
    }
}
