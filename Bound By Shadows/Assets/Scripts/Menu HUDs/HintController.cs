using UnityEngine;
using TMPro;

/* Zarz�dza wy�wietlaniem tekstowych podpowiedzi na UI.
   - Pokazuje wiadomo�� przekazan� przez `HintArea`.
   - Chowa podpowied�, gdy gracz opu�ci stref�.

   Autor: Julia Bigaj
*/

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
