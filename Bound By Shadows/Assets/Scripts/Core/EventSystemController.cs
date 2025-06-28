using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/**
 * @class EventSystemController
 * @brief Kontroluje istnienie tylko jednego aktywnego EventSystemu po zmianie sceny.
 * 
 * Skrypt zapobiega duplikowaniu EventSystemów po wczytywaniu scen i dba o ich poprawn¹ aktywacjê.
 * 
 * @author Julia Bigaj
 */
public class EventSystemController : MonoBehaviour
{
    /**
    * @brief Subskrybuje zdarzenie za³adowania sceny i wykonuje kontrolê EventSystemu.
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
    * @brief Usuwa subskrypcjê po dezaktywacji.
    */
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    /**
    * @brief Metoda wywo³ywana po za³adowaniu sceny.
    * @param scene Za³adowana scena.
    * @param mode Tryb ³adowania sceny.
    */
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateEventSystemState();
    }

    /**
     * @brief Sprawdza i usuwa zbêdny EventSystem z hierarchii, jeœli ju¿ istnieje aktywny.
     */
    void UpdateEventSystemState()
    {
        if (EventSystem.current != null && EventSystem.current != GetComponent<EventSystem>())
        {
            Destroy(gameObject);
        }
    }
}
