using System;
using TMPro;
using UnityEngine;
using VREnergy.PRO;
using VREnergy.VR.Headset;

namespace VREnergy.VR
{
    public class Relogio : MonoBehaviour, IHeadsetTargetable
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI footer;
        [SerializeField] private GameObject screen;

        private ProcedureStageHandler _procedureStageHandler;
    
        private float productThreshold = 0.75f;
    
        #region MONOBEHAVIOUR

        private void Awake()
        {
            _procedureStageHandler = DependencyContainer.Instance.Get<ProcedureStageHandler>();
            if (_procedureStageHandler != null)
            {
                _procedureStageHandler.OnStageChanged += UpdateDisplay;
                _procedureStageHandler.OnProcedureStart += UpdateDisplay;
                _procedureStageHandler.OnProcedureFinish += SetDescriptionToFinish;
            }
        }

        private void OnEnable()
        {
            screen.SetActive(false);

            UpdateDisplay();
        }

        #endregion

        public void UpdateDisplay()
        {
            UpdateDisplay(_procedureStageHandler.GetCurrentStage());
        }

        private void UpdateDisplay(Stage currentStage)
        {
            if (currentStage == null)
            {
                ClearDisplay();
                return;
            }
            
            StageSet proStageSet = (StageSet)_procedureStageHandler.StageProcedure;
            
            title.text = _procedureStageHandler.StageProcedure.Description;
            description.text = currentStage.Description;
            footer.text = $"Passo {proStageSet.GetSelectedRequirement() + 1} de {proStageSet.Requirements.Count}";
        }

        private void ClearDisplay()
        {
            title.text = "";
            description.text = "";
            footer.text = "";
        }
        
        private void SetDescriptionToFinish()
        {
            description.text = "PRO finalizado.";
        }

        public bool CanInteract => true;
        public bool CanHover => IsWatchOnCorrectRotation();

        public void OnInteract(GameObject interactor)
        {
        
        }

        public void OnHoverEnter()
        {
            screen.SetActive(true);
        }

        public void OnHoverExit()
        {
            screen.SetActive(false);
        }
    
        /// <summary>
        /// Diz se o relógio está na rotação certa para poder ligar a tela.
        /// </summary>
        /// <returns></returns>
        private bool IsWatchOnCorrectRotation()
        {
            // Veja https://roystan.net/media/tutorials/DotProductVisualization.gif para observar o funcionamento do Dot Product.
            // Verifica a diferença entre o Y axis Global com o Y axis do relógio.
            // Caso a diferença for tolerável, será possível ligar o relógio.
            return Vector3.Dot(Vector3.up, transform.up) >= productThreshold;
        }
    }
}
