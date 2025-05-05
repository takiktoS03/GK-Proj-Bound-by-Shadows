using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Level 1 - Cave");
    }

    public void LoadGame()
    {
        Debug.Log("Wczytywanie gry... (do zaimplementowania)");
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit called");
    }
}
