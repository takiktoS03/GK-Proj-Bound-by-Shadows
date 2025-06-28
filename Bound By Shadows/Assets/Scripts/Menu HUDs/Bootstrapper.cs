using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class Bootstrapper
 * @brief £aduje sceny startowe gry przy uruchomieniu aplikacji.
 *
 * Skrypt sprawdza, czy scena inicjalizacyjna (`InitScene`) jest za³adowana.
 * Jeœli nie — ³aduje j¹ jako scenê dodatkow¹ (Additive), a nastêpnie ³aduje
 * scenê g³ówn¹ (`MainMenu`) jako podstawow¹ (Single).
 *
 * U¿ywany jako punkt wejœciowy aplikacji.
 *
 * @author Julia Bigaj
 */
public class Bootstrapper : MonoBehaviour
{
    /**
     * @brief Inicjuje ³adowanie scen startowych po uruchomieniu gry.
     *
     * £aduje InitScene (jeœli nie jest ju¿ za³adowana) i prze³¹cza do MainMenu.
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
