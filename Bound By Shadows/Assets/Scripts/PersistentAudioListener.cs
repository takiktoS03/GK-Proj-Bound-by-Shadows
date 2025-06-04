using UnityEngine;

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
