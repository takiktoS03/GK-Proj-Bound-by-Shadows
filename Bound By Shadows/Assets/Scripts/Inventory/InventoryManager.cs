using UnityEngine;
using System.Collections.Generic;


/**
 * @class InventoryManager
 * @brief Zarz�dza systemem ekwipunku gracza.
 *
 * Przechowuje zebrane przedmioty oraz listy, zapewnia singleton do globalnego dost�pu
 * i pozwala na ich dodawanie i usuwanie.
 *
 * @author Julia Bigaj
 */
public class InventoryManager : MonoBehaviour
{
    /// @brief Singleton umo�liwiaj�cy dost�p do instancji klasy z innych skrypt�w.
    public static InventoryManager Instance;

    /// @brief Lista nazw zwyk�ych przedmiot�w w ekwipunku.
    public List<string> items = new List<string> ();

    /// @brief Lista zebranych list�w (LetterData).
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
     * @brief Dodaje przedmiot do listy przedmiot�w.
     * @param item Nazwa przedmiotu do dodania.
     */
    public void AddItem(string item)
    {
        items.Add(item);
        Debug.Log("Dodano do ekwipunku: " + item);
    }

    /**
     * @brief Usuwa przedmiot z listy przedmiot�w.
     * @param item Nazwa przedmiotu do usuni�cia.
     */
    public void RemoveItem(string item) 
    {  
        items.Remove(item);
        Debug.Log("Usuni�to z ekwipunku: " + item);
    }

    /**
     * @brief Dodaje list do listy zebranych list�w.
     * @param newLetter Obiekt LetterData reprezentuj�cy nowy list.
     */
    public void AddLetter(LetterData newLetter)
    {
        collectedLetters.Add(newLetter);
        Debug.Log("Dodano list do ekwipunku: " + newLetter.icon.name);
    }
}
