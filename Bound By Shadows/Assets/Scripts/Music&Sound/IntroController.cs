using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/**
 * @class IntroController
 * @brief Odpowiada za odtwarzanie sekwencji wprowadzenia do gry (intro).
 *
 * Skrypt pokazuje kolejne obrazy, odtwarza narracjê i dŸwiêki za pomoc¹ `MusicManager`,
 * a na koñcu wykonuje efekt przybli¿enia i zanikania ostatniego obrazu, po czym ³aduje scenê `"Level 1 - Cave"`.
 *
 * Przeznaczony do u¿ycia wy³¹cznie w scenie `"Intro"`.
 *
 * @author Julia Bigaj
 */
public class IntroController : MonoBehaviour
{
    /// @brief Obraz nr 1 wyœwietlany na pocz¹tku sekwencji.
    public GameObject image1;

    /// @brief Obraz nr 3 wyœwietlany w dalszej czêœci sekwencji.
    public GameObject image3;

    /// @brief Obraz nr 4 wyœwietlany w dalszej czêœci sekwencji.
    public GameObject image4;

    /// @brief Obraz nr 5 – koñcowy, na którym wykonywany jest efekt zoom i fade out.
    public GameObject image5;

    /// @brief Czas trwania efektu zoom.
    public float zoomDuration = 8f;

    /// @brief Wspó³czynnik przybli¿enia koñcowego obrazu.
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
     * @brief Rozpoczyna odtwarzanie sekwencji intro, jeœli aktywna scena to "Intro".
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
     * @brief Coroutine odpowiedzialna za przebieg ca³ej sekwencji wprowadzenia.
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
     * @brief Wykonuje efekt p³ynnego przybli¿enia obrazu.
     * @param target Transform obiektu do przybli¿enia.
     * @param fromScale Skala pocz¹tkowa.
     * @param toScale Skala docelowa.
     * @param duration Czas trwania przybli¿enia.
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
