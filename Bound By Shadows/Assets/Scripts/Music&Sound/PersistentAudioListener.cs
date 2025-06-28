using UnityEngine;

/**
 * @class PersistentAudioListener
 * @brief Zapewnia, że w scenie znajduje się tylko jeden aktywny `AudioListener`.
 *
 * Skrypt usuwa się automatycznie, jeśli w scenie istnieje już inny `AudioListener`,
 * zapobiegając konfliktom audio w Unity (komunikat: "There are 2 AudioListeners").
 * Ustawiony jako trwały między scenami dzięki `DontDestroyOnLoad`.
 *
 * @author Julia Bigaj
 */
public class PersistentAudioListener : MonoBehaviour
{
    /**
     * @brief Sprawdza, czy istnieje już inny `AudioListener`. Jeśli tak — usuwa siebie.
     */
    private void Awake()
    {
        // Usuwa ten listener, jeśli już istnieje inny (bezpiecznie)
        if (FindObjectsOfType<AudioListener>().Length > 1)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
    }
}

