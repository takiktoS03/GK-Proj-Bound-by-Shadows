using UnityEngine;

public class SetOrderInLayerForChildren : MonoBehaviour
{
    public int orderInLayer = 3;

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
