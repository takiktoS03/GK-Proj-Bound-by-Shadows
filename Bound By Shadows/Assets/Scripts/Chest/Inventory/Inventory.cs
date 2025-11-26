using System.Collections.Generic;
using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;

    [Header("UI")]
    public Transform slotsParent;
    public GameObject slotPrefab;

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

    public void UpdateUI()
    {
        // usu? stare sloty
        foreach (Transform child in slotsParent)
            Destroy(child.gameObject);

        // dodaj nowe sloty
        foreach (var stack in items)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);

            slot.transform.Find("Icon").GetComponent<UnityEngine.UI.Image>().sprite = stack.item.icon;
            slot.transform.Find("Count").GetComponent<TMPro.TextMeshProUGUI>().text = stack.quantity.ToString();
        }
    }
}
