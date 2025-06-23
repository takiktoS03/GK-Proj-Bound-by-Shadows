using UnityEngine;
using UnityEngine.UI;

/* Odpowiada za zarz�dzanie interfejsem u�ytkownika ekwipunku.
   - Otwiera/zamyka panel ekwipunku po naci�ni�ciu klawisza "E".
   - Od�wie�a sloty oraz umo�liwia wy�wietlanie tre�ci list�w.
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
        Debug.Log("Od�wie�am ekwipunek...");

        // Od�wie� sloty w UI
        slotSpawner.RefreshSlots();
    }

    public void ShowLetterContent(LetterData letterData)
    {
        Debug.Log("Otwarto list: " + letterData.icon.name);

        // Poka� zawarto��
        letterContentImage.sprite = letterData.content;
        letterContentImage.gameObject.SetActive(true);
    }
}
