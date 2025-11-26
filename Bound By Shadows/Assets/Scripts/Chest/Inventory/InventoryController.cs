using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public GameObject inventoryPanel; // przeci?gasz tu InventoryPanel
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
            Time.timeScale = 0f; // pauza
        else
            Time.timeScale = 1f; // normalnie
    }
}
