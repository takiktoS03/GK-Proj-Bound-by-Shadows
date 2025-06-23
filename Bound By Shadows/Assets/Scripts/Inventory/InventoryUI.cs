using UnityEngine;
using UnityEngine.UI;

/* Odpowiada za zarz¹dzanie interfejsem u¿ytkownika ekwipunku.
   - Otwiera/zamyka panel ekwipunku po naciœniêciu klawisza "E".
   - Odœwie¿a sloty oraz umo¿liwia wyœwietlanie treœci listów.
   - Korzysta z InventoryManager i InventorySlotSpawner.

   Autor: Julia Bigaj
*/

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public GameObject inventoryPanel;
    public Image letterContentImage;
    public InventorySlotSpawner slotSpawner;

    private bool isOpen = false;

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

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

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

    void RefreshInventory()
    {
        Debug.Log("Odœwie¿am ekwipunek...");

        // Odœwie¿ sloty w UI
        slotSpawner.RefreshSlots();
    }

    public void ShowLetterContent(LetterData letterData)
    {
        Debug.Log("Otwarto list: " + letterData.icon.name);

        // Poka¿ zawartoœæ
        letterContentImage.sprite = letterData.content;
        letterContentImage.gameObject.SetActive(true);
    }
}
