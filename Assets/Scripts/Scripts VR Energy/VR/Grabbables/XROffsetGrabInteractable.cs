using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XROffsetGrabInteractable : XRGrabInteractable
{
    private Vector3 initialAttachLocalPos;
    private Quaternion initialAttachLocalRot;
    
    protected override void Awake()
    {
        base.Awake();

        CreateAttachPoint();
    }

    protected override void OnSelectEntering(SelectEnterEventArgs interactor)
    {
        if (interactor.interactorObject is XRDirectInteractor)
        {
            attachTransform.position = interactor.interactorObject.transform.position;
            attachTransform.rotation = interactor.interactorObject.transform.rotation;
        }
        else
        {
            attachTransform.localPosition = initialAttachLocalPos;
            attachTransform.localRotation = initialAttachLocalRot;
        }

        base.OnSelectEntering(interactor);
    }

    private void CreateAttachPoint()
    {
        if (!attachTransform)
        {
            GameObject grab = new GameObject("Grab Pivot");
            grab.transform.SetParent(transform, false);
            attachTransform = grab.transform;
        }

        initialAttachLocalPos = attachTransform.localPosition;
        initialAttachLocalRot = attachTransform.localRotation;
    }
}
