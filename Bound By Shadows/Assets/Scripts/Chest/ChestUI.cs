using UnityEngine;
using UnityEngine.UI;
using TMPro;
//using static System.Net.Mime.MediaTypeNames;

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
        if (currentChest == null) return;

        // usu? stare sloty
        foreach (Transform child in slotParent)
            Destroy(child.gameObject);

        // dodaj nowe sloty
        foreach (var stack in currentChest.items)
        {
            GameObject slot = Instantiate(slotPrefab, slotParent);

            // ikonka
            slot.transform.Find("Icon")
                .GetComponent<Image>().sprite = stack.item.icon;

            // ilo??
            var countText = slot.transform.Find("Count")
                .GetComponent<TextMeshProUGUI>();

            countText.text = stack.quantity.ToString();
            countText.gameObject.SetActive(stack.quantity > 1);
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
