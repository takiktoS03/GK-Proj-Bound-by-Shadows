using UnityEngine;
using System.Collections;

public class Level1FadeIn : MonoBehaviour
{
    public CanvasGroup fadeOverlay;
    public float fadeDuration = 1f;

    void Start()
    {
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            fadeOverlay.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        fadeOverlay.alpha = 0f;
        gameObject.SetActive(false); // po fade in – wy³¹cz overlay
    }
}
