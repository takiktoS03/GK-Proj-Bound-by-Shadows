using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class EndGameTrigger
 * @brief Obs³uguje zakoñczenie gry po dotarciu gracza do punktu koñcowego.
 *
 * Po wejœciu gracza w trigger, rozpoczyna animacjê przyciemniania ekranu
 * (poprzez zmianê `alpha` komponentu `CanvasGroup`) i ³aduje scenê g³ównego menu.
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

    /// @brief Nazwa sceny g³ównego menu, do której nastêpuje powrót po zakoñczeniu gry.
    public string mainMenuSceneName = "MainMenu";

    /// @brief Czy aktualnie trwa proces przyciemniania.
    private bool isFading = false;

    /**
     * @brief Reaguje na wejœcie gracza w trigger koñcowy.
     * @param other Obiekt koliduj¹cy (np. gracz).
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFading && other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndLoadMenu());
        }
    }

    /**
     * @brief Wykonuje efekt przyciemnienia ekranu i ³aduje scenê g³ównego menu.
     * @return Enumerator do obs³ugi coroutine.
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
