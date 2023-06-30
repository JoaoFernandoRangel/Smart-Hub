using System.Collections.Generic;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace VREnergy.PRO.Model
{
    public class Step
    {
        public string Id { get; set; }
        public string Description { get; set; }
        public IEnumerable<Step> Requirements { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        public ExecutionSequence ExecutionType { get; set; }
        public Action Action { get; set; }

        public Stage ToStage()
        {
            switch (ExecutionType)
            {
                case ExecutionSequence.Action: return ToAction();
                case ExecutionSequence.Sequential: return ToSequential();
                case ExecutionSequence.Parallel: return ToParallel();
                default: return null;
            }
        }

        private Stage ToParallel()
        {
            return new StageParallel(Id, Description, StepRequirementsToStageRequirements(Requirements));
        }

        private Stage ToSequential()
        {
            return new StageSequential(Id, Description, StepRequirementsToStageRequirements(Requirements));
        }

        private Stage ToAction()
        {    
            return new StageAction(Id, Description, new PROAction(Action.Activator, Action.Receptor, Action.Interaction));
        }

        public static IEnumerable<Stage> StepRequirementsToStageRequirements(IEnumerable<Step> stepRequirements)
        {
            List<Stage> stageRequirements = new List<Stage>();
            foreach (var requirement in stepRequirements)
            {
                stageRequirements.Add(requirement.ToStage());
            }

            return stageRequirements;
        }
    }

    public class Action
    {
        public string Activator { get; set; }
        public string Receptor { get; set; }
        public string Interaction { get; set; }
    }
}
