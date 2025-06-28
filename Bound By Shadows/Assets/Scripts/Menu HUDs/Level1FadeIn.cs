using UnityEngine;
using System.Collections;

/**
 * @class Level1FadeIn
 * @brief Realizuje efekt płynnego zanikania czarnego ekranu po uruchomieniu poziomu.
 *
 * Skrypt przy starcie uruchamia animację `fade-in` przy użyciu `CanvasGroup`, stopniowo ujawniając widok gry.
 * Po zakończeniu efektu wyłącza obiekt z komponentem `CanvasGroup`.
 *
 * Przeznaczony do użytku jako efekt przejścia przy rozpoczęciu sceny (np. poziomu 1).
 *
 * @author Julia Bigaj
 */
public class Level1FadeIn : MonoBehaviour
{
    /// @brief Referencja do `CanvasGroup`, który kontroluje przezroczystość czarnego overlaya.
    public CanvasGroup fadeOverlay;

    /// @brief Czas trwania efektu zanikania (w sekundach).
    public float fadeDuration = 1f;

    /**
     * @brief Rozpoczyna efekt fade-in zaraz po załadowaniu sceny.
     */
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    /**
     * @brief Coroutine odpowiedzialna za stopniowe zmniejszanie przezroczystości `fadeOverlay`.
     * @return Enumerator wymagany przez `StartCoroutine`.
     */
    IEnumerator FadeIn()
    {
        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            fadeOverlay.alpha = Mathf.Lerp(1f, 0f, elapsed / fadeDuration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        fadeOverlay.alpha = 0f;
        gameObject.SetActive(false); ///< Wyłącza obiekt z overlayem po zakończeniu animacji.
    }
}

