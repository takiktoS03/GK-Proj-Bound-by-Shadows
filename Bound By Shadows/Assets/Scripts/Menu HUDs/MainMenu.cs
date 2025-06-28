using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 * @class MainMenu
 * @brief Obs³uguje przyciski menu g³ównego: rozpoczêcie gry, wczytanie stanu i wyjœcie.
 *
 * Klasa odpowiada za przechodzenie do odpowiednich scen oraz inicjalizacjê systemu zapisu
 * podczas wczytywania gry. Umo¿liwia równie¿ zakoñczenie dzia³ania aplikacji.
 *
 * @author Julia Bigaj
 */
public class MainMenu : MonoBehaviour
{
    /**
     * @brief Rozpoczyna now¹ grê, ³aduj¹c scenê wprowadzaj¹c¹.
     *
     * Zamiast poziomu g³ównego ³adowana jest scena `"Intro"`.
     */
    public void StartNewGame()
    {
        //SceneManager.LoadScene("Level 1 - Cave", LoadSceneMode.Single);
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }

    /**
     * @brief Wczytuje grê i ustawia callback na zakoñczenie ³adowania sceny.
     *
     * Po za³adowaniu sceny `"Level 1 - Cave"` nastêpuje automatyczne wywo³anie systemu zapisu.
     */
    public void LoadGame()
    {
        Debug.Log("[UI] LoadGame clicked");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Level 1 - Cave", LoadSceneMode.Single);
    }

    /**
     * @brief Callback wywo³ywany po za³adowaniu sceny – inicjalizuje system zapisu.
     * @param scene Obiekt reprezentuj¹cy nowo za³adowan¹ scenê.
     * @param mode Tryb ³adowania sceny (Single/Additive).
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[LOAD] Scene loaded: " + scene.name);
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SaveSystem.LoadCurrentScene();
    }

    /**
     * @brief Zamyka aplikacjê.
     *
     * Wywo³uje `Application.Quit()` i wypisuje debug log.
     */
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit called");
    }
}
