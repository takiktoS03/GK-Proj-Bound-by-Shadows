using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/**
 * @class IntroController
 * @brief Odpowiada za odtwarzanie sekwencji wprowadzenia do gry (intro).
 *
 * Skrypt pokazuje kolejne obrazy, odtwarza narrację i dźwięki za pomocą `MusicManager`,
 * a na końcu wykonuje efekt przybliżenia i zanikania ostatniego obrazu, po czym ładuje scenę `"Level 1 - Cave"`.
 *
 * Przeznaczony do użycia wyłącznie w scenie `"Intro"`.
 *
 * @author Julia Bigaj
 */
public class IntroController : MonoBehaviour
{
    /// @brief Obraz nr 1 wyświetlany na początku sekwencji.
    public GameObject image1;

    /// @brief Obraz nr 3 wyświetlany w dalszej części sekwencji.
    public GameObject image3;

    /// @brief Obraz nr 4 wyświetlany w dalszej części sekwencji.
    public GameObject image4;

    /// @brief Obraz nr 5 – końcowy, na którym wykonywany jest efekt zoom i fade out.
    public GameObject image5;

    /// @brief Czas trwania efektu zoom.
    public float zoomDuration = 8f;

    /// @brief Współczynnik przybliżenia końcowego obrazu.
    public float zoomScale = 1.3f;

    /// @brief Oryginalna skala obrazu image5.
    private Vector3 originalScale;

    /**
     * @brief Dezaktywuje wszystkie obrazy przy uruchomieniu obiektu.
     */
    void Awake()
    {
        image1.SetActive(false);
        image3.SetActive(false);
        image4.SetActive(false);
        image5.SetActive(false);
    }

    /**
     * @brief Rozpoczyna odtwarzanie sekwencji intro, jeśli aktywna scena to "Intro".
     */
    void Start()
    {
        if (SceneManager.GetActiveScene().name != "Intro")
            return;

        MusicManager.Instance.StopMusic();
        MusicManager.Instance.PlayIntroAudio();

        StartCoroutine(PlayIntro());
    }

    /**
     * @brief Coroutine odpowiedzialna za przebieg całej sekwencji wprowadzenia.
     * @return Enumerator dla `StartCoroutine`.
     */
    IEnumerator PlayIntro()
    {
        image1.SetActive(true);
        yield return new WaitForSeconds(6f);
        image1.SetActive(false);

        image3.SetActive(true);
        yield return new WaitForSeconds(4f);
        image3.SetActive(false);

        image4.SetActive(true);
        yield return new WaitForSeconds(7f);
        image4.SetActive(false);

        image5.SetActive(true);
        originalScale = image5.transform.localScale;
        StartCoroutine(ZoomImage(image5.transform, originalScale, originalScale * zoomScale, zoomDuration));
        yield return new WaitForSeconds(7f);

        // fade out
        CanvasGroup cg = image5.GetComponent<CanvasGroup>();
        yield return StartCoroutine(FadeOutImage(cg, 3f));

        SceneManager.LoadScene("Level 1 - Cave");
    }

    /**
     * @brief Wykonuje efekt płynnego przybliżenia obrazu.
     * @param target Transform obiektu do przybliżenia.
     * @param fromScale Skala początkowa.
     * @param toScale Skala docelowa.
     * @param duration Czas trwania przybliżenia.
     * @return Enumerator dla `StartCoroutine`.
     */
    IEnumerator ZoomImage(Transform target, Vector3 fromScale, Vector3 toScale, float duration)
    {
        float elapsed = 0f;
        while (elapsed < duration)
        {
            target.localScale = Vector3.Lerp(fromScale, toScale, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }
        target.localScale = toScale;
    }

    /**
     * @brief Wykonuje efekt stopniowego zanikania obrazu (`CanvasGroup.alpha`).
     * @param canvasGroup CanvasGroup z przypisanym obrazem.
     * @param duration Czas trwania efektu zanikania.
     * @return Enumerator dla `StartCoroutine`.
     */
    IEnumerator FadeOutImage(CanvasGroup canvasGroup, float duration)
    {
        float startAlpha = canvasGroup.alpha;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            canvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / duration);
            elapsed += Time.deltaTime;
            yield return null;
        }

        canvasGroup.alpha = 0f;
    }
}

