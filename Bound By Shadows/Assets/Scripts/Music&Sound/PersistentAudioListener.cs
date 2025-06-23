using UnityEngine;

/* Zapewnia, �e w scenie znajduje si� tylko jeden aktywny AudioListener.
   - Usuwa si�, je�li inny listener ju� istnieje.
   - Pozostaje mi�dzy scenami (DontDestroyOnLoad).

   Autor: Julia Bigaj
*/

public class PersistentAudioListener : MonoBehaviour
{
    private void Awake()
    {
        // Usuwa ten listener, je�li ju� istnieje inny (bezpiecznie)
        if (FindObjectsOfType<AudioListener>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
