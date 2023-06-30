using UnityEngine;
using UnityEngine.Events;

namespace VREnergy.VR.Headset
{
    public class HeadsetRaycast : MonoBehaviour
    {
        [Header("Camera")]
        [SerializeField] private Camera cam;
        
        [Header("Ray Settings")]
        [SerializeField] private float rayDistance = 5f;
        [SerializeField] private float raySphereRadius = 0.1f;
        [SerializeField] private LayerMask targetLayerMask;

        [Header("Events")]
        [SerializeField] private UnityEvent onHoverEnter;
        [SerializeField] private UnityEvent onHoverExit;

        [Header("Debug")] 
        [SerializeField] private bool enableDebug = true;

        private IHeadsetTargetable _currentHeadsetTargetable;

        #region MONOBEHAVIOUR

        private void Awake()
        {
            if (cam == null)
            {
                cam = Camera.main;
            }
        }

        private void Update()
        {
            CheckForInteractable();
        }

        #endregion
        
        public void Interact()
        {
            if (_currentHeadsetTargetable != null)
            {
                if (_currentHeadsetTargetable.CanInteract)
                {
                    _currentHeadsetTargetable.OnInteract(gameObject);
                }
            }
        }

        public void CheckForInteractable()
        {
            Ray ray = new Ray(cam.transform.position, cam.transform.forward);
            RaycastHit hitInfo;

            bool hitInteractable =
                Physics.SphereCast(ray, raySphereRadius, out hitInfo, rayDistance, targetLayerMask);

#if UNITY_EDITOR
            if (enableDebug)
            {
                Debug.DrawRay(ray.origin, ray.direction * rayDistance, hitInteractable ? Color.green : Color.red);
            }
#endif
            
            if (hitInteractable)
            {
                IHeadsetTargetable headsetTargetable = hitInfo.transform.GetComponent<IHeadsetTargetable>();

                if (headsetTargetable != null)
                {
                    if (!headsetTargetable.CanHover)
                    {
                        if (_currentHeadsetTargetable == headsetTargetable)
                        {
                            onHoverExit?.Invoke();
                            _currentHeadsetTargetable.OnHoverExit();
                            _currentHeadsetTargetable = null;
                        }
                        return;
                    }
                    
                    if (_currentHeadsetTargetable == null)
                    {
                        _currentHeadsetTargetable = headsetTargetable;
                        headsetTargetable.OnHoverEnter();
                        onHoverEnter?.Invoke();
                    }
                    else if (_currentHeadsetTargetable != headsetTargetable)
                    {
                        onHoverExit?.Invoke();
                        _currentHeadsetTargetable.OnHoverExit();
                        _currentHeadsetTargetable = headsetTargetable;
                        headsetTargetable.OnHoverEnter();
                        onHoverExit?.Invoke();
                    }
                }
                else
                {
                    _currentHeadsetTargetable?.OnHoverExit();
                    onHoverExit?.Invoke();
                    _currentHeadsetTargetable = null;
                }
            }
            else
            {
                _currentHeadsetTargetable?.OnHoverExit();
                onHoverExit?.Invoke();
                _currentHeadsetTargetable = null;
            }
        }
    }
}
