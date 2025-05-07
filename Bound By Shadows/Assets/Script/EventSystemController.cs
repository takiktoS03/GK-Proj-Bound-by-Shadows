using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EventSystemController : MonoBehaviour
{
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

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        UpdateEventSystemState();
    }

    void UpdateEventSystemState()
    {
        // Aktywuj tylko je�li nie ma innego aktywnego EventSystemu
        if (EventSystem.current != null && EventSystem.current != GetComponent<EventSystem>())
        {
            gameObject.SetActive(false);
        }
        else
        {
            gameObject.SetActive(true);
        }
    }
}
