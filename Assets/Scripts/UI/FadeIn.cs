using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeIn : MonoBehaviour
{
    public CanvasGroup canvasGroup;
    public float fadeTime;

    public void Start()
    {
        //canvasGroup = gameObject.AddComponent<CanvasGroup>() as CanvasGroup;
        /*
        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        fadeTime = 0.5f;
        */
    }

    public void OnEnable()
    {
        //canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.alpha = 0f;
        fadeTime = 0.5f;
        StartCoroutine(DoFadeIn(fadeTime, canvasGroup));
    }

    /*
    public void OnDisable()
    {
        canvasGroup.alpha = 0;
    }
    */

    public IEnumerator DoFadeIn(float fadeTime, CanvasGroup canvasGroup)//, CanvasGroup canvasGroup)
    {
        float elapsedTime = 0f;
        while (canvasGroup.alpha < 1)
        {
            elapsedTime += Time.deltaTime;
            canvasGroup.alpha = Mathf.Clamp01(elapsedTime / fadeTime);
            yield return null;
        }

        yield return null;
    }
}
