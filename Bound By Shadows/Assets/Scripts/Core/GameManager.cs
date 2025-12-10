using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public CanvasGroup fadeScreen;
    public Slider loadingBar;

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (loadingBar != null) loadingBar.gameObject.SetActive(false);
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "BootScene")
        {
            LoadLevel("MainMenu");            
        }
    }

    /// <summary>
    /// Główna metoda do zmiany sceny.
    /// </summary>
    /// <param name="sceneName">Nazwa sceny docelowej</param>
    /// <param name="fadeDuration">Czas ściemniania (domyślnie 1s)</param>
    public void LoadLevel(string sceneName, float fadeDuration = 1.0f)
    {
        StartCoroutine(LoadSceneRoutine(sceneName, fadeDuration));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, float fadeDuration)
    {
        fadeScreen.blocksRaycasts = true; // Blokuje kliknięcia

        yield return FadeIn(fadeDuration).WaitForCompletion();

        if (loadingBar != null) loadingBar.gameObject.SetActive(true);

        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneName);
        operation.allowSceneActivation = false;

        // Pętla ładowania (dla loading baru lub długich poziomów)
        while (operation.progress < 0.9f)
        {
            if (loadingBar != null) loadingBar.value = operation.progress;
            yield return null;
        }

        // Chwila na "dobicie" paska do końca
        if (loadingBar != null) loadingBar.value = 1f;
        yield return new WaitForSeconds(0.1f); // Krótka pauza bezpieczeństwa

        operation.allowSceneActivation = true;

        // Czekamy aż scena faktycznie się przełączy
        while (!operation.isDone) yield return null;

        if (loadingBar != null) loadingBar.gameObject.SetActive(false);

        yield return FadeOut(fadeDuration).WaitForCompletion();
        fadeScreen.blocksRaycasts = false;
    }

    public Tween FadeOut(float fadeDuration = 1.0f)
    {
        // Zwraca animację, żeby można było na nią czekać
        return fadeScreen.DOFade(0f, fadeDuration).SetUpdate(true);
    }

    public Tween FadeIn(float fadeDuration = 1.0f)
    {
        return fadeScreen.DOFade(1f, fadeDuration).SetUpdate(true);
    }
}