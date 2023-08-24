using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PistaoScript : MonoBehaviour
{
    public Animator animator;
    public GameObject pistao;
    public float speed = 1.0f;
    [SerializeField]

    private int situacao = 1;
    private Vector3 posicaoOriginal;
    [SerializeField]
    private Vector3 posicaoMaxima;

    private Vector3 posicaoMinima;

    void Start()
    {
        // Salva a posição original do pistão
        posicaoOriginal = pistao.transform.position;
    }

    void Update()
    {
        if (animator != null && animator.isActiveAndEnabled)
        {
            AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0);

            string currentStateName = stateInfo.shortNameHash.ToString();

            Debug.Log("Current State: " + currentStateName);

            if (situacao == 2)
            {
                MovePistao(posicaoMaxima);
            }
            else if (situacao == 3)
            {
                MovePistao(posicaoMinima);
            }
            else if (situacao == 1)
            {
                MovePistao(posicaoOriginal);
            }
        }
    }
   
    public void SetSituacao(int value)
    {
        situacao = value;
    }
    public void SetPosicaoMaxima(Vector3 position)
    {
        posicaoMaxima = position;
    }

    public void SetPosicaoMinima(Vector3 position)
    {
        posicaoMinima = position;
    }
    public void Set()
    {
        SetPosicaoMaxima(posicaoMaxima);
    }
    public void MovePistao(Vector3 targetPosition)
    {
        // Move o pistão em direção à posição alvo usando interpolação linear
        float step = speed * Time.deltaTime;
        pistao.transform.position = Vector3.Lerp(pistao.transform.position, targetPosition, step);
    }
}


