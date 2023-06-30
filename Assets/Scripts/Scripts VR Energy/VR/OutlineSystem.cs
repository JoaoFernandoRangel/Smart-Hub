using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.Interaction;
using UnityEngine.XR.Interaction.Toolkit;

public class OutlineSystem : MonoBehaviour
{
    public void AddOutLine(XRBaseInteractable interactable)
    {
        Outline outline = interactable.gameObject.GetComponent<Outline>();
        if (!outline)
            outline = interactable.gameObject.AddComponent<Outline>();

        outline.OutlineWidth = 2;
        outline.OutlineMode = Outline.Mode.OutlineAll;
        outline.OutlineColor = Color.green;
    }
    public void RemoveOutLine(XRBaseInteractable interactable)
    {
        Outline outline = interactable.gameObject.GetComponent<Outline>();
        if (outline)
            Destroy(outline);
    }
}
