using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

/**
 * @class ButtonHoverUnderline
 * @brief Dodaje i usuwa podkreœlenie tekstu przycisku podczas najechania kursorem myszy.
 *
 * Skrypt wykorzystuje interfejsy `IPointerEnterHandler` i `IPointerExitHandler`, aby reagowaæ na zdarzenia
 * interfejsu u¿ytkownika i umo¿liwiaæ wizualne wyró¿nienie aktywnego elementu.
 *
 * Autor: Julia Bigaj
 */
public class ButtonHoverUnderline : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    /// @brief Referencja do komponentu tekstowego (TextMeshProUGUI), który bêdzie podkreœlany.
    public TextMeshProUGUI label;

    /**
     * @brief Dodaje podkreœlenie tekstu po najechaniu kursorem.
     * @param eventData Dane zdarzenia wskaŸnika (myszy).
     */
    public void OnPointerEnter(PointerEventData eventData)
    {
        label.fontStyle |= FontStyles.Underline;
    }

    /**
     * @brief Usuwa podkreœlenie tekstu po opuszczeniu obszaru przycisku przez kursor.
     * @param eventData Dane zdarzenia wskaŸnika (myszy).
     */
    public void OnPointerExit(PointerEventData eventData)
    {
        label.fontStyle &= ~FontStyles.Underline;
    }
}
