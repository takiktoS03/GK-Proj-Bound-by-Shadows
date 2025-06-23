using TMPro;
using System.Collections;
using UnityEngine;

public class ThoughtUI : MonoBehaviour
{
    public TextMeshProUGUI thoughtText;

    private Coroutine currentRoutine;

    public void ShowThought(string text, float duration = 3f)
    {
        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(ShowRoutine(text, duration));
    }

    private IEnumerator ShowRoutine(string text, float duration)
    {
        thoughtText.text = text;
        thoughtText.alpha = 1;

        yield return new WaitForSeconds(duration);

        thoughtText.alpha = 0;
        thoughtText.text = "";
    }
}
