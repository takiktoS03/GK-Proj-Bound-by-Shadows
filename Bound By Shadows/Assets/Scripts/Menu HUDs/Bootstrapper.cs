using UnityEngine;
using UnityEngine.SceneManagement;

/* �aduje scen� inicjalizacyjn� (`InitScene`) jako dodatkow� oraz menu g��wne (`MainMenu`) jako g��wn�.
   - U�ywany jako punkt startowy projektu po uruchomieniu gry.

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