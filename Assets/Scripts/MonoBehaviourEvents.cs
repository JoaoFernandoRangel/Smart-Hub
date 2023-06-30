using UnityEngine;
using UnityEngine.Events;

namespace VREnergy
{
    public class MonoBehaviourEvents : MonoBehaviour
    {
        [Header("Events")]
        [SerializeField] private UnityEvent onEnable;
        [SerializeField] private UnityEvent onDisable;

        private void OnEnable() => onEnable?.Invoke();

        private void OnDisable() => onDisable?.Invoke();
    }
}