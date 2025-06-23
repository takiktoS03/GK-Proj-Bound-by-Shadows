using UnityEngine;

/* Skrypt przypisuje wszystkim dzieciom (z komponentem SpriteRenderer) okre�lony `sortingOrder`.
   - U�ywany g��wnie przy dekoracjach, aby ustawi� odpowiedni� warstw� renderowania sprite'�w.
   - Mo�e by� przypisany do obiektu nadrz�dnego (np. "Dekoracje").

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
