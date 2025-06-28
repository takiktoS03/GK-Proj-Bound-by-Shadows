using UnityEngine;

/**
 * @class LeafRevealer
 * @brief Ods�ania ukryt� lokalizacj�, gdy gracz wejdzie w obszar kolizji z li��mi.
 *
 * Skrypt przypisany do obiektu z li��mi. Gdy gracz wejdzie w trigger,
 * zostaje aktywowany wskazany obiekt `hiddenLocation` (np. ukryte przej�cie).
 * Mo�e by� cz�ci� zagadki logicznej lub ukrytego obszaru w grze.
 *
 * @author Julia Bigaj
 */
public class LeafRevealer : MonoBehaviour
{
    /// @brief Obiekt, kt�ry zostanie ujawniony po wej�ciu gracza w trigger (np. ukryta �cie�ka).
    public GameObject hiddenLocation;

    /**
     * @brief Wykrywany jest gracz wchodz�cy w trigger � ujawnienie ukrytej lokalizacji.
     * @param other Obiekt koliduj�cy (np. gracz).
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("Gracz wszed� w li�cie!");
            hiddenLocation.SetActive(true);
        }
    }
}
