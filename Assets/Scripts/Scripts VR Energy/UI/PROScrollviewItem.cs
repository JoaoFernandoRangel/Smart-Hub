using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using VREnergy.PRO.Model;

namespace VREnergy.UI
{
    public class PROScrollviewItem : MonoBehaviour
    {
        [SerializeField] private TextMeshProUGUI nameText;
        [SerializeField] private Button button;
        
        public event Action<Procedure> OnClick;
        
        private Procedure _procedure;
        
        public void Initialize(Procedure procedure)
        {
            _procedure = procedure;
            nameText.text = procedure.Name;
            button.onClick.AddListener(() => OnClick?.Invoke(_procedure));
        }
    }
}
