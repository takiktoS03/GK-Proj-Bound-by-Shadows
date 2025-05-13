using UnityEngine;

/**
 Julia Bigaj
 Ustawianie u rodzica (Dekoracje) automatycznie warstwy 3 u jego dzieci
 **/
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

    // Update is called once per frame
    void Update()
    {
        
    }
}
