using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

/**
 * @class MainMenu
 * @brief Obs�uguje przyciski menu g��wnego: rozpocz�cie gry, wczytanie stanu i wyj�cie.
 *
 * Klasa odpowiada za przechodzenie do odpowiednich scen oraz inicjalizacj� systemu zapisu
 * podczas wczytywania gry. Umo�liwia r�wnie� zako�czenie dzia�ania aplikacji.
 *
 * @author Julia Bigaj
 */
public class MainMenu : MonoBehaviour
{
    /**
     * @brief Rozpoczyna now� gr�, �aduj�c scen� wprowadzaj�c�.
     *
     * Zamiast poziomu g��wnego �adowana jest scena `"Intro"`.
     */
    public void StartNewGame()
    {
        //SceneManager.LoadScene("Level 1 - Cave", LoadSceneMode.Single);
        SceneManager.LoadScene("Intro", LoadSceneMode.Single);
    }

    /**
     * @brief Wczytuje gr� i ustawia callback na zako�czenie �adowania sceny.
     *
     * Po za�adowaniu sceny `"Level 1 - Cave"` nast�puje automatyczne wywo�anie systemu zapisu.
     */
    public void LoadGame()
    {
        Debug.Log("[UI] LoadGame clicked");
        SceneManager.sceneLoaded += OnSceneLoaded;
        SceneManager.LoadScene("Level 1 - Cave", LoadSceneMode.Single);
    }

    /**
     * @brief Callback wywo�ywany po za�adowaniu sceny � inicjalizuje system zapisu.
     * @param scene Obiekt reprezentuj�cy nowo za�adowan� scen�.
     * @param mode Tryb �adowania sceny (Single/Additive).
     */
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log("[LOAD] Scene loaded: " + scene.name);
        SceneManager.sceneLoaded -= OnSceneLoaded;

        SaveSystem.LoadCurrentScene();
    }

    /**
     * @brief Zamyka aplikacj�.
     *
     * Wywo�uje `Application.Quit()` i wypisuje debug log.
     */
    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit called");
    }
}
