using UnityEngine;
using UnityEngine.SceneManagement;

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
        Debug.Log("[UI] LoadGame clicked");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[LOAD] Scene loaded from PauseMenu: " + scene.name);
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

    public void ShowGameOver()
    {
        gameOverUI.SetActive(true);
        pauseMenuUI.SetActive(false);
        Time.timeScale = 0f;
        isPaused = true;
        isGameOver = true;
        PlayGameOverSound();
    }
    private void PlayGameOverSound()
    {
        if (gameOverAudio != null && !gameOverAudio.isPlaying)
        {
            gameOverAudio.Play();
        }
    }
}