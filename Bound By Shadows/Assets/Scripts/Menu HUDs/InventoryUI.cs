using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;

    private bool isOpen = false;

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

        if (isOpen)
        {
            RefreshInventory();
        }
    }

    void RefreshInventory()
    {
        Debug.Log("Odœwie¿am ekwipunek...");
    }
}
