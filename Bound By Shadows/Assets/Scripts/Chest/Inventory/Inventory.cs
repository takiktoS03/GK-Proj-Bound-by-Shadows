using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public System.Action OnInventoryChanged;

    public static Inventory Instance;

    [Header("UI")]
    public Transform slotsParent;
    public GameObject slotPrefab;

    public Image previewImage;
    public TextMeshProUGUI previewText;

    [Header("Character Preview")]
    public GameObject previewImageEthan;
    public Button showCharacterButton;

    [Header("Hotbar")]
    public HotbarSlot[] hotbarSlots;

    [HideInInspector]
    public ItemSO selectedItemForHotbar;


    private List<ItemStack> items = new List<ItemStack>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemSO itemSO, int quantity)
    {
        if (itemSO.itemType != ItemType.Letter)
        {
            foreach (var stack in items)
            {
                if (stack.item == itemSO)
                {
                    stack.quantity += quantity;
                    UpdateUI();
                    OnInventoryChanged?.Invoke();
                    RefreshHotbar();
                    return;
                }
            }
        }

        items.Add(new ItemStack(itemSO, quantity));
        UpdateUI();
        OnInventoryChanged?.Invoke();
        RefreshHotbar();
    }
    public void ShowPreview(ItemSO item)
    {

        if (item.itemType == ItemType.Letter)
        {
            if (previewImageEthan != null)
                previewImageEthan.SetActive(false);
        }
        else
        {
            // dla reszty itemów bohater zostaje
            if (previewImageEthan != null)
                previewImageEthan.SetActive(true);
        }

        // Reset preview UI
        previewImage.gameObject.SetActive(false);
        previewText.gameObject.SetActive(false);

        // Je?li ma tekst
        if (item.hasTextPreview)
        {
            previewText.text = item.textPreview;
            previewText.gameObject.SetActive(true);

            // ukryj obrazek
            var c = previewImage.color;
            c.a = 0f;
            previewImage.color = c;
        }
        // Je?li ma obrazek
        else if (item.hasImagePreview)
        {
            previewImage.sprite = item.imagePreview;
            previewImage.preserveAspect = true;

            // poka? obrazek
            var c = previewImage.color;
            c.a = 1f;       // ALFA = 100%
            previewImage.color = c;

            previewImage.gameObject.SetActive(true);
        }
        else
        {
            previewText.text = "Brak podgl?du";
            previewText.gameObject.SetActive(true);

            // ukryj obrazek
            var c = previewImage.color;
            c.a = 0f;
            previewImage.color = c;
        }
    }

    public void SelectItemForHotbar(ItemSO item)
    {
        if (!item.canBeInHotbar)
            return;

        selectedItemForHotbar = item;
        Debug.Log("Selected for hotbar: " + item.itemName);
    }

    public void ShowEthan()
    {
        // schowaj list
        previewImage.gameObject.SetActive(false);
        previewText.gameObject.SetActive(false);

        // poka? bohatera
        if (previewImageEthan != null)
            previewImageEthan.SetActive(true);
    }

    public void UpdateUI()
    {

        // usu? stare sloty
        foreach (Transform child in slotsParent)
            Destroy(child.gameObject);

        // dodaj nowe sloty
        foreach (var stack in items)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);

            // ustawianie ikony i licznika
            slot.transform.Find("Icon").GetComponent<Image>().sprite = stack.item.icon;
            slot.transform.Find("Count").GetComponent<TMPro.TextMeshProUGUI>().text = stack.quantity.ToString();

            // powi?zanie slota z danym itemem
            slot.GetComponent<InventoryItemSlot>().Init(stack);
        }
    }

    public int GetItemCount(ItemSO item)
    {
        int total = 0;

        foreach (var stack in items)
        {
            if (stack.item == item)
            {
                total += stack.quantity;
            }
        }

        return total;
    }

    private void RefreshHotbar()
    {
        foreach (var slot in hotbarSlots)
        {
            if (slot != null)
                slot.UpdateCount();
        }
    }
}
