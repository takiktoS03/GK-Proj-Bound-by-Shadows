using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/**
 * @class EventSystemController
 * @brief Kontroluje istnienie tylko jednego aktywnego EventSystemu po zmianie sceny.
 * 
 * Skrypt zapobiega duplikowaniu EventSystem�w po wczytywaniu scen i dba o ich poprawn� aktywacj�.
 * 
 * @author Julia Bigaj
 */
public class EventSystemController : MonoBehaviour
{
    /**
    * @brief Subskrybuje zdarzenie za�adowania sceny i wykonuje kontrol� EventSystemu.
    */
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
        // natychmiastowa dezaktywacja nadmiarowego EventSystemu
        if (EventSystem.current != null && EventSystem.current != GetComponent<EventSystem>())
        {
            gameObject.SetActive(false);
            return;
        }

        UpdateEventSystemState();
    }

    /**
    * @brief Usuwa subskrypcj� po dezaktywacji.
    */
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /**
    * @brief Metoda wywo�ywana po za�adowaniu sceny.
    * @param scene Za�adowana scena.
    * @param mode Tryb �adowania sceny.
    */
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateEventSystemState();
    }

    /**
     * @brief Sprawdza i usuwa zb�dny EventSystem z hierarchii, je�li ju� istnieje aktywny.
     */
    void UpdateEventSystemState()
    {
        if (EventSystem.current != null && EventSystem.current != GetComponent<EventSystem>())
        {
            Destroy(gameObject);
        }
    }
}
