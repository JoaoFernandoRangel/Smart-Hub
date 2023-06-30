using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using VREnergy.PRO.Model;
using System;
using System.Collections.Generic;
using System.Linq;
public class StageAction : Stage
{
    public PROAction Action { get; }
    public event System.Action OnActionComplete;

    public StageAction(string id, string description, PROAction action)
        : base(id, description, ExecutionSequence.Action)
    {
        Action = action;
    }

    public StageAction(string id, string description, string activator, string receptor, string interaction)
        : this(id, description, new PROAction(activator, receptor, interaction))
    { }

    public override int NewAction(PROAction otherAction, Stack<Stage> pathStages)
    {
        if (IsCorrectAction(otherAction) != null)
        {
            if (Status == StepStatus.Done)  // A step that has already been taken is not counted
                return 0;
            PercentComplete = 1f;
            OnActionComplete?.Invoke();
            Debug.Log($"Completou {Id}");
            Status = StepStatus.Done;
            return 1;
        }
        return -1;
    }

    public override Stack<Stage> IsCorrectAction(PROAction otherAction)
    {
        if (Action.Equals(otherAction))
        {
            var path = new Stack<Stage>();
            path.Push(this);
            return path;
        }
        //else if (action.IsInverse(otherAction) && status == STATUS_DONE)    // Undoing action
        //{
        //    SetStatus(STATUS_DOING);
        //}
        return null;
    }

    public override Stage CurrentStage()
    {
        return this;
    }

    public override float GetPercentComplete()
    {
        if (Status != StepStatus.Done)
            return 0f;
        else
            return PercentComplete;
    }
}
