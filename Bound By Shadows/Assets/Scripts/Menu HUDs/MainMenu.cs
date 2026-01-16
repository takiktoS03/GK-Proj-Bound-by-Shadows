using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 * Skrypt obsługujący logikę menu głównego gry, w tym rozpoczęcie nowej gry,
 * wczytanie zapisu oraz wyjście z aplikacji.
 *
 * @author Julia Bigaj
 */

public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevelSceneName = "Intro";
    // Intro Cave Dungeon DungeonSecondFloor AbilityCutScene

    /**
     * @brief Rozpoczyna nową grę, ładując scenę wprowadzającą.
     */
    public void StartNewGame()
    {
        SaveSystem.loadOnSceneStart = false;
        SessionDestroyedRegistry.Clear();
        GameManager.Instance.LoadLevel(firstLevelSceneName);
    }

    /**
     * @brief Wczytuje grę
     *
     * Po załadowaniu sceny następuje automatyczne wywołanie systemu zapisu w GameManager
     */
    public void LoadGame()
    {
        string sceneToLoad = SaveSystem.GetLastSavedScene();
        if (string.IsNullOrEmpty(sceneToLoad))
        {
            return;
        }
        SaveSystem.loadOnSceneStart = true;
        SaveSystem.restorePlayerPositionOnLoad = true;
        GameManager.Instance.LoadLevel(sceneToLoad);
    }

    /**
     * @brief Zamyka aplikację.
     */
    public void QuitGame()
    {
        Application.Quit();
    }
}

