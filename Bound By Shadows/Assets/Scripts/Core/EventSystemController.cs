using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/**
 * @class EventSystemController
 * @brief Kontroluje istnienie tylko jednego aktywnego EventSystemu po zmianie sceny.
 * 
 * Skrypt zapobiega duplikowaniu EventSystemów po wczytywaniu scen i dba o ich poprawną aktywację.
 * 
 * @author Julia Bigaj
 */
public class EventSystemController : MonoBehaviour
{
    /**
    * @brief Subskrybuje zdarzenie załadowania sceny i wykonuje kontrolę EventSystemu.
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
    * @brief Usuwa subskrypcję po dezaktywacji.
    */
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /**
    * @brief Metoda wywoływana po załadowaniu sceny.
    * @param scene Załadowana scena.
    * @param mode Tryb ładowania sceny.
    */
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateEventSystemState();
    }

    /**
     * @brief Sprawdza i usuwa zbędny EventSystem z hierarchii, jeśli już istnieje aktywny.
     */
    void UpdateEventSystemState()
    {
        if (EventSystem.current != null && EventSystem.current != GetComponent<EventSystem>())
        {
            Destroy(gameObject);
        }
    }
}

