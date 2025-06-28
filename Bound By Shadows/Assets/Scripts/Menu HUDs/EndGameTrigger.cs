using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class EndGameTrigger
 * @brief Obsługuje zakończenie gry po dotarciu gracza do punktu końcowego.
 *
 * Po wejściu gracza w trigger, rozpoczyna animację przyciemniania ekranu
 * (poprzez zmianę `alpha` komponentu `CanvasGroup`) i ładuje scenę głównego menu.
 *
 * Wymaga na scenie obiektu z czarnym `Image` (UI) oraz komponentem `CanvasGroup`.
 *
 * @author Julia Bigaj
 */
public class EndGameTrigger : MonoBehaviour
{
    /// @brief Referencja do `CanvasGroup` odpowiedzialnego za przyciemnienie ekranu.
    public CanvasGroup fadeOverlay;

    /// @brief Czas trwania animacji przyciemnienia.
    public float fadeDuration = 1.5f;

    /// @brief Nazwa sceny głównego menu, do której następuje powrót po zakończeniu gry.
    public string mainMenuSceneName = "MainMenu";

    /// @brief Czy aktualnie trwa proces przyciemniania.
    private bool isFading = false;

    /**
     * @brief Reaguje na wejście gracza w trigger końcowy.
     * @param other Obiekt kolidujący (np. gracz).
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFading && other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndLoadMenu());
        }
    }

    /**
     * @brief Wykonuje efekt przyciemnienia ekranu i ładuje scenę głównego menu.
     * @return Enumerator do obsługi coroutine.
     */
    private System.Collections.IEnumerator FadeAndLoadMenu()
    {
        isFading = true;

        float elapsed = 0f;
        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            fadeOverlay.alpha = Mathf.Clamp01(elapsed / fadeDuration);
            yield return null;
        }

        yield return new WaitForSeconds(0.3f); // opcjonalna przerwa

        SceneManager.LoadScene(mainMenuSceneName);
    }
}

