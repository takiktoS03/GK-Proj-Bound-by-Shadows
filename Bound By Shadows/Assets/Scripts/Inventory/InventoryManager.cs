using UnityEngine;
using System.Collections.Generic;


/**
 * @class InventoryManager
 * @brief Zarz¹dza systemem ekwipunku gracza.
 *
 * Przechowuje zebrane przedmioty oraz listy, zapewnia singleton do globalnego dostêpu
 * i pozwala na ich dodawanie i usuwanie.
 *
 * @author Julia Bigaj
 */
public class InventoryManager : MonoBehaviour
{
    /// @brief Singleton umo¿liwiaj¹cy dostêp do instancji klasy z innych skryptów.
    public static InventoryManager Instance;

    /// @brief Lista nazw zwyk³ych przedmiotów w ekwipunku.
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
     * @param item Nazwa przedmiotu do usuniêcia.
     */
    public void RemoveItem(string item) 
    {  
        items.Remove(item);
        Debug.Log("Usuniêto z ekwipunku: " + item);
    }

    /**
     * @brief Dodaje list do listy zebranych listów.
     * @param newLetter Obiekt LetterData reprezentuj¹cy nowy list.
     */
    public void AddLetter(LetterData newLetter)
    {
        collectedLetters.Add(newLetter);
        Debug.Log("Dodano list do ekwipunku: " + newLetter.icon.name);
    }
}
