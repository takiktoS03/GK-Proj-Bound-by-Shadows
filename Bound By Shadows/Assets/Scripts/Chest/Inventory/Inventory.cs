using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
//using static System.Net.Mime.MediaTypeNames;

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

    [Header("Preview Hotbar")]
    public GameObject previewHotbarRoot;

    [HideInInspector]
    public ItemSO selectedItemForHotbar;


    private List<ItemStack> items = new List<ItemStack>();

    public void BindUI(
    Transform slotsParent,
    GameObject slotPrefab,
    Image previewImage,
    TextMeshProUGUI previewText,
    GameObject previewImageEthan,
    Button showCharacterButton,
    HotbarSlot[] hotbarSlots,
    GameObject previewHotbarRoot
)
    {
        this.slotsParent = slotsParent;
        this.slotPrefab = slotPrefab;
        this.previewImage = previewImage;
        this.previewText = previewText;
        this.previewImageEthan = previewImageEthan;
        this.showCharacterButton = showCharacterButton;
        this.hotbarSlots = hotbarSlots;
        this.previewHotbarRoot = previewHotbarRoot;

        UpdateUI();
        RefreshHotbar();
    }

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
        bool isLetter = item.itemType == ItemType.Letter;

        // POSTA?
        if (previewImageEthan != null)
            previewImageEthan.SetActive(!isLetter);

        // HOTBAR PRZY POSTACI
        if (previewHotbarRoot != null)
            previewHotbarRoot.SetActive(!isLetter);

        // reset preview
        previewImage.gameObject.SetActive(false);
        previewText.gameObject.SetActive(false);

        if (item.hasTextPreview)
        {
            previewText.text = item.textPreview;
            previewText.gameObject.SetActive(true);

            var c = previewImage.color;
            c.a = 0f;
            previewImage.color = c;
        }
        else if (item.hasImagePreview)
        {
            previewImage.sprite = item.imagePreview;
            previewImage.preserveAspect = true;

            var c = previewImage.color;
            c.a = 1f;
            previewImage.color = c;

            previewImage.gameObject.SetActive(true);
        }
    }

    public void SelectItemForHotbar(ItemSO item)
    {
        if (!item.canBeInHotbar)
            return;

        selectedItemForHotbar = item;
        //Debug.Log("Selected for hotbar: " + item.itemName);
    }

    public void ShowEthan()
    {
        // schowaj list
        previewImage.gameObject.SetActive(false);
        previewText.gameObject.SetActive(false);

        // poka? bohatera
        if (previewImageEthan != null && previewHotbarRoot != null)
            previewImageEthan.SetActive(true);
            previewHotbarRoot.SetActive(true);
    }

    public void UpdateUI()
    {
        // usu? stare sloty
        foreach (Transform child in slotsParent)
            Destroy(child.gameObject);

        // zbuduj UI od nowa
        foreach (var stack in items)
        {
            GameObject slotGO = Instantiate(slotPrefab, slotsParent);

            // ustaw ikon? i licznik (nazwy dzieci musz? si? zgadza? z prefabem)
            slotGO.transform.Find("Icon").GetComponent<Image>().sprite = stack.item.icon;
            slotGO.transform.Find("Count").GetComponent<TMPro.TextMeshProUGUI>().text = stack.quantity.ToString();

            // podepnij dane do slota (to masz w InventoryItemSlot)
            slotGO.GetComponent<InventoryItemSlot>().Init(stack);
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

    public bool ConsumeItem(ItemSO item, int amount = 1)
    {
        for (int i = 0; i < items.Count; i++)
        {
            if (items[i].item != item)
                continue;

            if (items[i].quantity < amount)
                return false;

            items[i].quantity -= amount;

            // je?li po zu?yciu mamy 0 ? usu? z listy i wyczy?? hotbar
            if (items[i].quantity <= 0)
            {
                ItemSO removedItem = items[i].item;
                items.RemoveAt(i);
                ClearItemFromHotbar(removedItem);
            }

            // od?wie? UI i hotbar
            UpdateUI();
            RefreshHotbar();
            OnInventoryChanged?.Invoke();

            return true; 
        }

        return false; // nie znaleziono itemu
    }


    public void ClearItemFromHotbar(ItemSO item)
    {
        for (int i = 0; i < HotbarData.Instance.items.Length; i++)
        {
            if (HotbarData.Instance.items[i] == item)
            {
                HotbarData.Instance.SetItem(i, null);
            }
        }
        foreach (var slot in hotbarSlots)
        {
            if (slot != null && slot.HasItem(item))
            {
                slot.Clear();
            }
        }
    }
}
