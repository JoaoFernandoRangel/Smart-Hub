using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PistaoAnimatorScript : MonoBehaviour
{

    
    Animator anim;
    public float velocidade = 1.23f;
    [SerializeField]

    float velocidadeMaisRapida;
    [SerializeField]
    UnityEvent evento;
    [SerializeField]
    PistonForce pistonForce;
    
    public void Fire()
    {
        evento.Invoke();
    }
    private void Start()
    {
        
        anim = GetComponent<Animator>();
        anim.speed = velocidade;
        pistonForce.force = velocidade * 0.2f;
    }
    public void ajustarVelocidade(float valorDiminuir)
    {

        velocidade = valorDiminuir;
        if (velocidade <= 1)
        {
            velocidade = 0.8f;
        }
        pistonForce.force = velocidade * 0.02f;
        anim.speed = velocidade;
        if (velocidade > velocidadeMaisRapida)
        {
            velocidadeMaisRapida = velocidade;
        }
       
    }

}
