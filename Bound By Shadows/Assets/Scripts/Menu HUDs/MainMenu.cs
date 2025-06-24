using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/* Obs³uguje przyciski menu g³ównego: rozpocznij grê, wczytaj grê, wyjdŸ.
   - Wczytuje scenê gry i inicjalizuje system zapisu przy wczytaniu.

   Autor: Julia Bigaj
*/

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        //SceneManager.LoadScene("Level 1 - Cave", LoadSceneMode.Single);
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }

    public void LoadGame()
    {
        Debug.Log("[UI] LoadGame clicked");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Level 1 - Cave", LoadSceneMode.Single);
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[LOAD] Scene loaded: " + scene.name);
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SaveSystem.LoadCurrentScene();
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit called");
    }
}
