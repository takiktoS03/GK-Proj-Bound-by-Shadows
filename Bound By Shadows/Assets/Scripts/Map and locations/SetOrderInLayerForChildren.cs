using UnityEngine;

/**
 * @class SetOrderInLayerForChildren
 * @brief Ustawia warstwê rysowania (`sortingOrder`) dla wszystkich dzieci posiadaj¹cych komponent SpriteRenderer.
 *
 * Skrypt przeznaczony do kontrolowania kolejnoœci renderowania sprite'ów w hierarchii obiektów.
 * Mo¿e byæ u¿ywany np. na obiekcie nadrzêdnym zawieraj¹cym elementy dekoracyjne.
 *
 * @author Julia Bigaj
 */
public class SetOrderInLayerForChildren : MonoBehaviour
{
    /// @brief Wartoœæ warstwy renderowania przypisywana wszystkim dzieciom.
    public int orderInLayer = 2;

    /**
     * @brief Ustawia `sortingOrder` wszystkim komponentom SpriteRenderer w obiektach potomnych.
     *
     * Wywo³ywana raz po uruchomieniu gry (przy starcie sceny).
     */
    void Start()
    {
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sortingOrder = orderInLayer;
        }
    }
}
