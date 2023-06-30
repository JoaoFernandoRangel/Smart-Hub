using UnityEngine;

/*
 * Para remover os estranhos comportamentos do Rigidbody causados pelos collisores da porta
 * é preciso definir os valores via script.
 * 
 * Para mais informações veja:
 * https://docs.unity3d.com/ScriptReference/Rigidbody-inertiaTensor.html
 * https://docs.unity3d.com/ScriptReference/Rigidbody-inertiaTensorRotation.html
 * https://docs.unity3d.com/ScriptReference/Rigidbody-centerOfMass.html
 */
public class ResetPortaMeioRigidbodyComputedValues : MonoBehaviour
{
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void Start()
    {
        ResetPortaMeioRigidbodyValues();
    }

    private void ResetPortaMeioRigidbodyValues()
    {
        _rigidbody.inertiaTensor = Vector3.one;
        _rigidbody.inertiaTensorRotation = Quaternion.identity;
        _rigidbody.centerOfMass = Vector3.zero;
    }
}
