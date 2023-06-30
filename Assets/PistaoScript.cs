using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistaoScript : MonoBehaviour
{
    [SerializeField]
    private GameObject pistonObject;

    [SerializeField]
    Animator pistonAnim;

    EventAnimationScript evasdent;
    public float speed = 5f;

    private Vector3 originalPosition;

    private bool isMovingForward = false;
    public int stateAtual;

    private void Start()
    {
        originalPosition = pistonObject.transform.position;

    }

    public void MoverParaFrente()
    {
        isMovingForward = true;
    }
    public void Mover()
    {
        isMovingForward = !isMovingForward;
    }

    public void MoverParaPosicaoOriginal()
    {
        isMovingForward = false;
    }

    private void Update()
    {
        // Get the current state information
        switch (stateAtual)
        {
            case 1:
                
                break;
            case 2:
                Mover();
                break;
            case 3:
                
                break;
            default:
                break;
        }
        if (isMovingForward)
        {
            float step = speed * Time.deltaTime;
            Vector3 targetPosition = new Vector3(pistonObject.transform.position.x, pistonObject.transform.position.y, pistonObject.transform.position.z + step);
            pistonObject.transform.position = Vector3.MoveTowards(pistonObject.transform.position, targetPosition, step);
        }
        else
        {
            float step = speed * Time.deltaTime;
            pistonObject.transform.position = Vector3.MoveTowards(pistonObject.transform.position, originalPosition, step);
        }
    }

}
