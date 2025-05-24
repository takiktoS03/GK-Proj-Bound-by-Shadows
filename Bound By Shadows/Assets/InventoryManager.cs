using UnityEngine;
using System.Collections.Generic;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<string> items = new List<string> ();

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void AddItem(string item)
    {
        items.Add(item);
        Debug.Log("Dodano do ekwipunku: " + item);
    }

    public void RemoveItem(string item) 
    {  
        items.Remove(item);
        Debug.Log("Usuniêto z ekwipunku: " + item);
    }
    
}
