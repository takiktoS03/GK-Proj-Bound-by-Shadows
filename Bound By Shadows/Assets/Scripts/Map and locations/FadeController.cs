using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class FadeController : MonoBehaviour
{
    public static FadeController instance;

    public Image fadePanel;
    public float fadeSpeed = 1f;

    private void Awake()
    {
        instance = this;
    }

    public void FadeOutIn(System.Action actionAfterFadeOut)
    {
        StartCoroutine(FadeSequence(actionAfterFadeOut));
    }

    IEnumerator FadeSequence(System.Action afterFadeOut)
    {
        yield return StartCoroutine(Fade(1f));

        yield return new WaitForSeconds(1f);

        afterFadeOut?.Invoke();

        yield return StartCoroutine(Fade(0f));
    }

    IEnumerator Fade(float targetAlpha)
    {
        float startAlpha = fadePanel.color.a;

        while (!Mathf.Approximately(startAlpha, targetAlpha))
        {
            startAlpha = Mathf.MoveTowards(startAlpha, targetAlpha, fadeSpeed * Time.deltaTime);
            fadePanel.color = new Color(0, 0, 0, startAlpha);

            yield return null;
        }
    }
}
