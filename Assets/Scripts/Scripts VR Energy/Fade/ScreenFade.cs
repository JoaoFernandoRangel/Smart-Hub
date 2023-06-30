using System;
using System.Collections;
using DG.Tweening;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.XR.Interaction.Toolkit;

public class ScreenFade : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField, Range(0, 5)] 
    private float duration = 0.25f;

    private Renderer _renderer;

    private void Awake()
    {
        _renderer = GetComponent<Renderer>();
        _renderer.material.SetFloat(FadeShaderProperty.alpha, 1);
    }

    private void Start()
    {
        FadeOut();
    }

    public float FadeIn()
    {
        _renderer.material.DOFloat(1, FadeShaderProperty.alpha, duration);
        return duration;
    }

    public float FadeOut()
    {
        _renderer.material.DOFloat(0, FadeShaderProperty.alpha, duration);
        return duration;
    }
    
    public IEnumerator FadeSequence(UnityAction<SelectExitEventArgs> actionEvent, SelectExitEventArgs args, Action onComplete = null)
    {
        float duration = FadeIn();

        yield return new WaitForSeconds(duration);
        actionEvent.Invoke(args);
        onComplete?.Invoke();

        FadeOut();
    }

    private struct FadeShaderProperty
    {
        public static readonly int alpha = Shader.PropertyToID("_Alpha");
    }
}
