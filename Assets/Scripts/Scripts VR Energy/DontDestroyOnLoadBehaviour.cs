using UnityEngine;

namespace VREnergy
{
    public class DontDestroyOnLoadBehaviour : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}