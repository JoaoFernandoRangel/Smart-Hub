using System.Collections.Generic;

namespace VREnergy.PRO.Model
{
    [System.Serializable]
    public class Procedure
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public int SceneId { get; set; }
        public Scene Scene { get; set; }
        public IEnumerable<Step> Requirements { get; set; }

        public Stage GetStage(bool notSequenceConstraint)
        {
            StageSequential stage = new StageSequential(Name, Description, Step.StepRequirementsToStageRequirements(Requirements));
            stage.notSequenceConstraint = notSequenceConstraint;
            return stage;
        }
    }
}
