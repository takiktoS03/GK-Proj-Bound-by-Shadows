using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrapper : MonoBehaviour
{
    void Start()
    {
        SceneManager.LoadScene("MainMenu", LoadSceneMode.Single);
    }
}