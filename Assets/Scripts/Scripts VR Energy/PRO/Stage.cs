using System.Collections.Generic;
using UnityEngine;
using VREnergy.PRO.Model;

/// <summary>
/// Representação abstrata de um passo do PRO.
/// </summary>
[System.Serializable]
public abstract class Stage
{
    public string Id { get; set; }
    public string Description { get; set; }
    public ExecutionSequence Type { get; set; }
    public StepStatus Status { get; set; } = StepStatus.Todo;
    public float PercentComplete { get; set; }
    public float Weight { get; set; } = 1f;

    protected Stage(string id, string description, ExecutionSequence type)
    {
        Id = id;
        Description = description;
        Type = type;
    }

    /// <summary>
    /// Recieves an action and return the result.
    /// -1: Error; 0: Nothing; 1: State done.
    /// </summary>
    public abstract int NewAction(PROAction otherAction, Stack<Stage> pathStages = null);

    public abstract Stack<Stage> IsCorrectAction(PROAction otherAction);

    public abstract Stage CurrentStage();     // Return the selected stage

    public abstract float GetPercentComplete();

    public void SetCompleteness(float value)
    {
        if (PercentComplete == 0 || value < PercentComplete)
            PercentComplete = Mathf.Clamp01(value);
    }

    public void Error(string errorName)
    {
        Debug.Log($"New procedure error happened:\n {errorName}");
    }
}
