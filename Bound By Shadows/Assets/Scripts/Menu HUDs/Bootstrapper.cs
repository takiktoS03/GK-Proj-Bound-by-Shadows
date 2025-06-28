using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class Bootstrapper
 * @brief Ładuje sceny startowe gry przy uruchomieniu aplikacji.
 *
 * Skrypt sprawdza, czy scena inicjalizacyjna (`InitScene`) jest załadowana.
 * Jeśli nie — ładuje ją jako scenę dodatkową (Additive), a następnie ładuje
 * scenę główną (`MainMenu`) jako podstawową (Single).
 *
 * Używany jako punkt wejściowy aplikacji.
 *
 * @author Julia Bigaj
 */
public class Bootstrapper : MonoBehaviour
{
    /**
     * @brief Inicjuje ładowanie scen startowych po uruchomieniu gry.
     *
     * Ładuje InitScene (jeśli nie jest już załadowana) i przełącza do MainMenu.
     */
    void Start()
    {
        if (!SceneManager.GetSceneByName("InitScene").isLoaded)
        {
            SceneManager.LoadScene("InitScene", LoadSceneMode.Additive);
        }

        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}

