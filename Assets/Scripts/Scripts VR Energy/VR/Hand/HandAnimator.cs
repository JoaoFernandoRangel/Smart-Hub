using UnityEngine;

namespace VREnergy.VR.Hand
{
    public abstract class HandAnimator : MonoBehaviour
    {
        [SerializeField] 
        protected Animator animator;

        protected virtual void Awake()
        {
            if (animator == null)
            {
                animator = GetComponentInChildren<Animator>();
            }
        }
    
        protected virtual void Update()
        {
            AnimateHand();
        }

        protected virtual void AnimateHand()
        {
            if (animator == null) { return; }
        }
    }
}