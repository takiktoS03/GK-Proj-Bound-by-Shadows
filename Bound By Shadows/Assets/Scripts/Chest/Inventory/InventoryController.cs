using UnityEngine;

public class InventoryController : MonoBehaviour
{
    public static InventoryController Instance { get; private set; }

    private GameObject inventoryPanel;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }
    private bool isOpen = false;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            bool open = !inventoryPanel.activeSelf;
            inventoryPanel.SetActive(open);
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

    public void SetInventoryPanel(GameObject panel)
    {
        inventoryPanel = panel;
        inventoryPanel.SetActive(false);
    }

}
