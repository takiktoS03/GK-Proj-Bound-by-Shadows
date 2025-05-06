using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    public void StartNewGame()
    {
        SceneManager.LoadScene("Level 1 - Cave");
    }

    public void LoadGame()
    {
        if (System.IO.File.Exists(Application.persistentDataPath + "/save.dat"))
        {
            string scene = PlayerPrefs.GetString("LastScene", "Level 1 - Cave");
            SceneManager.LoadScene(scene);
            StartCoroutine(LoadAfterSceneLoad());
        }
        else
        {
            Debug.LogWarning("Brak zapisu do wczytywania!");
        }
    }

    private IEnumerator LoadAfterSceneLoad()
    {
        yield return new WaitForSeconds(0.1f);
        Debug.Log("[MainMenu] Wczytywanie SaveManagera...");
        SaveManager.Instance.LoadGame();
    }


    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Exit called");
    }
}
