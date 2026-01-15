using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 * @class MainMenu
 * @brief Obsługuje przyciski menu głównego: rozpoczęcie gry, wczytanie stanu i wyjście.
 *
 * Klasa odpowiada za przechodzenie do odpowiednich scen oraz inicjalizację systemu zapisu
 * podczas wczytywania gry. Umożliwia również zakończenie działania aplikacji.
 *
 * @author Julia Bigaj
 */
public class MainMenu : MonoBehaviour
{
    [SerializeField] private string firstLevelSceneName = "DungeonSecondFloor";
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
            Debug.LogWarning("Brak zapisu gry!");
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

