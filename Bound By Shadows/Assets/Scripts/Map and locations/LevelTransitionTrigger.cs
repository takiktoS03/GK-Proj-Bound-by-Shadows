using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelTransitionTrigger : MonoBehaviour
{
    [Header("Ustawienia przejscia")]
    [Tooltip("Nazwa sceny, do ktorej przechodzimy")]
    public string nextSceneName = "Cave";

    [Tooltip("Czas trwania efektu fade")]
    public float fadeDuration = 1.5f;

    private bool isTransitioning = false;
    private CanvasGroup fadeCanvas;

    private void Start()
    {
        // Szukamy obiektu CanvasGroup (np. czarnego overlay)
        fadeCanvas = FindObjectOfType<CanvasGroup>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTransitioning && other.CompareTag("Player"))
        {
            StartCoroutine(FadeAndLoadNextScene());
        }
    }

    private System.Collections.IEnumerator FadeAndLoadNextScene()
    {
        isTransitioning = true;

        // Jesli mamy CanvasGroup, to robimy efekt przyciemnienia
        if (fadeCanvas != null)
        {
            float elapsed = 0f;
            while (elapsed < fadeDuration)
            {
                elapsed += Time.deltaTime;
                fadeCanvas.alpha = Mathf.Lerp(0f, 1f, elapsed / fadeDuration);
                yield return null;
            }
        }

        yield return new WaitForSeconds(0.3f); // opcjonalnie

        SceneManager.LoadScene(nextSceneName);
    }
}
