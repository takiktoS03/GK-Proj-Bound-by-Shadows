using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class PauseMenu
 * @brief Zarzπdza stanem pauzy i koÒca gry w trakcie rozgrywki.
 *
 * Obs≥uguje wstrzymywanie i wznawianie gry, zapisywanie/wczytywanie stanu,
 * powrÛt do menu g≥Ûwnego oraz ekran koÒca gry. Blokuje czas gry i interakcje
 * UI w odpowiednich momentach. Dzia≥a tylko w scenie `"Level 1 - Cave"`.
 *
 * @author Julia Bigaj
 */
public class PauseMenu : MonoBehaviour
{
    /// @brief Panel UI menu pauzy.
    public GameObject pauseMenuUI;

    /// @brief Czy gra zakoÒczy≥a siÍ (np. przegrana).
    public static bool isGameOver = false;

    /// @brief Panel UI ekranu koÒca gry.
    public GameObject gameOverUI;

    /// @brief èrÛd≥o düwiÍku odtwarzanego przy przegranej.
    public AudioSource gameOverAudio;

    /// @brief Czy gra jest aktualnie zapauzowana.
    public static bool isPaused = false;

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
    }

    /**
     * @brief Obs≥uguje skrÛt klawiaturowy (Escape) i logikÍ pauzy.
     */
    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Level 1 - Cave")
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameOver)
                return;

            if (ChestPanelManager.Instance != null && ChestPanelManager.Instance.IsChestOpen())
            {
                ChestPanelManager.Instance.CloseChest();
                return;
            }

            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    /**
     * @brief Wznawia grÍ po pauzie.
     */
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    /**
     * @brief Wczytuje bieøπcπ scenÍ i odtwarza zapisany stan.
     */
    public void LoadGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * @brief Callback wykonywany po zakoÒczeniu ≥adowania sceny ñ uruchamia system zapisu.
     * @param scene Nowo za≥adowana scena.
     * @param mode Tryb ≥adowania sceny.
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SaveSystem.LoadCurrentScene();
    }

    /**
     * @brief Wstrzymuje grÍ i aktywuje menu pauzy.
     */
    public void Pause()
    {
        if (gameOverUI.activeSelf)
            return;

        pauseMenuUI.SetActive(true);
        gameOverUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    /**
     * @brief KoÒczy grÍ i wraca do menu g≥Ûwnego.
     */
    public void QuitGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }

    /**
     * @brief Zapisuje bieøπcy stan gry.
     */
    public void SaveGame()
    {
        SaveSystem.SaveCurrentScene();
    }

    /**
     * @brief Coroutine wyúwietlajπca ekran koÒca gry po krÛtkim opÛünieniu.
     * @return Enumerator dla StartCoroutine.
     */
    public IEnumerator ShowGameOver()
    {
        yield return new WaitForSeconds(1.3f);
        PlayGameOverSound();
        yield return new WaitForSeconds(0.4f);

        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
        isGameOver = true;
    }

    /**
     * @brief Odtwarza düwiÍk koÒca gry, jeúli jeszcze nie gra.
     */
    private void PlayGameOverSound()
    {
        if (gameOverAudio != null && !gameOverAudio.isPlaying)
        {
            gameOverAudio.Play();
        }
    }
}
