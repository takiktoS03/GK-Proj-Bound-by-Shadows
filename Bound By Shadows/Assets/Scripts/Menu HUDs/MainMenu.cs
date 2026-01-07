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
    /**
     * @brief Rozpoczyna nową grę, ładując scenę wprowadzającą.
     *
     * Zamiast poziomu głównego ładowana jest scena `"Intro"`.
     */
    public void StartNewGame()
    {
        SceneManager.LoadScene("Cave", LoadSceneMode.Single);
        //SceneManager.LoadScene("Intro", LoadSceneMode.Single);
        //SceneManager.LoadScene("Dungeon", LoadSceneMode.Single);
    }

    /**
     * @brief Wczytuje grę i ustawia callback na zakończenie ładowania sceny.
     *
     * Po załadowaniu sceny `"Level 1 - Cave"` następuje automatyczne wywołanie systemu zapisu.
     */
    public void LoadGame()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        //SceneManager.LoadScene("Dungeon", LoadSceneMode.Single);
        //SceneManager.LoadScene("Cave", LoadSceneMode.Single);
    }

    /**
     * @brief Callback wywoływany po załadowaniu sceny – inicjalizuje system zapisu.
     * @param scene Obiekt reprezentujący nowo załadowaną scenę.
     * @param mode Tryb ładowania sceny (Single/Additive).
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SaveSystem.LoadCurrentScene();
    }

    /**
     * @brief Zamyka aplikację.
     *
     */
    public void QuitGame()
    {
        Application.Quit();
    }
}

