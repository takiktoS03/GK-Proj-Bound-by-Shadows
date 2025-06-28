using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class HUDVisibility
 * @brief Ukrywa interfejs HUD w scenie menu g��wnego.
 *
 * Skrypt automatycznie dezaktywuje obiekt HUD, je�li aktualnie aktywna scena to `"MainMenu"`.
 * Zapewnia, �e elementy interfejsu nie s� widoczne podczas przebywania w menu.
 *
 * @author Julia Bigaj
 */
public class HUDVisibility : MonoBehaviour
{
    /**
     * @brief Sprawdza aktywn� scen� i wy��cza HUD w menu g��wnym.
     */
    void Start()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (scene == "MainMenu")
        {
            gameObject.SetActive(false);
        }
    }
}
