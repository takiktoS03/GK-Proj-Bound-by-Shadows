using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class PauseMenu
 * @brief Zarz�dza stanem pauzy i ko�ca gry w trakcie rozgrywki.
 *
 * Obs�uguje wstrzymywanie i wznawianie gry, zapisywanie/wczytywanie stanu,
 * powr�t do menu g��wnego oraz ekran ko�ca gry. Blokuje czas gry i interakcje
 * UI w odpowiednich momentach. Dzia�a tylko w scenie `"Level 1 - Cave"`.
 *
 * @author Julia Bigaj
 */
public class PauseMenu : MonoBehaviour
{
    /// @brief Panel UI menu pauzy.
    public GameObject pauseMenuUI;

    /// @brief Czy gra zako�czy�a si� (np. przegrana).
    public static bool isGameOver = false;

    /// @brief Panel UI ekranu ko�ca gry.
    public GameObject gameOverUI;

    /// @brief �r�d�o d�wi�ku odtwarzanego przy przegranej.
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
     * @brief Obs�uguje skr�t klawiaturowy (Escape) i logik� pauzy.
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
     * @brief Wznawia gr� po pauzie.
     */
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    /**
     * @brief Wczytuje bie��c� scen� i odtwarza zapisany stan.
     */
    public void LoadGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * @brief Callback wykonywany po zako�czeniu �adowania sceny � uruchamia system zapisu.
     * @param scene Nowo za�adowana scena.
     * @param mode Tryb �adowania sceny.
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SaveSystem.LoadCurrentScene();
    }

    /**
     * @brief Wstrzymuje gr� i aktywuje menu pauzy.
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
     * @brief Ko�czy gr� i wraca do menu g��wnego.
     */
    public void QuitGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }

    /**
     * @brief Zapisuje bie��cy stan gry.
     */
    public void SaveGame()
    {
        SaveSystem.SaveCurrentScene();
    }

    /**
     * @brief Coroutine wy�wietlaj�ca ekran ko�ca gry po kr�tkim op�nieniu.
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
     * @brief Odtwarza d�wi�k ko�ca gry, je�li jeszcze nie gra.
     */
    private void PlayGameOverSound()
    {
        if (gameOverAudio != null && !gameOverAudio.isPlaying)
        {
            gameOverAudio.Play();
        }
    }
}
