using UnityEngine;
using UnityEngine.UI;

/**
 * @class InventoryUI
 * @brief Zarządza interfejsem ekwipunku oraz wyświetlaniem treści listów.
 *
 * Umożliwia otwieranie/zamykanie ekwipunku, odświeżanie jego zawartości
 * oraz pokazywanie treści wybranego listu (LetterData).
 *
 * @author Julia Bigaj
 */
public class InventoryUI : MonoBehaviour
{
    /// @brief Singleton zapewniający dostęp do UI ekwipunku.
    public static InventoryUI Instance;

    /// @brief Panel główny UI ekwipunku.
    public GameObject inventoryPanel;

    /// @brief Obraz prezentujący treść listu.
    public Image letterContentImage;

    /// @brief Komponent odpowiedzialny za generowanie slotów.
    public InventorySlotSpawner slotSpawner;

    /// @brief Czy panel ekwipunku jest aktualnie otwarty.
    private bool isOpen = false;

    /**
     * @brief Ustawia instancję singletonu.
     */
    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /**
    * @brief Sprawdza naciśnięcie klawisza otwierającego ekwipunek.
    */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    /**
     * @brief Przełącza widoczność panelu ekwipunku.
     */
    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        UIStateManager.isUIOpen = isOpen;

        if (isOpen)
        {
            RefreshInventory();
        }
    }

    /**
     * @brief Odświeża ekwipunek przy jego otwarciu.
     */
    void RefreshInventory()
    {
        Debug.Log("Odświeżam ekwipunek...");

        // Odśwież sloty w UI
        slotSpawner.RefreshSlots();
    }

    /**
     * @brief Wyświetla treść wybranego listu.
     * @param letter Obiekt `LetterData`, którego treść ma zostać pokazana.
     */
    public void ShowLetterContent(LetterData letterData)
    {
        Debug.Log("Otwarto list: " + letterData.icon.name);

        // Pokaż zawartość
        letterContentImage.sprite = letterData.content;
        letterContentImage.gameObject.SetActive(true);
    }
}

