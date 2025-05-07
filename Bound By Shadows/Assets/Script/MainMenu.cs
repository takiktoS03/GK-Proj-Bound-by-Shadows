using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Level 1 - Cave", LoadSceneMode.Single);
    }

    public void LoadGame()
    {
        Debug.Log("[UI] LoadGame clicked");
        StartCoroutine(LoadAndRestore());
    }

    private IEnumerator LoadAndRestore()
    {
        Debug.Log("[LOAD] Coroutine started");

        SceneManager.LoadScene("Level 1 - Cave", LoadSceneMode.Single);

        // Poczekaj 0.5–1 sekundy zanim szukasz obiektu
        yield return new WaitForSeconds(4f);

        var found = GameObject.FindObjectOfType<PlayerSaveData>();
        Debug.Log(found == null
            ? "[LOAD]  NIE znaleziono PlayerSaveData po 1s!"
            : $"[LOAD]  Znaleziono PlayerSaveData: {found.name}");

        SaveSystem.LoadCurrentScene();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit called");
    }
}
