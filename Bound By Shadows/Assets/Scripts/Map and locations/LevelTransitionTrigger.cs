using UnityEngine;
using UnityEngine.SceneManagement;
using DG.Tweening;


/**
 * @class LevelTransitionTrigger
 * @brief Ładuje scenę po wejściu w trigger i płynnie przechodzi animacją pomiędzy scenami
 * Używa biblioteki DOTween do animacji: fade in, fade out
 *
 * @author Filip Kudła
 */
public class LevelTransitionTrigger : MonoBehaviour
{
    [Header("Ustawienia przejscia")]
    public string nextSceneName = "Cave";

    [Tooltip("Czas trwania efektu fade")]
    public float fadeDuration = 1.5f;

    [Tooltip("Canvas z czarnym tłem")]
    public CanvasGroup fadeCanvas;

    private bool isTransitioning = false;

    private void Start()
    {
        if (fadeCanvas != null)
        {
            fadeCanvas.alpha = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!isTransitioning && other.CompareTag("Player"))
        {
            StartTransition();
        }
    }

    private void StartTransition()
    {
        if (fadeCanvas == null)
        {
            Debug.LogError("Brak CanvasGroup do fade!");
            return;
        }

        isTransitioning = true;

        // Fade do czerni
        fadeCanvas.DOFade(1f, fadeDuration).SetUpdate(true).OnComplete(() =>
        {
            // Ładujemy scenę
            SceneManager.LoadScene(nextSceneName);

            // Po załadowaniu sceny — rozjaśnienie
            fadeCanvas.alpha = 1f; // nadal czarne po loadzie
            fadeCanvas.DOFade(0f, fadeDuration).SetUpdate(true);
        });
    }
}
