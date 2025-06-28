using UnityEngine;

/**
 * @class PersistentAudioListener
 * @brief Zapewnia, ¿e w scenie znajduje siê tylko jeden aktywny `AudioListener`.
 *
 * Skrypt usuwa siê automatycznie, jeœli w scenie istnieje ju¿ inny `AudioListener`,
 * zapobiegaj¹c konfliktom audio w Unity (komunikat: "There are 2 AudioListeners").
 * Ustawiony jako trwa³y miêdzy scenami dziêki `DontDestroyOnLoad`.
 *
 * @author Julia Bigaj
 */
public class PersistentAudioListener : MonoBehaviour
{
    /**
     * @brief Sprawdza, czy istnieje ju¿ inny `AudioListener`. Jeœli tak — usuwa siebie.
     */
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
