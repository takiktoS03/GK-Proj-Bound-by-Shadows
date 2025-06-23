using UnityEngine;

/* Zapewnia, ¿e w scenie znajduje siê tylko jeden aktywny AudioListener.
   - Usuwa siê, jeœli inny listener ju¿ istnieje.
   - Pozostaje miêdzy scenami (DontDestroyOnLoad).

   Autor: Julia Bigaj
*/

public class PersistentAudioListener : MonoBehaviour
{
    private void Awake()
    {
        // Usuwa ten listener, jeœli ju¿ istnieje inny (bezpiecznie)
        if (FindObjectsOfType<AudioListener>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}
