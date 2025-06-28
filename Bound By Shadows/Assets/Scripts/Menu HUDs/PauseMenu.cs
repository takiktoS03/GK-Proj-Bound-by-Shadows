using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class PauseMenu
 * @brief Zarządza stanem pauzy i końca gry w trakcie rozgrywki.
 *
 * Obsługuje wstrzymywanie i wznawianie gry, zapisywanie/wczytywanie stanu,
 * powrót do menu głównego oraz ekran końca gry. Blokuje czas gry i interakcje
 * UI w odpowiednich momentach. Działa tylko w scenie `"Level 1 - Cave"`.
 *
 * @author Julia Bigaj
 */
public class PauseMenu : MonoBehaviour
{
    /// @brief Panel UI menu pauzy.
    public GameObject pauseMenuUI;

    /// @brief Czy gra zakończyła się (np. przegrana).
    public static bool isGameOver = false;

    /// @brief Panel UI ekranu końca gry.
    public GameObject gameOverUI;

    /// @brief Źródło dźwięku odtwarzanego przy przegranej.
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
     * @brief Obsługuje skrót klawiaturowy (Escape) i logikę pauzy.
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
     * @brief Wznawia grę po pauzie.
     */
    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    /**
     * @brief Wczytuje bieżącą scenę i odtwarza zapisany stan.
     */
    public void LoadGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    /**
     * @brief Callback wykonywany po zakończeniu ładowania sceny – uruchamia system zapisu.
     * @param scene Nowo załadowana scena.
     * @param mode Tryb ładowania sceny.
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
        SaveSystem.LoadCurrentScene();
    }

    /**
     * @brief Wstrzymuje grę i aktywuje menu pauzy.
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
     * @brief Kończy grę i wraca do menu głównego.
     */
    public void QuitGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;
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
     * @brief Odtwarza dźwięk końca gry, jeśli jeszcze nie gra.
     */
    private void PlayGameOverSound()
    {
        if (gameOverAudio != null && !gameOverAudio.isPlaying)
        {
            gameOverAudio.Play();
        }
    }
}

