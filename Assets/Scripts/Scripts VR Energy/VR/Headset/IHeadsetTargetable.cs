using UnityEngine;

namespace VREnergy.VR.Headset
{
    public interface IHeadsetTargetable
    {
        bool CanInteract { get; }
        bool CanHover { get; }
        void OnInteract(GameObject interactor);
        void OnHoverEnter();
        void OnHoverExit();
    }
}
