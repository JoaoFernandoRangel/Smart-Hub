using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FallDetector : MonoBehaviour
{
    /*
    public float tiltThreshold = 30f; // Adjust the threshold angle as needed
    [SerializeField]
    UnityEvent copoCaiu;
    bool copoCaiuBool = false;
    private void Start()
    {
         copoCaiuBool = false;

    }
    private void Update()
    {
        Vector3 upDirection = transform.up;
        float currentAngle = Vector3.Angle(upDirection, Vector3.up);

        if (currentAngle > tiltThreshold)
        {
            if (!copoCaiuBool)
            {
                copoCaiu.Invoke();
                copoCaiuBool = true;
            }

            // Perform actions or raise events for tilting or falling object
        }
    }
    public void ActiveBool()
    {
        copoCaiuBool = false;

    }*/
    [SerializeField]
    UnityEvent caiu;
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Chao"))
        {
            caiu.Invoke();
            print("caiu.Invoke();");
        }
    }
}
