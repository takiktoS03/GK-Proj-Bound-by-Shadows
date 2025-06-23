using UnityEngine;
using UnityEngine.SceneManagement;

/* Ukrywa HUD, jeœli aktualnie za³adowan¹ scen¹ jest `MainMenu`.
   - Zapewnia czysty interfejs w menu g³ównym.

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
