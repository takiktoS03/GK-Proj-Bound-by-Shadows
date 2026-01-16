using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/**
 * Centralny menedżer gry odpowiedzialny za ładowanie scen,
 * przejścia wizualne oraz ustawianie pozycji gracza po zmianie sceny.
 *
 * @author Filip Kudła
 */
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    [Header("UI References")]
    public CanvasGroup fadeScreen;
    public Slider loadingBar;

    [HideInInspector] public string targetSpawnId;
    [HideInInspector] public bool IsLoading { get; private set; }

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        if (loadingBar != null) loadingBar.gameObject.SetActive(false);
        if (fadeScreen != null) fadeScreen.alpha = 0f;
    }

    private void Start()
    {
        if (SceneManager.GetActiveScene().name == "BootScene")
        {
            LoadLevel("MainMenu");
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (SaveSystem.loadOnSceneStart)
        {
            SaveSystem.LoadCurrentScene();
            SaveSystem.loadOnSceneStart = false;
        }
        if (!string.IsNullOrEmpty(targetSpawnId))
        {
            MovePlayerToSpawnPoint(targetSpawnId);
            targetSpawnId = null;
        }
    }

    private void MovePlayerToSpawnPoint(string spawnId)
    {
        SpawnPoint[] points = FindObjectsByType<SpawnPoint>(FindObjectsSortMode.None);
        foreach (var point in points)
        {
            if (point.spawnID == spawnId)
            {
                GameObject player = GameObject.FindGameObjectWithTag("Player");
                if (player != null)
                {
                    player.transform.position = point.transform.position;
                }
                break;
            }
        }
    }

    /// <summary>
    /// Główna metoda do zmiany sceny.
    /// </summary>
    /// <param name="sceneName">Nazwa sceny docelowej</param>
    /// <param name="fadeDuration">Czas ściemniania (domyślnie 1s)</param>
    public void LoadLevel(string sceneName, float fadeDuration = 1.0f)
    {
        if (IsLoading) return;
        StartCoroutine(LoadSceneRoutine(sceneName, fadeDuration));
    }

    private IEnumerator LoadSceneRoutine(string sceneName, float fadeDuration)
    {
        IsLoading = true;
        fadeScreen.blocksRaycasts = true; // Blokuje kliknięcia

        yield return FadeIn(fadeDuration).SetUpdate(true).WaitForCompletion();

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
        yield return new WaitForSeconds(0.2f);

        operation.allowSceneActivation = true;

        // Czekamy aż scena faktycznie się przełączy
        while (!operation.isDone) yield return null;

        if (loadingBar != null) loadingBar.gameObject.SetActive(false);

        yield return FadeOut(fadeDuration).SetUpdate(true).WaitForCompletion();
        fadeScreen.blocksRaycasts = false;
        IsLoading = false;
    }


    // Fade'y zwracają animację, żeby można było na nią czekać
    public Tween FadeOut(float fadeDuration = 1.0f)
    {
        return fadeScreen.DOFade(0f, fadeDuration).SetUpdate(true);
    }

    public Tween FadeIn(float fadeDuration = 1.0f)
    {
        return fadeScreen.DOFade(1f, fadeDuration).SetUpdate(true);
    }
}