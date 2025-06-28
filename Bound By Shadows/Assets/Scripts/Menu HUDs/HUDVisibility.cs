using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class HUDVisibility
 * @brief Ukrywa interfejs HUD w scenie menu g³ównego.
 *
 * Skrypt automatycznie dezaktywuje obiekt HUD, jeœli aktualnie aktywna scena to `"MainMenu"`.
 * Zapewnia, ¿e elementy interfejsu nie s¹ widoczne podczas przebywania w menu.
 *
 * @author Julia Bigaj
 */
public class HUDVisibility : MonoBehaviour
{
    /**
     * @brief Sprawdza aktywn¹ scenê i wy³¹cza HUD w menu g³ównym.
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
