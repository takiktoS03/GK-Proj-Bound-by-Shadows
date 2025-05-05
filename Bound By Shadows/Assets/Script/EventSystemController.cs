using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class EventSystemController : MonoBehaviour
{
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
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
        // Aktywuj tylko jeœli EventSystem nale¿y do aktywnej sceny
        gameObject.SetActive(gameObject.scene == SceneManager.GetActiveScene());
    }
}
