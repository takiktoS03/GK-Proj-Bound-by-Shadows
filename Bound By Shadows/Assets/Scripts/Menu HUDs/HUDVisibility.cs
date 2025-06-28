using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class HUDVisibility
 * @brief Ukrywa interfejs HUD w scenie menu głównego.
 *
 * Skrypt automatycznie dezaktywuje obiekt HUD, jeśli aktualnie aktywna scena to `"MainMenu"`.
 * Zapewnia, że elementy interfejsu nie są widoczne podczas przebywania w menu.
 *
 * @author Julia Bigaj
 */
public class HUDVisibility : MonoBehaviour
{
    /**
     * @brief Sprawdza aktywną scenę i wyłącza HUD w menu głównym.
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

