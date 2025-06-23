using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/* Zarz¹dza menu pauzy i ekranem koñca gry.
   - Obs³uguje pauzowanie, wznawianie, zapisywanie, wczytywanie i wyjœcie do menu g³ównego.
   - Zatrzymuje czas gry i blokuje interakcje UI w odpowiednich momentach.

   Autor: Julia Bigaj
*/

public class PauseMenu : MonoBehaviour
{
    public GameObject pauseMenuUI;
    public static bool isGameOver = false;
    public GameObject gameOverUI;
    public AudioSource gameOverAudio;
    public static bool isPaused = false;

    void Start()
    {
        pauseMenuUI.SetActive(false);
        gameOverUI.SetActive(false);
        isPaused = false;
        isGameOver = false;
        Time.timeScale = 1f;
    }

    void Update()
    {
        if (SceneManager.GetActiveScene().name != "Level 1 - Cave")
            return;

        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isGameOver)
                return;

            if (ChestPanelManager.Instance != null && ChestPanelManager.Instance.IsChestOpen()) {
                ChestPanelManager.Instance.CloseChest();
                return;
            } 

            if (isPaused)
                Resume();
            else
                Pause();
        }
    }

    public void Resume()
    {
        pauseMenuUI.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void LoadGame()
    {

        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SaveSystem.LoadCurrentScene();
    }

    public void Pause()
    {
        if (gameOverUI.activeSelf)
            return;

        pauseMenuUI.SetActive(true);
        gameOverUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void QuitGame()
    {
        Time.timeScale = 1f;
        isPaused = false;
        isGameOver = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void SaveGame()
    {
        SaveSystem.SaveCurrentScene();
    }

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
    private void PlayGameOverSound()
    {
        if (gameOverAudio != null && !gameOverAudio.isPlaying)
        {
            gameOverAudio.Play();
        }
    }
}