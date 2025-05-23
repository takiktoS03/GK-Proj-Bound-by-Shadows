using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    void Start()
    {
        if (!SceneManager.GetSceneByName("InitScene").isLoaded)
        {
            SceneManager.LoadScene("InitScene", LoadSceneMode.Additive);
        }
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}