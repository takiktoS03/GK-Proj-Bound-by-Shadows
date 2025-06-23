using UnityEngine;
using UnityEngine.SceneManagement;

/* Ukrywa HUD, je�li aktualnie za�adowan� scen� jest `MainMenu`.
   - Zapewnia czysty interfejs w menu g��wnym.

   Autor: Julia Bigaj
*/

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
