using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

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
