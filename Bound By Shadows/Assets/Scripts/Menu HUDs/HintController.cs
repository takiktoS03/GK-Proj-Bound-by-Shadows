using UnityEngine;
using TMPro;

/**
 * @class HintController
 * @brief Zarządza wyświetlaniem tekstowych podpowiedzi w interfejsie użytkownika.
 *
 * Współpracuje z komponentami takimi jak `HintArea`, umożliwiając dynamiczne
 * pokazywanie i ukrywanie wiadomości dla gracza.
 *
 * @author Julia Bigaj
 */
public class HintController : MonoBehaviour
{
    /// @brief Referencja do komponentu TextMeshProUGUI wyświelającego podpowiedzi.
    public TextMeshProUGUI hintText;

    /**
     * @brief Wyświetla wiadomość podpowiedzi na ekranie.
     * @param message Tekst wiadomości do wyświetlenia.
     */
    public void ShowHint(string message)
    {
        if (hintText == null) return;

        hintText.text = message;
        hintText.gameObject.SetActive(true);
    }

    /**
     * @brief Ukrywa aktualnie wyświetlaną podpowiedź.
     */
    public void HideHint()
    {
        if (hintText == null) return;

        hintText.gameObject.SetActive(false);
    }
}

