using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class AnimationController : MonoBehaviour
{
    Animator animator;
    [Header("Referencias")]
    [SerializeField]
    TMP_Text velocidade;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }

    public void ChangeSpeed(float speed)
    {
        animator.speed = speed;
        velocidade.text = speed.ToString(); 
    }

}
