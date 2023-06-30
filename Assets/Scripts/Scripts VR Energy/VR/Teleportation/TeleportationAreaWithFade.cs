using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

namespace VREnergy.VR.Teleportation
{
    public class TeleportationAreaWithFade : TeleportationArea
    {
        [SerializeField] private ScreenFade screenFade;

        protected override void Awake()
        {
            base.Awake();
            if (screenFade == null)
            {
                screenFade = FindObjectOfType<ScreenFade>();
            }
        }

        protected override void OnSelectExited(SelectExitEventArgs args)
        {
            if (teleportTrigger == TeleportTrigger.OnSelectExited)
            {
                var rayInteractor = args.interactorObject as XRRayInteractor;
                if (rayInteractor != null)
                {
                    if (rayInteractor.TryGetCurrent3DRaycastHit(out var raycastHit))
                    {
                        var isMyCollider = false;
                        foreach (var interactionCollider in colliders)
                        {
                            if (interactionCollider == raycastHit.collider)
                            {
                                isMyCollider = true;
                                break;
                            }
                        }

                        if (isMyCollider)
                        {
                            StartCoroutine(screenFade.FadeSequence(base.OnSelectExited, args));
                        }
                    }
                }
            }
        }
    }
}
