using UnityEngine;

/**
 * @class LeafRevealer
 * @brief Odsłania ukrytą lokalizację, gdy gracz wejdzie w obszar kolizji z liśćmi.
 *
 * Skrypt przypisany do obiektu z liśćmi. Gdy gracz wejdzie w trigger,
 * zostaje aktywowany wskazany obiekt `hiddenLocation` (np. ukryte przejście).
 * Może być częścią zagadki logicznej lub ukrytego obszaru w grze.
 *
 * @author Julia Bigaj
 */
public class LeafRevealer : MonoBehaviour
{
    /// @brief Obiekt, który zostanie ujawniony po wejściu gracza w trigger (np. ukryta ścieżka).
    public GameObject hiddenLocation;

    /**
     * @brief Wykrywany jest gracz wchodzący w trigger — ujawnienie ukrytej lokalizacji.
     * @param other Obiekt kolidujący (np. gracz).
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz wszedł w liście!");
            hiddenLocation.SetActive(true);
        }
    }
}

