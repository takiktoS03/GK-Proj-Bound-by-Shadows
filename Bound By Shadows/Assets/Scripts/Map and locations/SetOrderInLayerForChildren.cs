using UnityEngine;

/* Skrypt przypisuje wszystkim dzieciom (z komponentem SpriteRenderer) okreœlony `sortingOrder`.
   - U¿ywany g³ównie przy dekoracjach, aby ustawiæ odpowiedni¹ warstwê renderowania sprite'ów.
   - Mo¿e byæ przypisany do obiektu nadrzêdnego (np. "Dekoracje").

   Autor: Julia Bigaj
*/

public class SetOrderInLayerForChildren : MonoBehaviour
{
    public int orderInLayer = 2;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        foreach(SpriteRenderer sr in GetComponentsInChildren<SpriteRenderer>())
        {
            sr.sortingOrder = orderInLayer;
        }
    }
}
