using UnityEngine;
using UnityEngine.SceneManagement;

public class HUDVisibility : MonoBehaviour
{
    void Start()
    {
        string scene = SceneManager.GetActiveScene().name;
        if (scene == "MainMenu") // nazwa sceny menu
        {
            gameObject.SetActive(false); 
        }
    }
}
