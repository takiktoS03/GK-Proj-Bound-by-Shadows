using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class EndGameTrigger
 * @brief Obs�uguje zako�czenie gry po dotarciu gracza do punktu ko�cowego.
 *
 * Po wej�ciu gracza w trigger, rozpoczyna animacj� przyciemniania ekranu
 * (poprzez zmian� `alpha` komponentu `CanvasGroup`) i �aduje scen� g��wnego menu.
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

    /// @brief Nazwa sceny g��wnego menu, do kt�rej nast�puje powr�t po zako�czeniu gry.
    public string mainMenuSceneName = "MainMenu";

    /// @brief Czy aktualnie trwa proces przyciemniania.
    private bool isFading = false;

    /**
     * @brief Reaguje na wej�cie gracza w trigger ko�cowy.
     * @param other Obiekt koliduj�cy (np. gracz).
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFading && other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndLoadMenu());
        }
    }

    /**
     * @brief Wykonuje efekt przyciemnienia ekranu i �aduje scen� g��wnego menu.
     * @return Enumerator do obs�ugi coroutine.
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
