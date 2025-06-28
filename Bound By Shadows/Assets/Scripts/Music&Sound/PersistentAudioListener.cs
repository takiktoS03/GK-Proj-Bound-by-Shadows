using UnityEngine;

/**
 * @class PersistentAudioListener
 * @brief Zapewnia, �e w scenie znajduje si� tylko jeden aktywny `AudioListener`.
 *
 * Skrypt usuwa si� automatycznie, je�li w scenie istnieje ju� inny `AudioListener`,
 * zapobiegaj�c konfliktom audio w Unity (komunikat: "There are 2 AudioListeners").
 * Ustawiony jako trwa�y mi�dzy scenami dzi�ki `DontDestroyOnLoad`.
 *
 * @author Julia Bigaj
 */
public class PersistentAudioListener : MonoBehaviour
{
    /**
     * @brief Sprawdza, czy istnieje ju� inny `AudioListener`. Je�li tak � usuwa siebie.
     */
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
