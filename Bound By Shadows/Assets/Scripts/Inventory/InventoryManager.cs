using UnityEngine;
using System.Collections.Generic;


/* Skrypt zarz¹dza systemem ekwipunku:
   - Przechowuje zwyk³e przedmioty i zebrane listy.
   - Zapewnia singleton, dziêki czemu mo¿na siê do niego ³atwo odwo³ywaæ z innych klas.
   - Umo¿liwia dodawanie i usuwanie przedmiotów oraz list.

   Autor: Julia Bigaj
*/

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance;

    public List<string> items = new List<string> ();

    public List<LetterData> collectedLetters = new List<LetterData>();

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

    public void AddLetter(LetterData newLetter)
    {
        collectedLetters.Add(newLetter);
        Debug.Log("Dodano list do ekwipunku: " + newLetter.icon.name);
    }
}
