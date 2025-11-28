using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("UI")]
    public Transform slotsParent;
    public GameObject slotPrefab;

    public TextMeshProUGUI previewDescriptionText;
    public Image previewImage;
    public TextMeshProUGUI previewText;

    private List<ItemStack> items = new List<ItemStack>();

    private void Awake()
    {
        Instance = this;
    }

    public void AddItem(ItemSO itemSO, int quantity)
    {
        // je?li item ju? istnieje ? zwi?ksz ilo??
        // LISTY nie mog? si? stackowa?
        if (itemSO.itemType != ItemType.Letter)
        {
            // je?li item ju? istnieje ? zwi?ksz quantity
            foreach (var stack in items)
            {
                if (stack.item == itemSO)
                {
                    stack.quantity += quantity;
                    UpdateUI();
                    return;
                }
            }
        }

        // je?li to list albo nowy item ? dodaj jako osobny wpis
        items.Add(new ItemStack(itemSO, quantity));
        UpdateUI();

    }
    public void ShowPreview(ItemSO item)
    {
        // Reset UI
        previewImage.gameObject.SetActive(false);
        previewText.gameObject.SetActive(false);
        previewDescriptionText.gameObject.SetActive(true);

        // Ustaw tytu?
        previewDescriptionText.text = item.description;

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
}
