using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VREnergy.PRO;
using VREnergy.PRO.Model;
using VREnergy.SceneBuilder;
using VREnergy.SceneManagement;
using VREnergy.VR;

namespace VREnergy.UI
{
    public class PROTutorialMenuView : UIView
    {
        [SerializeField] private PROScrollviewItem proItemPrefab;
        [SerializeField] private Transform scrollviewContent;
        [SerializeField] private TextMeshProUGUI infoText;
        [SerializeField] private Button startButton;

        private Procedure _selectedProcedure;
        private ProcedureBuilder _procedureBuilder;
        private SceneLoader _sceneLoader;
        private TextMeshProUGUI _startButtonText;

        private void Awake()
        {
            _procedureBuilder = FindObjectOfType<ProcedureBuilder>();
            _sceneLoader = FindObjectOfType<SceneLoader>();
            _startButtonText = startButton.GetComponentInChildren<TextMeshProUGUI>();
        }

        public override void Initialize(UIViewManager context)
        {
            base.Initialize(context);
            var procedures = DependencyContainer.Instance.Get<IProcedureService>().ListProcedures();

            foreach (var procedure in procedures)
            {
                PROScrollviewItem item = Instantiate(proItemPrefab, scrollviewContent);
                item.Initialize(procedure);
                item.OnClick += SetCurrentProcedure;
            }

            startButton.onClick.AddListener(StartProcedure);
            startButton.interactable = false;
        }

        private void SetCurrentProcedure(Procedure procedure)
        {
            SetInfoText(procedure);
            _selectedProcedure = procedure;
            
            startButton.interactable = true;
        }

        private void SetInfoText(Procedure procedure)
        {
            infoText.text = procedure.Description;
        }

        private void StartProcedure()
        {
            StartCoroutine(StartProcedureCoroutine());
        }

        private IEnumerator StartProcedureCoroutine()
        {
            yield return null;
            startButton.interactable = false;
            _startButtonText.text = "Carregando...";
            
            _procedureBuilder.ConstructProcedure(_selectedProcedure.Id);
            
            Debug.Log(_selectedProcedure, this);
        }
    }
}
