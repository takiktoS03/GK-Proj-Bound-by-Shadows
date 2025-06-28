using UnityEngine;
using System.Collections;

/**
 * @class Level1FadeIn
 * @brief Realizuje efekt p³ynnego zanikania czarnego ekranu po uruchomieniu poziomu.
 *
 * Skrypt przy starcie uruchamia animacjê `fade-in` przy u¿yciu `CanvasGroup`, stopniowo ujawniaj¹c widok gry.
 * Po zakoñczeniu efektu wy³¹cza obiekt z komponentem `CanvasGroup`.
 *
 * Przeznaczony do u¿ytku jako efekt przejœcia przy rozpoczêciu sceny (np. poziomu 1).
 *
 * @author Julia Bigaj
 */
public class Level1FadeIn : MonoBehaviour
{
    /// @brief Referencja do `CanvasGroup`, który kontroluje przezroczystoœæ czarnego overlaya.
    public CanvasGroup fadeOverlay;

    /// @brief Czas trwania efektu zanikania (w sekundach).
    public float fadeDuration = 1f;

    /**
     * @brief Rozpoczyna efekt fade-in zaraz po za³adowaniu sceny.
     */
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    /**
     * @brief Coroutine odpowiedzialna za stopniowe zmniejszanie przezroczystoœci `fadeOverlay`.
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
        gameObject.SetActive(false); ///< Wy³¹cza obiekt z overlayem po zakoñczeniu animacji.
    }
}
