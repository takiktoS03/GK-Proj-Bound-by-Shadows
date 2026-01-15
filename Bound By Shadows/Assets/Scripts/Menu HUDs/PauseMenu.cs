using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class PauseMenu
 * @brief Zarządza stanem pauzy i końca gry w trakcie rozgrywki.
 */
public class PauseMenu : MonoBehaviour
{
    /// @brief Panel UI menu pauzy.
    public GameObject pauseMenuUI;

    /// @brief Czy gra zakończyła się (np. przegrana).
    public static bool isGameOver = false;

    /// @brief Panel UI ekranu końca gry.
    public GameObject gameOverUI;

    /// @brief Czy gra jest aktualnie zapauzowana.
    public static bool isPaused = false;

    /// @brief Flaga informująca nową scenę, że ma wczytać zapis po starcie.
    public static bool isReloading = false;

    [Header("Loading Screen")]
    // Przypisz tu ten czarny Panel, który stworzyłeś
    [SerializeField] private GameObject loadingOverlay;

    /**
     * @brief Inicjalizacja stanu gry i UI przy starcie.
     */
    void Start()
    {
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;

        // POPRAWKA: Sprawdzamy, czy scena została załadowana przez przycisk "Wczytaj" (Restart)
        //if (isReloading)
        //{
        //    if (loadingOverlay != null) loadingOverlay.SetActive(true);

        //    isReloading = false;
        //    StartCoroutine(LoadAfterOneFrame());
        //}
        //else
        //{
        //    // Jeśli to zwykłe wejście do poziomu (New Game / przejście z innego poziomu):
        //    // Wyłącz czarny ekran natychmiast
        //    if (loadingOverlay != null) loadingOverlay.SetActive(false);
        //}
    }

    /**
     * @brief Obsługuje skrót klawiaturowy (Escape) i logikę pauzy.
     */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameOver)
                return;

            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    /**
     * @brief Wznawia grę po pauzie.
     */
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    // USUNIĘTO: OnEnable, OnDisable i OnSceneLoaded - nie są potrzebne w tej klasie
    // i powodowały problemy przy restarcie tej samej sceny.

    /**
     * @brief Wczytuje bieżącą scenę i odtwarza zapisany stan.
     */
    public void LoadGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;
        UIStateManager.isUIOpen = false;

        gameOverUI.SetActive(false);
        pauseMenuUI.SetActive(false);
        // Ustawiamy flagę, żeby po załadowaniu sceny Start() wiedział, że ma wczytać save
        //isReloading = true;
        DestroyedRegistry.Load();

        // 3. Ustawiamy flagę, że jesteśmy w trakcie przeładowania zapisu
        isReloading = true;
        GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().name);
    }

    private IEnumerator LoadGameRoutine()
    {
        // 1 klatka – żeby scena się w pełni zainicjalizowała
        yield return null;
        DestroyedRegistry.Load();
        SaveSystem.LoadCurrentScene();
    }
    /**
     * @brief Wstrzymuje grę i aktywuje menu pauzy.
     */
    public void Pause()
    {
        SoundLibrary.Instance.StopSteps();
        if (gameOverUI.activeSelf)
            return;

        pauseMenuUI.SetActive(true);
        gameOverUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    /**
     * @brief Kończy grę i wraca do menu głównego.
     */
    public void QuitGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;
        isReloading = false; // Resetujemy flagę na wszelki wypadek
        SceneManager.LoadScene("MainMenu");
    }

    /**
     * @brief Zapisuje bieżący stan gry.
     */
    public void SaveGame()
    {
        SaveSystem.SaveCurrentScene();
    }

    /**
     * @brief Coroutine wyświetlająca ekran końca gry po krótkim opóźnieniu.
     */
    public IEnumerator ShowGameOver()
    {
        SoundLibrary.Instance.StopSteps();
        yield return new WaitForSeconds(1.5f);
        SoundLibrary.Instance.PlayGameOver();
        yield return new WaitForSeconds(0.4f);

        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        isGameOver = true;
    }
    private IEnumerator LoadAfterOneFrame()
    {
        yield return new WaitForSeconds(1f);

        // Ładujemy dane (przesuwamy postać)
        DestroyedRegistry.Load();
        SaveSystem.LoadCurrentScene();

        // Czekamy jeszcze jedną klatkę, aby kamera zdążyła przeskoczyć za graczem
        yield return null;

        // TERAZ odsłaniamy widok - gracz jest już na miejscu
        if (loadingOverlay != null)
            loadingOverlay.SetActive(false);
    }
}