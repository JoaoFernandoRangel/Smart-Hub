using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;
using VREnergy.PRO.Model;

public abstract class StageSet : Stage
{
    protected readonly List<Stage> requirements;
    public readonly ReadOnlyCollection<Stage> Requirements;
    public int selectReqID = -1; // Used on parallel and sequential

    // Evaluationg performance
    public int quantInterruptions = 0;

    protected StageSet(string id, string description, ExecutionSequence type, IEnumerable<Stage> requirements)
        : base(id, description, type)
    {
        this.requirements = requirements.ToList();
        Requirements = this.requirements.AsReadOnly();
    }

    public int GetSelectedRequirement()
    {
        return selectReqID;
    }

    public void JumpRequirement()
    {
        SelectRequirement(selectReqID + 1);
        //if (selectReqID == -1)
        //{
        //    SetStatus(STATUS_DONE);
        //}
    }

    public void SelectRequirement(int reqId)
    {
        // TESTE
        //Debug.LogWarning("Selected " + reqId + " from " + ID + ", limite " + limite);

        if (reqId >= 0 && reqId < requirements.Count)
        {
            selectReqID = reqId;
            if (requirements[reqId].Status != StepStatus.Done)
            {
                //requirements[selectReqID].SetStatus(STATUS_DOING);
                requirements[selectReqID].Status = StepStatus.Doing;
            }
            else
            {
                JumpRequirement();
            }
        }
        else
        {
            selectReqID = -1;
        }
    }

    protected int GetReqID(Stage req)
    {
        for (int i = 0; i < requirements.Count; i++)
        {
            if (requirements[i] == req)
                return i;
        }
        Debug.LogError("Couldn't find requirement " + req.Id + " on " + Id + ".");
        return -1;
    }
    
    protected float GetRequirementCompleteness(Stage req)
    {
        float compMin = Mathf.Infinity;
        float completeness = 0f;

        if (req.Weight == 0f)
            return 1f;
        foreach (var item in requirements)         // Finding minimum weight
        {
            if (item.Status == StepStatus.Done && item != req)
                continue;

            if (item.Weight < compMin)
            {
                compMin = item.Weight;
            }
        }
        completeness = (1f - ((req.Weight - compMin) / req.Weight));
        completeness -= quantInterruptions * 0.05f;       // Applying interruptions penalties
        return Mathf.Clamp(completeness, 0f, 1f);
    }

    protected int RequirementsDone()
    {
        int count = 0;
        foreach (var req in requirements)
        {
            if (req.Status == StepStatus.Done)
                count++;
        }
        return count;
    }
}
