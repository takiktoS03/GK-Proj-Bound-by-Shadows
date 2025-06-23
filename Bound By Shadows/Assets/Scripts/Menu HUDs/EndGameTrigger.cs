using UnityEngine;
using UnityEngine.SceneManagement;


/* Klasa odpowiedzialna za zako�czenie gry po dotarciu bohatera do punktu ko�cowego.
- Gdy gracz wejdzie w trigger, ekran zostaje przyciemniony (CanvasGroup alpha),
- Przyciemnienie kontrolowane jest przez `CanvasGroup`, kt�ry powinien by� czarnym obrazem z w��czonym `CanvasGroup`
 (np. UI -> Image z kolorem czarnym, bez sprite�a).

 Autor: Julia Bigaj
*/


public class EndGameTrigger : MonoBehaviour
{
    public CanvasGroup fadeOverlay;
    public float fadeDuration = 1.5f;
    public string mainMenuSceneName = "MainMenu";

    private bool isFading = false;

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isFading && other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndLoadMenu());
        }
    }

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
