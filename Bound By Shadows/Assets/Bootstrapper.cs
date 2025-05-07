using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    void Start()
    {
        if (!SceneManager.GetSceneByName("MainMenu").isLoaded)
        {
            SceneManager.LoadScene("MainMenu", LoadSceneMode.Additive);
        }
    }
}
