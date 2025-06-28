using UnityEngine;
using UnityEngine.UI;

/**
 * @class InventoryUI
 * @brief Zarz�dza interfejsem ekwipunku oraz wy�wietlaniem tre�ci list�w.
 *
 * Umo�liwia otwieranie/zamykanie ekwipunku, od�wie�anie jego zawarto�ci
 * oraz pokazywanie tre�ci wybranego listu (LetterData).
 *
 * @author Julia Bigaj
 */
public class InventoryUI : MonoBehaviour
{
    /// @brief Singleton zapewniaj�cy dost�p do UI ekwipunku.
    public static InventoryUI Instance;

    /// @brief Panel g��wny UI ekwipunku.
    public GameObject inventoryPanel;

    /// @brief Obraz prezentuj�cy tre�� listu.
    public Image letterContentImage;

    /// @brief Komponent odpowiedzialny za generowanie slot�w.
    public InventorySlotSpawner slotSpawner;

    /// @brief Czy panel ekwipunku jest aktualnie otwarty.
    private bool isOpen = false;

    /**
     * @brief Ustawia instancj� singletonu.
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
    * @brief Sprawdza naci�ni�cie klawisza otwieraj�cego ekwipunek.
    */
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    /**
     * @brief Prze��cza widoczno�� panelu ekwipunku.
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
     * @brief Od�wie�a ekwipunek przy jego otwarciu.
     */
    void RefreshInventory()
    {
        Debug.Log("Od�wie�am ekwipunek...");

        // Od�wie� sloty w UI
        slotSpawner.RefreshSlots();
    }

    /**
     * @brief Wy�wietla tre�� wybranego listu.
     * @param letter Obiekt `LetterData`, kt�rego tre�� ma zosta� pokazana.
     */
    public void ShowLetterContent(LetterData letterData)
    {
        Debug.Log("Otwarto list: " + letterData.icon.name);

        // Poka� zawarto��
        letterContentImage.sprite = letterData.content;
        letterContentImage.gameObject.SetActive(true);
    }
}
