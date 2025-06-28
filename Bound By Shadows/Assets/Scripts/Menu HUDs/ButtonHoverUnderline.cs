using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

/**
 * @class ButtonHoverUnderline
 * @brief Dodaje i usuwa podkre�lenie tekstu przycisku podczas najechania kursorem myszy.
 *
 * Skrypt wykorzystuje interfejsy `IPointerEnterHandler` i `IPointerExitHandler`, aby reagowa� na zdarzenia
 * interfejsu u�ytkownika i umo�liwia� wizualne wyr�nienie aktywnego elementu.
 *
 * Autor: Julia Bigaj
 */
public class ButtonHoverUnderline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// @brief Referencja do komponentu tekstowego (TextMeshProUGUI), kt�ry b�dzie podkre�lany.
    public TextMeshProUGUI label;

    /**
     * @brief Dodaje podkre�lenie tekstu po najechaniu kursorem.
     * @param eventData Dane zdarzenia wska�nika (myszy).
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        label.fontStyle |= FontStyles.Underline;
    }

    /**
     * @brief Usuwa podkre�lenie tekstu po opuszczeniu obszaru przycisku przez kursor.
     * @param eventData Dane zdarzenia wska�nika (myszy).
     */
    public void OnPointerExit(PointerEventData eventData)
    {
        label.fontStyle &= ~FontStyles.Underline;
    }
}
