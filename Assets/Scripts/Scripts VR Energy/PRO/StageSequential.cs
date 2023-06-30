using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VREnergy.PRO.Model;

public class StageSequential : StageSet
{
    public bool notSequenceConstraint = true;

    public StageSequential(string id, string description, IEnumerable<Stage> requirements)
        : base(id, description, ExecutionSequence.Sequential, requirements)
    {
    }
    
    public override int NewAction(PROAction otherAction, Stack<Stage> pathStages)
    {
        //Stage requirementMatched;
        if (selectReqID < 0)     // Initializing sequential stage
        {
            SelectRequirement(0);
            Status = StepStatus.Doing;
        }
        if (pathStages == null)
        {
            // Tenta aplicar a ação no passo esperado
            pathStages = IsCorrectAction(otherAction);
            if (pathStages == null)
            {
                // Se a ação não pertencer ao passo esperado, tenta com todos os outros passos
                if (notSequenceConstraint)
                {
                    // O Passo Sequencial tenta os subpassos
                    pathStages = IsAnotherStepCorrectAction(otherAction);
                    if (pathStages == null)
                    {
                        // Caso a ação não pertença a nenhum passo
                        return -1;
                    }
                    else
                    {
                        // Seleciona o passo que corresponde à ação do jogador
                        //SelectRequirement(GetReqID(pathStages.));
                    }
                }
                else
                {
                    return -1;
                }
            }
        }

        pathStages.Pop();       // Removing the current stage from the stack

        if (pathStages.Peek().NewAction(otherAction, pathStages) == 1)     // Checks if requirement has been completed
        {
            pathStages.Peek().SetCompleteness(GetRequirementCompleteness(requirements[selectReqID]));

            if (selectReqID == requirements.Count - 1)    // Checks if stage has been completed
            {
                Status = StepStatus.Done;
                SetCompleteness(1f);
                return 1;
            }
            else
            {
                SelectRequirement(selectReqID + 1);
            }
        }
        return 0;
    }

    public override Stack<Stage> IsCorrectAction(PROAction otherAction)
    {
        if (selectReqID < 0)
            return null;

        Stack<Stage> path = requirements[selectReqID].IsCorrectAction(otherAction);
        if (path != null)
        {
            path.Push(this);
            return path;
        }
        else
        {
            return null;
        }
    }

    public Stack<Stage> IsAnotherStepCorrectAction(PROAction otherAction)
    {
        for (int i = 0; i < requirements.Count; i++)
        {
            var requirement = requirements[i];
            var actionPath = requirement.IsCorrectAction(otherAction);
            if (actionPath != null)
            {
                Error($"Ação esperada era do passo {requirements[selectReqID].Id}, mas foi referente ao passo {requirement.Id}");
                quantInterruptions++;
                actionPath.Push(this);
                return actionPath;
            }
        }
        return null;
    }

    public override Stage CurrentStage()
    {
        if (selectReqID >= 0)
            return requirements[selectReqID].CurrentStage();
        else
            return this;
    }

    public override float GetPercentComplete()
    {
        if (Status != StepStatus.Done)
            return 0f;
        
        else    // Parallel or sequential
        {
            float total = 0f;
            foreach (var item in requirements)
            {
                total += item.GetPercentComplete();
            }
            // Retornando o completeness geral, que é a média dos requisitos multiplicado pelo completness individual da etapa
            return (total / requirements.Count) * PercentComplete;
        }
    }
}
