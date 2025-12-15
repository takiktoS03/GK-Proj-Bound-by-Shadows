using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public Image icon;

    private ItemSO assignedItem;

    public bool IsEmpty => assignedItem == null;

    private void Start()
    {
        icon.enabled = false;
    }

    public void SetItem(ItemSO item)
    {
        assignedItem = item;
        icon.sprite = item.icon;
        icon.enabled = true;
    }

    public void OnClick()
    {
        ItemSO selected = Inventory.Instance.selectedItemForHotbar;
        if (selected == null) return;

        SetItem(selected);

        // czy?cimy wybór (opcjonalnie)
        Inventory.Instance.selectedItemForHotbar = null;
    }
}
