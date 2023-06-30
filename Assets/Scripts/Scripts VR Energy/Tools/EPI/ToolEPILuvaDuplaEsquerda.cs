using UnityEngine;
using UnityEngine.XR.Interaction.Toolkit;

public class ToolEPILuvaDuplaEsquerda : MonoBehaviour, ILeftHandSocket
{
    [SerializeField] private GameObject handModel;

    public GameObject HandModel => handModel;
    
    private XRGrabInteractable grabInteractable;
    private Renderer handRender;
    
    private void Awake()
    {
        grabInteractable = GetComponent<XRGrabInteractable>();
        handRender = GetComponentInChildren<Renderer>();
    }

    private void OnEnable()
    {
        grabInteractable.selectEntered.AddListener(OnSelectEnteredListener);
        grabInteractable.selectExited.AddListener(OnSelectExitedListener);
    }

    private void OnDisable()
    {
        grabInteractable.selectEntered.RemoveListener(OnSelectEnteredListener);
        grabInteractable.selectExited.RemoveListener(OnSelectExitedListener);
    }

    private void OnSelectEnteredListener(SelectEnterEventArgs interactor)
    {
        if (interactor.interactorObject is XRSocketInteractor)
        {
            handRender.enabled = false;
        }
    }

    private void OnSelectExitedListener(SelectExitEventArgs interactor)
    {
        if (interactor.interactorObject is XRSocketInteractor)
        {
            handRender.enabled = true;
        }
    }
}
