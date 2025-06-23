using UnityEngine;
using System.Collections.Generic;


/* Skrypt zarz�dza systemem ekwipunku:
   - Przechowuje zwyk�e przedmioty i zebrane listy.
   - Zapewnia singleton, dzi�ki czemu mo�na si� do niego �atwo odwo�ywa� z innych klas.
   - Umo�liwia dodawanie i usuwanie przedmiot�w oraz list.

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
        Debug.Log("Usuni�to z ekwipunku: " + item);
    }

    public void AddLetter(LetterData newLetter)
    {
        collectedLetters.Add(newLetter);
        Debug.Log("Dodano list do ekwipunku: " + newLetter.icon.name);
    }
}
