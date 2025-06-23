using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

/* Ten skrypt zarz¹dza aktywnoœci¹ obiektów EventSystem w scenach Unity.
   Zapewnia, ¿e w danym momencie aktywny jest tylko jeden EventSystem — nadmiarowe zostaj¹ dezaktywowane lub zniszczone.
   Dziêki temu unika siê konfliktów podczas prze³¹czania scen lub ³adowania ich asynchronicznie.

   Autor: Julia Bigaj
*/

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
        if (EventSystem.current != null && EventSystem.current != GetComponent<EventSystem>())
        {
            Destroy(gameObject);
        }
    }
}
