using UnityEngine;

/**
 * @class SetOrderInLayerForChildren
 * @brief Ustawia warstwę rysowania (`sortingOrder`) dla wszystkich dzieci posiadających komponent SpriteRenderer.
 *
 * Skrypt przeznaczony do kontrolowania kolejności renderowania sprite'ów w hierarchii obiektów.
 * Może być używany np. na obiekcie nadrzędnym zawierającym elementy dekoracyjne.
 *
 * @author Julia Bigaj
 */
public class SetOrderInLayerForChildren : MonoBehaviour
{
    /// @brief Wartość warstwy renderowania przypisywana wszystkim dzieciom.
    public int orderInLayer = 2;

    /**
     * @brief Ustawia `sortingOrder` wszystkim komponentom SpriteRenderer w obiektach potomnych.
     *
     * Wywoływana raz po uruchomieniu gry (przy starcie sceny).
     */
    void Start()
    {
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sortingOrder = orderInLayer;
        }
    }
}

