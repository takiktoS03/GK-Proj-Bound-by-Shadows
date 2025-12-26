using TMPro;
using UnityEngine;
using UnityEngine.UI;

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
