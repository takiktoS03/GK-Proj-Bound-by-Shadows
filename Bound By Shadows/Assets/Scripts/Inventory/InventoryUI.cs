using UnityEngine;
using UnityEngine.UI;

/**
 * @class InventoryUI
 * @brief Zarz¹dza interfejsem ekwipunku oraz wyœwietlaniem treœci listów.
 *
 * Umo¿liwia otwieranie/zamykanie ekwipunku, odœwie¿anie jego zawartoœci
 * oraz pokazywanie treœci wybranego listu (LetterData).
 *
 * @author Julia Bigaj
 */
public class InventoryUI : MonoBehaviour
{
    /// @brief Singleton zapewniaj¹cy dostêp do UI ekwipunku.
    public static InventoryUI Instance;

    /// @brief Panel g³ówny UI ekwipunku.
    public GameObject inventoryPanel;

    /// @brief Obraz prezentuj¹cy treœæ listu.
    public Image letterContentImage;

    /// @brief Komponent odpowiedzialny za generowanie slotów.
    public InventorySlotSpawner slotSpawner;

    /// @brief Czy panel ekwipunku jest aktualnie otwarty.
    private bool isOpen = false;

    /**
     * @brief Ustawia instancjê singletonu.
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
    * @brief Sprawdza naciœniêcie klawisza otwieraj¹cego ekwipunek.
    */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    /**
     * @brief Prze³¹cza widocznoœæ panelu ekwipunku.
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
     * @brief Odœwie¿a ekwipunek przy jego otwarciu.
     */
    void RefreshInventory()
    {
        Debug.Log("Odœwie¿am ekwipunek...");

        // Odœwie¿ sloty w UI
        slotSpawner.RefreshSlots();
    }

    /**
     * @brief Wyœwietla treœæ wybranego listu.
     * @param letter Obiekt `LetterData`, którego treœæ ma zostaæ pokazana.
     */
    public void ShowLetterContent(LetterData letterData)
    {
        Debug.Log("Otwarto list: " + letterData.icon.name);

        // Poka¿ zawartoœæ
        letterContentImage.sprite = letterData.content;
        letterContentImage.gameObject.SetActive(true);
    }
}
