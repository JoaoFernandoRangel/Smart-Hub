using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardsTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private float speedInMPS;

    private void Update()
    {
        MoveObjectTowardsTarget();
    }

    private void MoveObjectTowardsTarget()
    {
        if (target == null)
        {
            Debug.LogWarning("Target is not set.");
            return;
        }

        Vector3 direction = (target.position - transform.position).normalized;
        float distanceToMove = speedInMPS * Time.deltaTime;

        if (distanceToMove >= Vector3.Distance(transform.position, target.position))
        {
            transform.position = target.position;
        }
        else
        {
            transform.position += direction * distanceToMove;
        }
    }
}
