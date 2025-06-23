using UnityEngine;
using UnityEngine.SceneManagement;

/* £aduje scenê inicjalizacyjn¹ (`InitScene`) jako dodatkow¹ oraz menu g³ówne (`MainMenu`) jako g³ówn¹.
   - U¿ywany jako punkt startowy projektu po uruchomieniu gry.

   Autor: Julia Bigaj
*/

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