using TMPro;
using System.Collections;
using UnityEngine;

public class TextUI : MonoBehaviour
{
    public TextMeshProUGUI[] dialogBoxes;

    private Coroutine currentRoutine;

    public void ShowTextDialog(string text, float duration = 3f, int boxIndex = 0)
    {
        if (boxIndex < 0 || boxIndex >= dialogBoxes.Length) return;

        if (currentRoutine != null)
            StopCoroutine(currentRoutine);
        currentRoutine = StartCoroutine(ShowRoutine(text, duration, dialogBoxes[boxIndex]));
    }

    private IEnumerator ShowRoutine(string text, float duration, TextMeshProUGUI box)
    {
        box.text = text;
        box.alpha = 1;

        yield return new WaitForSeconds(duration);

        box.alpha = 0;
        box.text = "";
    }
}
