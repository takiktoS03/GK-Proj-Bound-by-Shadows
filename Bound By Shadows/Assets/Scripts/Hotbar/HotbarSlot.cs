using TMPro;
using UnityEngine;
using UnityEngine.UI;

/**
 * Skrypt obsługujący przypisywanie przedmiotów do slotów hotbara z poziomu UI.
 * Zapewnia synchronizację danych hotbara z zawartością ekwipunku.
 *
 * @author Julia Bigaj
 */
public class HotbarSlot : MonoBehaviour
{
    public Image icon;
    public TextMeshProUGUI countObjectsText;
    public int slotIndex;

    private ItemSO assignedItem;

    public bool IsEmpty => assignedItem == null;

    public bool HasItem(ItemSO item)
    {
        return assignedItem == item;
    }

    private void Start()
    {
        icon.enabled = false;
        countObjectsText.gameObject.SetActive(false);
    }

    public void SetItem(ItemSO item)
    {

        foreach (var slot in FindObjectsOfType<HotbarSlot>())
        {
            if (slot != this && slot.HasItem(item))
            {
                slot.Clear();
            }
        }

        for (int i = 0; i < HotbarData.Instance.items.Length; i++)
        {
            if (HotbarData.Instance.items[i] == item)
            {
                HotbarData.Instance.SetItem(i, null);
            }
        }

        assignedItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;
        UpdateCount();

        HotbarData.Instance.SetItem(slotIndex, item);
    }

    public void OnClick()
    {
        ItemSO selected = Inventory.Instance.selectedItemForHotbar;
        if (selected == null) return;

        SetItem(selected);

        // czy?cimy wybór (opcjonalnie)
        Inventory.Instance.selectedItemForHotbar = null;
    }

    public void UpdateCount()
    {
        if (assignedItem == null)
        {
            countObjectsText.gameObject.SetActive(false);
            return;
        }

        int count = Inventory.Instance.GetItemCount(assignedItem);

        countObjectsText.text = count.ToString();
        countObjectsText.gameObject.SetActive(count > 1);
    }

    public void Clear()
    {
        assignedItem = null;
        icon.enabled = false;
        countObjectsText.gameObject.SetActive(false);
    }
}
