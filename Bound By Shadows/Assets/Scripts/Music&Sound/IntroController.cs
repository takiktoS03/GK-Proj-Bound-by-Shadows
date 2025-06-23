using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

/* Obs³uguje wprowadzenie do gry w postaci sekwencji obrazów i narracji.
   - Odtwarza dŸwiêk, animacje i przejœcia.
   - Na koñcu automatycznie ³aduje poziom gry (`Level 1 - Cave`).

   Autor: Julia Bigaj
*/

public class IntroController : MonoBehaviour
{
    public GameObject image1;
    public GameObject image3;
    public GameObject image4;
    public GameObject image5;

    public float zoomDuration = 8f;
    public float zoomScale = 1.3f;

    private Vector3 originalScale;

    void Awake()
    {
        image1.SetActive(false);
        image3.SetActive(false);
        image4.SetActive(false);
        image5.SetActive(false);
    }

    void Start()
    { 
        if (SceneManager.GetActiveScene().name != "Intro")
            return;

        MusicManager.Instance.StopMusic();
        MusicManager.Instance.PlayIntroAudio();

        StartCoroutine(PlayIntro());
    }

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
        yield return StartCoroutine(FadeOutImage(cg, 3f)); // np. 1 sekunda fade out

        SceneManager.LoadScene("Level 1 - Cave");

    }

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

