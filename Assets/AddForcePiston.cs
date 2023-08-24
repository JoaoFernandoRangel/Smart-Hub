using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddForcePiston : MonoBehaviour
{

    [SerializeField] private Vector3 firstPosition;
    [SerializeField] private Vector3 secondPosition;
    [SerializeField] Rigidbody rb;
    [SerializeField] float force = 1000;
    [SerializeField] bool isFirstState = true;
    [SerializeField] Vector3 forceDirection;
    [SerializeField] int state;
    private void FixedUpdate()
    {
        IntState(state);
        Vector3 targetPosition = isFirstState ? firstPosition : secondPosition;
        Vector3 forceDirection = (targetPosition - transform.position).normalized;
        rb.AddForce(forceDirection * force, ForceMode.Force);
    }
    [ContextMenu("ChangeState")]
    public void ChangeState()
    {
        isFirstState = !isFirstState;
    }
    public void IntState(int state)
    {
        if (state == 1)
        {
            isFirstState = true;
        }
        else if (state == 3)
        {
            isFirstState = false;
        }
    }
}
