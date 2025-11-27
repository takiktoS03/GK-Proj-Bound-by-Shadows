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
    }
}