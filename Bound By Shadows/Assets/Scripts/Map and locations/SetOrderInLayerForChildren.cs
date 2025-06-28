using UnityEngine;

/**
 * @class SetOrderInLayerForChildren
 * @brief Ustawia warstw� rysowania (`sortingOrder`) dla wszystkich dzieci posiadaj�cych komponent SpriteRenderer.
 *
 * Skrypt przeznaczony do kontrolowania kolejno�ci renderowania sprite'�w w hierarchii obiekt�w.
 * Mo�e by� u�ywany np. na obiekcie nadrz�dnym zawieraj�cym elementy dekoracyjne.
 *
 * @author Julia Bigaj
 */
public class SetOrderInLayerForChildren : MonoBehaviour
{
    /// @brief Warto�� warstwy renderowania przypisywana wszystkim dzieciom.
    public int orderInLayer = 2;

    /**
     * @brief Ustawia `sortingOrder` wszystkim komponentom SpriteRenderer w obiektach potomnych.
     *
     * Wywo�ywana raz po uruchomieniu gry (przy starcie sceny).
     */
    void Start()
    {
        foreach (SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sortingOrder = orderInLayer;
        }
    }
}
