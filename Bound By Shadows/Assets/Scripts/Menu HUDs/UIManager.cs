using UnityEngine;

/* Singleton odpowiedzialny za zarz¹dzanie elementami interfejsu u¿ytkownika w ca³ej grze.
   - Przenoszony miêdzy scenami (DontDestroyOnLoad).

   Autor: Julia Bigaj
*/

public class UIManager : MonoBehaviour
{
    public static UIManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject); 
    }
}
