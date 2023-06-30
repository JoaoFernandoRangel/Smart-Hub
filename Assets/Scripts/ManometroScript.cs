using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManometroScript : MonoBehaviour
{
    private Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Ativar()
    {
        print("Ativei");
        animator.SetBool("80Psi", true);
        
    }
}
