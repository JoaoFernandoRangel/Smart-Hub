using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class TextOpacity : MonoBehaviour
{
    public float fadeDuration = 2f; // Duration of the fade in seconds
    public TMP_Text textComponent;

    private CanvasGroup canvasGroup;
    [SerializeField]
    Color startColor;
    private void Start()
    {
             textComponent = GetComponent<TMP_Text>();

         canvasGroup = textComponent.GetComponent<CanvasGroup>();
        //textComponent.color = startColor;

    }
    [ContextMenu("FadeTextNow")]
    public void FadeTextNow()
    {
        StartCoroutine(FadeText());
    }
    private IEnumerator FadeText()
    {
        float elapsedTime = 0f;
        

        textComponent.color = startColor;

        while (elapsedTime < fadeDuration)
        {
            elapsedTime += Time.deltaTime;
            float normalizedTime = elapsedTime / fadeDuration;
            float targetOpacity = Mathf.Lerp(startColor.a, 0f, normalizedTime);

            Color newColor = textComponent.color;
            newColor.a = targetOpacity;
            textComponent.color = newColor;

            yield return null;
        }

        Color finalColor = textComponent.color;
        finalColor.a = 0f;
        textComponent.color = finalColor; 
    }
}
