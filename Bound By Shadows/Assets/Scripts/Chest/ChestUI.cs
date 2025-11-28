using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ChestUI : MonoBehaviour
{
    public static ChestUI Instance;

    public GameObject chestPanel;
    public Transform slotParent;
    public GameObject slotPrefab;

    private Chest currentChest;
    public bool IsOpen => chestPanel.activeSelf;

    private void Awake()
    {
        Instance = this;
        chestPanel.SetActive(false);
    }

    private void Update()
    {
        // Naci?ni?cie F podczas otwartej skrzyni = Take All + zamknij
        if (chestPanel.activeSelf && Input.GetKeyDown(KeyCode.F))
        {
            TakeAll();
            Hide();
        }
    }

    public void Show(Chest chest)
    {
        currentChest = chest;
        chestPanel.SetActive(true);
        UpdateUI();
    }

    public void Hide()
    {
        chestPanel.SetActive(false);
    }

    public void UpdateUI()
    {
        // Usu? stare sloty
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        // Dodaj nowe sloty
        for (int i = 0; i < currentChest.items.Count; i++)
        {
            var stack = currentChest.items[i];
            GameObject slot = Instantiate(slotPrefab, slotParent);

            // ustaw ikon?
            slot.transform.Find("Icon").GetComponent<Image>().sprite = stack.item.icon;
        }
    }

    public void TakeAll()
    {
        // przenie? do ekwipunku
        foreach (var stack in currentChest.items)
        {
            Inventory.Instance.AddItem(stack.item, stack.quantity);
        }

        currentChest.items.Clear();
        UpdateUI();
    }
}
