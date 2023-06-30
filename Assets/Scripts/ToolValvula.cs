using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VREnergy.PRO;

public class ToolValvula : MonoBehaviour
{
    private Animator animator;
    [SerializeField]
    string activator;
    [SerializeField]
    string receptor;
    [SerializeField]
    States states;
    [SerializeField]
    bool receptorIsObject = true;
    private void Start()
    {
        animator = GetComponent<Animator>();
    }
    public void Ativar()
    {
        
        if (animator)
        {
            animator.SetBool("girar", true);
        }

        if (receptorIsObject)
        {
            FindObjectOfType<ProcedureStageHandler>().NewAction(new PROAction
            {
                Activator = activator,
                Receptor = GetComponent<PROAsset>().UnityId,
                Interaction = states.ToString()
            });
        }
        else
        {
            FindObjectOfType<ProcedureStageHandler>().NewAction(new PROAction
            {
                Activator = activator,
                Receptor = receptor,
                Interaction = states.ToString()
            });
        }
        
        
    }
}
