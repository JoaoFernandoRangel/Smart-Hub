using System;
using TMPro;
using UnityEngine;
using VREnergy.PRO;

namespace VREnergy.Tools
{
    public class ToolTelevisao : MonoBehaviour
    {
        [SerializeField]
        private TextMeshProUGUI title;
        [SerializeField]
        private TextMeshProUGUI description;
        [SerializeField]
        private TextMeshProUGUI footer;
        
        private ProcedureStageHandler _procedureStageHandler;
        
        private void Awake()
        {
            _procedureStageHandler = DependencyContainer.Instance.Get<ProcedureStageHandler>();
            if (_procedureStageHandler != null)
            {
                _procedureStageHandler.OnStageChanged += UpdateTelevisionDescription;
                _procedureStageHandler.OnProcedureStart += SetTelevisionTitle;
                _procedureStageHandler.OnProcedureFinish += SetDescriptionToFinish;
            }
        }

        private void UpdateTelevisionDescription(Stage currentStage)
        {
            description.text = currentStage.Description;
            
            try
            {
                StageSet proStageSet = (StageSet)_procedureStageHandler.StageProcedure;
                footer.text = "Passo " + (proStageSet.GetSelectedRequirement() + 1) + " de " + proStageSet.Requirements.Count;
            }
            catch
            {
                footer.text = "";
            }
        }

        private void SetTelevisionTitle()
        {
            title.text = _procedureStageHandler.StageProcedure.Description;
        }

        private void SetDescriptionToFinish()
        {
            description.text = "PRO finalizado.";
        }
    }
}