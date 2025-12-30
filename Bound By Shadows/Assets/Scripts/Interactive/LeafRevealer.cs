using UnityEngine;

/**
 * @class LeafRevealer
 * @brief Odsłania ukrytą lokalizację, gdy gracz wejdzie w obszar kolizji.
 *
 * Skrypt przypisany do obiektu. Gdy gracz wejdzie w trigger,
 * zostaje aktywowany wskazany obiekt `hiddenLocation` (np. ukryte przejście).
 * Może być częścią zagadki logicznej lub ukrytego obszaru w grze.
 *
 * @author Julia Bigaj
 */
public class LocationRevealer : MonoBehaviour
{
    /// @brief Obiekt, który zostanie ujawniony po wejściu gracza w trigger (np. ukryta ścieżka).
    public GameObject hiddenLocation;
    /// @brief Obiekt, który będzie usunięty po wejściu gracza w trigger (np. tło).
    public GameObject activeLocation;

    /**
     * @brief Wykrywany jest gracz wchodzący w trigger — ujawnienie ukrytej lokalizacji.
     * @param other Obiekt kolidujący (np. gracz).
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Destroy(activeLocation);
            hiddenLocation.SetActive(true);
        }
    }
}

