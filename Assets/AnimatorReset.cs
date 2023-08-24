using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimatorReset : MonoBehaviour
{
    public Animator animator1;
    public Animator animator2;
    public Animator animator3;
    public Animator animator4;

    public void PlayDefaultAnimations()
    {
        animator1.Play("Idle", 0, 0f);
        animator2.Play("Idle", 0, 0f);
        animator3.Play("idle", 0, 0f);
        animator4.Play("Iddle", 0, 0f);
        animator1.ResetTrigger("fire");
        animator2.ResetTrigger("fire");
        animator3.ResetTrigger("fire");
        animator4.ResetTrigger("detect");

    }
}
