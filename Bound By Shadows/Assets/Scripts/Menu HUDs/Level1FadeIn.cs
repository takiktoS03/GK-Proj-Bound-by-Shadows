using UnityEngine;
using System.Collections;

/**
 * @class Level1FadeIn
 * @brief Realizuje efekt p�ynnego zanikania czarnego ekranu po uruchomieniu poziomu.
 *
 * Skrypt przy starcie uruchamia animacj� `fade-in` przy u�yciu `CanvasGroup`, stopniowo ujawniaj�c widok gry.
 * Po zako�czeniu efektu wy��cza obiekt z komponentem `CanvasGroup`.
 *
 * Przeznaczony do u�ytku jako efekt przej�cia przy rozpocz�ciu sceny (np. poziomu 1).
 *
 * @author Julia Bigaj
 */
public class Level1FadeIn : MonoBehaviour
{
    /// @brief Referencja do `CanvasGroup`, kt�ry kontroluje przezroczysto�� czarnego overlaya.
    public CanvasGroup fadeOverlay;

    /// @brief Czas trwania efektu zanikania (w sekundach).
    public float fadeDuration = 1f;

    /**
     * @brief Rozpoczyna efekt fade-in zaraz po za�adowaniu sceny.
     */
    void Start()
    {
        StartCoroutine(FadeIn());
    }

    /**
     * @brief Coroutine odpowiedzialna za stopniowe zmniejszanie przezroczysto�ci `fadeOverlay`.
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
        gameObject.SetActive(false); ///< Wy��cza obiekt z overlayem po zako�czeniu animacji.
    }
}
