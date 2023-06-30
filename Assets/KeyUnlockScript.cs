using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyUnlockScript : MonoBehaviour
{
    public bool locked = true;
    [SerializeField]
    private string animatorTrigger= "Open";
    ActivatePROAction activatePRO;
    Animator animator;
    private void Start()
    {
        animator = GetComponent<Animator>();
        activatePRO=GetComponent<ActivatePROAction>();
    }
    public void ChangeLockedState(bool lo)
    {
        locked= lo;
    }
    public void Open()
    {
        if (!locked)
        {
            print("open");
            animator.SetTrigger(animatorTrigger);
            activatePRO.Ativar();
        }
    }
}
