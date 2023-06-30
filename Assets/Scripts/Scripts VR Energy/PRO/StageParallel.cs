using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VREnergy.PRO.Model;

public class StageParallel : StageSet
{
    public int quantCheck;      // Number of requeriments needed

    public StageParallel(string id, string description, IEnumerable<Stage> requirements)
        : base(id, description, ExecutionSequence.Parallel, requirements)
    {
    }
    
    public override int NewAction(PROAction otherAction, Stack<Stage> pathStages)
    {
        int statusReq;
        Stage correctReq;

        if (pathStages == null)
        {
            pathStages = IsCorrectAction(otherAction);
            if (pathStages == null)     // If it's an error
            {
                return -1;
            }
        }

        pathStages.Pop();       // Removing the current stage from the stack
        // TESTE
        Debug.Log($"Chegou no paralelo");
        correctReq = pathStages.Peek();

        if (selectReqID >= 0)
        {
            if (correctReq != requirements[selectReqID])   // Check if the action has been interrupted
            {
                Debug.LogWarning("An interruption occurred in " + requirements[selectReqID].Id + " of " + Id);
                quantInterruptions++;
                SelectRequirement(GetReqID(correctReq));
            }
        }

        statusReq = correctReq.NewAction(otherAction, new Stack<Stage>(new Stack<Stage>(pathStages)));
        if (statusReq == 0 && correctReq.Status != StepStatus.Done)         // Selecting the requirement
        {
            SelectRequirement(GetReqID(correctReq));
            return 0;
        }
        else if (statusReq == 1)    // Checking if requirement is done
        {
            int newStatus = GetStatus();
            correctReq.SetCompleteness(GetRequirementCompleteness(pathStages.Peek()));
            SelectRequirement(-1);

            if (newStatus == 1)
            {
                SetCompleteness(1f);
            }
            return newStatus;
        }
        return -1;
    }

    public override Stack<Stage> IsCorrectAction(PROAction otherAction)
    {
        foreach (var requirement in requirements)
        {
            Stack<Stage> path = requirement.IsCorrectAction(otherAction);
            if (path != null)
            {
                path.Push(this);
                return path;
            }
        }
        // Algum requisito da etapa foi desfeito, então volta pra etapa de "Fazendo"
        if (Status == StepStatus.Done && GetStatus() != 1)
        {
            Status = StepStatus.Doing;
        }
        return null;
    }

    public override Stage CurrentStage()
    {
        if (selectReqID != -1)    // Se já tem um requerimento selecionado
        {
            return requirements[selectReqID].CurrentStage();
        }
        else if (RequirementsDone() == requirements.Count - 1)   // Selecionando o único que falta
        {
            foreach (var req in requirements)
            {
                if (req.Status != StepStatus.Done)
                    return req.CurrentStage();
            }
        }
        else      // Escolhendo por prioridade
        {
            float minWeight = Mathf.Infinity;
            int chosenReq = -1;
            for (int i = 0; i < requirements.Count; i++)
            {
                if (requirements[i].Status == StepStatus.Done)  // Quem já foi feito nãoé analisado
                    continue;
                if (requirements[i].Weight == minWeight)    // Se existe mais de um com o peso mínimo, ninguém é selecionado
                {
                    chosenReq = -1;
                }
                else if (requirements[i].Weight < minWeight)    // Se for o novo mínimo, é salvo
                {
                    chosenReq = i;
                    minWeight = requirements[i].Weight;
                }
            }
            if (chosenReq != -1)
                return requirements[chosenReq];
        }
        return this;    // Se não tiver um único requisito com prioridade ótima, é enviado esse ao invés do requerimento
    }

    public override float GetPercentComplete()
    {
        if (Status != StepStatus.Done)
            return 0f;

        else    // Parallel or sequential
        {
            float total = 0f;
            int quantDone = 0;
            foreach (var item in requirements)
            {
                if (quantCheck > 0 && quantDone > quantCheck)       // Se for um paralelo com um número menor se requisitos, somente a quantidade exigida é analisada
                    break;
                total += item.GetPercentComplete();
            }
            // Retornando o completeness geral, que é a média dos requisitos multiplicado pelo completness individual da etapa
            return (total / (quantCheck == 0 ? requirements.Count : quantCheck)) * PercentComplete;
        }
    }

    int GetStatus()
    {
        int quantDone = 0;
        foreach (var req in requirements)
        {
            if (req.Status == StepStatus.Done)
                quantDone++;
        }
        quantCheck = Mathf.Clamp(quantCheck, 0, requirements.Count);
        if ((quantCheck <= 0 && quantDone == requirements.Count) ||
            (quantCheck > 0 && quantDone >= quantCheck))
        {
            Status = StepStatus.Done;
            return 1;
        }
        return 0;
    }
}
