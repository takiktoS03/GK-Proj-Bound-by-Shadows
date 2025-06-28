using UnityEngine;
using System.Collections.Generic;


/**
 * @class InventoryManager
 * @brief Zarządza systemem ekwipunku gracza.
 *
 * Przechowuje zebrane przedmioty oraz listy, zapewnia singleton do globalnego dostępu
 * i pozwala na ich dodawanie i usuwanie.
 *
 * @author Julia Bigaj
 */
public class InventoryManager : MonoBehaviour
{
    /// @brief Singleton umożliwiający dostęp do instancji klasy z innych skryptów.
    public static InventoryManager Instance;

    /// @brief Lista nazw zwykłych przedmiotów w ekwipunku.
    public List<string> items = new List<string> ();

    /// @brief Lista zebranych listów (LetterData).
    public List<LetterData> collectedLetters = new List<LetterData>();

    /**
     * @brief Ustawia singleton i zapobiega zniszczeniu obiektu przy zmianie sceny.
     */
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

    /**
     * @brief Dodaje przedmiot do listy przedmiotów.
     * @param item Nazwa przedmiotu do dodania.
     */
    public void AddItem(string item)
    {
        items.Add(item);
        Debug.Log("Dodano do ekwipunku: " + item);
    }

    /**
     * @brief Usuwa przedmiot z listy przedmiotów.
     * @param item Nazwa przedmiotu do usunięcia.
     */
    public void RemoveItem(string item) 
    {  
        items.Remove(item);
        Debug.Log("Usunięto z ekwipunku: " + item);
    }

    /**
     * @brief Dodaje list do listy zebranych listów.
     * @param newLetter Obiekt LetterData reprezentujący nowy list.
     */
    public void AddLetter(LetterData newLetter)
    {
        collectedLetters.Add(newLetter);
        Debug.Log("Dodano list do ekwipunku: " + newLetter.icon.name);
    }
}

