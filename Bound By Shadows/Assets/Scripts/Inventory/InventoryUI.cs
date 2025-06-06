using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public static InventoryUI Instance;

    public GameObject inventoryPanel;
    public Image letterContentImage;
    public InventorySlotSpawner slotSpawner;

    private bool isOpen = false;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            ToggleInventory();
        }
    }

    void Awake()
    {
        Debug.Log("InventoryUI Awake wywo³ane!");
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void ToggleInventory()
    {
        isOpen = !isOpen;
        inventoryPanel.SetActive(isOpen);

        if (isOpen)
        {
            RefreshInventory();
        }
    }

    void RefreshInventory()
    {
        Debug.Log("Odœwie¿am ekwipunek...");

        // Poka¿ zawartoœæ listów z collectedLetters
        foreach (var letter in InventoryManager.Instance.collectedLetters)
        {
            Debug.Log("List w ekwipunku: " + letter.icon.name);
            // Tutaj mo¿esz wstawiaæ miniaturki w UI, np. jako sloty
        }

        slotSpawner.RefreshSlots();

    }

    public void ShowLetterContent(Sprite content)
    {
        Debug.Log("ShowLetterContent wywo³ane!");
        letterContentImage.sprite = content;
        letterContentImage.gameObject.SetActive(true);
    }
}
