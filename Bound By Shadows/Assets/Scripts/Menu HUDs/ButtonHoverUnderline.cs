using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

/**
 * @class ButtonHoverUnderline
 * @brief Dodaje i usuwa podkreślenie tekstu przycisku podczas najechania kursorem myszy.
 *
 * Skrypt wykorzystuje interfejsy `IPointerEnterHandler` i `IPointerExitHandler`, aby reagować na zdarzenia
 * interfejsu użytkownika i umożliwiać wizualne wyróżnienie aktywnego elementu.
 *
 * Autor: Julia Bigaj
 */
public class ButtonHoverUnderline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// @brief Referencja do komponentu tekstowego (TextMeshProUGUI), który będzie podkreślany.
    public TextMeshProUGUI label;

    /**
     * @brief Dodaje podkreślenie tekstu po najechaniu kursorem.
     * @param eventData Dane zdarzenia wskaźnika (myszy).
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        label.fontStyle |= FontStyles.Underline;
    }

    /**
     * @brief Usuwa podkreślenie tekstu po opuszczeniu obszaru przycisku przez kursor.
     * @param eventData Dane zdarzenia wskaźnika (myszy).
     */
    public void OnPointerExit(PointerEventData eventData)
    {
        label.fontStyle &= ~FontStyles.Underline;
    }
}

