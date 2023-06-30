using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class XREtiquetaSocket : XRSocketInteractor
{
    [SerializeField] GameObject objetoConectado;

    protected override void Awake()
    {
        base.Awake();
    }

    public override bool CanHover(IXRHoverInteractable interactable)
    {
        return base.CanHover(interactable) && interactable.transform.TryGetComponent(out ToolEtiqueta etiqueta);
    }

    public override bool CanSelect(IXRSelectInteractable interactable)
    {
        return base.CanSelect(interactable) && interactable.transform.TryGetComponent(out ToolEtiqueta etiqueta);
    }

    protected override void OnSelectEntered(SelectEnterEventArgs interactable)
    {
        base.OnSelectEntered(interactable);
        if (interactable == null) { return; }
        if (interactable.interactableObject.transform.TryGetComponent(out ToolEtiqueta etiqueta))
        {
            etiqueta.Conectar(objetoConectado);
        }
    }
}
