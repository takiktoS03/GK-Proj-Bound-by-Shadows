using UnityEngine;

/**
 * @class LeafRevealer
 * @brief Ods³ania ukryt¹ lokalizacjê, gdy gracz wejdzie w obszar kolizji z liœæmi.
 *
 * Skrypt przypisany do obiektu z liœæmi. Gdy gracz wejdzie w trigger,
 * zostaje aktywowany wskazany obiekt `hiddenLocation` (np. ukryte przejœcie).
 * Mo¿e byæ czêœci¹ zagadki logicznej lub ukrytego obszaru w grze.
 *
 * @author Julia Bigaj
 */
public class LeafRevealer : MonoBehaviour
{
    /// @brief Obiekt, który zostanie ujawniony po wejœciu gracza w trigger (np. ukryta œcie¿ka).
    public GameObject hiddenLocation;

    /**
     * @brief Wykrywany jest gracz wchodz¹cy w trigger — ujawnienie ukrytej lokalizacji.
     * @param other Obiekt koliduj¹cy (np. gracz).
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz wszed³ w liœcie!");
            hiddenLocation.SetActive(true);
        }
    }
}
