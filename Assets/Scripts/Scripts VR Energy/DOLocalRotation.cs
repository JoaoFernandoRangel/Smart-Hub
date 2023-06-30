using System.Collections;
using DG.Tweening;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Options;
using UnityEngine;
using UnityEngine.Events;

public class DOLocalRotation : MonoBehaviour
{
    [SerializeField] private Transform objectToRotate;
    [SerializeField] private Vector3 targetRotation;
    [SerializeField] private bool invertRotation = true;
    [SerializeField] private float duration = 1f;
    [SerializeField] private float delay;
    [SerializeField] private Ease easeType = Ease.Linear;

    public UnityEvent onStart;
    public UnityEvent onComplete;

    private bool isRotating;
    private bool isInInitialState = true;
    private TweenerCore<Quaternion, Vector3, QuaternionOptions> tweener;

    public bool IsRotating => isRotating;
    public bool IsInInitialState => isInInitialState;

    private void Start()
    {
        if (objectToRotate == null)
        {
            Debug.LogError($"Object to Rotate is null.", this);
        }
    }

    public void PerformRotation()
    {
        if (isRotating) { return; }
        if (objectToRotate == null) { return; }
        StartCoroutine(RotateObject());
    }
    
    private IEnumerator RotateObject()
    {
        isRotating = true;
        
        yield return new WaitForSeconds(delay);
        
        onStart?.Invoke();

        if (invertRotation)
        {
            if (isInInitialState)
            {
                tweener = objectToRotate.DORotate(targetRotation, duration, RotateMode.LocalAxisAdd)
                    .SetEase(easeType)
                    .SetAutoKill(false);
            }
            else
            {
                tweener.PlayBackwards();
            }
        }
        else
        {
            tweener = objectToRotate.DORotate(targetRotation, duration)
                .SetEase(easeType);
        }
        
        while (tweener.IsPlaying())
        {
            yield return null;
        }

        isRotating = false;
        if (invertRotation)
            isInInitialState = !isInInitialState;
        onComplete?.Invoke();
    }
        
#if UNITY_EDITOR
    private void OnValidate()
    {
        duration = Mathf.Clamp(duration, .01f, float.MaxValue);
        delay = Mathf.Clamp(delay, 0f, float.MaxValue);
    }

    [ContextMenu("Debug Rotate")]
    private void DebugRotate()
    {
        PerformRotation();
    }
#endif
}

