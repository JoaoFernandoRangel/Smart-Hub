using TMPro;
using UnityEngine;
using VREnergy.PRO;
using VREnergy.VR.Headset;

namespace VREnergy.VR
{
    public class Relogio : MonoBehaviour
    {
        [Header("UI Elements")]
        [SerializeField] private TextMeshProUGUI title;
        [SerializeField] private TextMeshProUGUI description;
        [SerializeField] private TextMeshProUGUI footer;
        [SerializeField] private GameObject screen;

        private ProcedureStageHandler _procedureStageHandler;
        private float productThreshold = 0.75f;



        private void Update()
        {
            if (IsWatchOnCorrectRotation())
                OnHoverEnter();
            else
                OnHoverExit();
        }



        // ... Restante do código sem alterações ...
        public void OnHoverEnter()
        {
            screen.SetActive(true);
        }

        public void OnHoverExit()
        {
            screen.SetActive(false);
        }
        private bool IsWatchOnCorrectRotation()
        {
            return Vector3.Dot(Vector3.up, transform.up) >= productThreshold;
        }


    }
}
