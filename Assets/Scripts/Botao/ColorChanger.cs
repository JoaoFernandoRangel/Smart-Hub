using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ColorChanger : MonoBehaviour
{
    public Material selectMaterial = null;

    private MeshRenderer meshRenderer = null;
    private XRBaseInteractable interactable = null;
    private Material originalMaterial = null;

    private void Awake()
    {
        meshRenderer = GetComponentInChildren<MeshRenderer>();
        originalMaterial = meshRenderer.material;

        interactable = GetComponent<XRBaseInteractable>();
        interactable.hoverEntered.AddListener(SetSelectMaterial);
        interactable.hoverExited.AddListener(SetOriginalMaterial);
    }

    private void OnDestroy()
    {
        interactable.hoverEntered.RemoveListener(SetSelectMaterial);
        interactable.hoverExited.RemoveListener(SetOriginalMaterial);
    }

    private void SetSelectMaterial(HoverEnterEventArgs interactor)
    {
        meshRenderer.material = selectMaterial;
    }

    private void SetOriginalMaterial(HoverExitEventArgs interactor)
    {
        meshRenderer.material = originalMaterial;
    }
}
