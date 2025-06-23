using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

/* Dodaje i usuwa podkreœlenie tekstu TMP na przycisku podczas najechania kursorem.
   - Umo¿liwia wizualne wyró¿nienie aktywnego elementu interfejsu.

   Autor: Julia Bigaj
*/

public class ButtonHoverUnderline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public TextMeshProUGUI label;

    public void OnPointerEnter(PointerEventData eventData)
    {
        label.fontStyle |= FontStyles.Underline;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        label.fontStyle &= ~FontStyles.Underline;
    }
}
