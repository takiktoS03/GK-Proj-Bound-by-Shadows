using UnityEngine;
using TMPro;

/* Zarz¹dza wyœwietlaniem tekstowych podpowiedzi na UI.
   - Pokazuje wiadomoœæ przekazan¹ przez `HintArea`.
   - Chowa podpowiedŸ, gdy gracz opuœci strefê.

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
