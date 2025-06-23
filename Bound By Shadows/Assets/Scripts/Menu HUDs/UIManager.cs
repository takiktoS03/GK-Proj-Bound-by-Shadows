using UnityEngine;

/* Singleton odpowiedzialny za zarz�dzanie elementami interfejsu u�ytkownika w ca�ej grze.
   - Przenoszony mi�dzy scenami (DontDestroyOnLoad).

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
