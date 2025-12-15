using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InventoryItemSlot : MonoBehaviour
{
    private ItemStack itemStack;

    public void Init(ItemStack stack)
    {
        itemStack = stack;
    }

    public void OnClick()
    {
        Debug.Log("CLICKED SLOT: " + itemStack.item.itemName);
        Inventory.Instance.ShowPreview(itemStack.item);

        if (itemStack.item.canBeInHotbar)
        {
            Inventory.Instance.SelectItemForHotbar(itemStack.item);
        }
    }


    public ItemSO GetItem()
    {
        return itemStack.item;
    }

}