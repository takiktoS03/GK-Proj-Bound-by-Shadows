using UnityEngine;
using TMPro;

public class HintController : MonoBehaviour
{
    public TextMeshProUGUI hintText;

    public void ShowHint(string message)
    {
        if (hintText == null) return;

        hintText.text = message;
        hintText.gameObject.SetActive(true);
    }

    public void HideHint()
    {
        if (hintText == null) return;

        hintText.gameObject.SetActive(false);
    }
}
