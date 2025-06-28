using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class Bootstrapper
 * @brief �aduje sceny startowe gry przy uruchomieniu aplikacji.
 *
 * Skrypt sprawdza, czy scena inicjalizacyjna (`InitScene`) jest za�adowana.
 * Je�li nie � �aduje j� jako scen� dodatkow� (Additive), a nast�pnie �aduje
 * scen� g��wn� (`MainMenu`) jako podstawow� (Single).
 *
 * U�ywany jako punkt wej�ciowy aplikacji.
 *
 * @author Julia Bigaj
 */
public class Bootstrapper : MonoBehaviour
{
    /**
     * @brief Inicjuje �adowanie scen startowych po uruchomieniu gry.
     *
     * �aduje InitScene (je�li nie jest ju� za�adowana) i prze��cza do MainMenu.
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
