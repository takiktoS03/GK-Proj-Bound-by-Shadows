using System.Collections.Generic;
using UnityEngine;

public class Chest : MonoBehaviour
{
    public List<ItemStack> items;

    public bool isOpened = false;

    public void OpenChest()
    {
        if (isOpened) return;

        isOpened = true;

        SoundLibrary.Instance.PlayChest();

        GetComponent<Animator>().SetTrigger("Open");

        ChestUI.Instance.Show(this);
    }

    public void CloseChest()
    {
        isOpened = true;

        ChestUI.Instance.Hide();
    }

    public void TakeItem(int index)
    {
        ItemStack stack = items[index];

        Inventory.Instance.AddItem(stack.item, stack.quantity);

        items.RemoveAt(index);

        ChestUI.Instance.UpdateUI();
    }

    public void TakeAll()
    {
        Debug.Log("Chest.TakeAll CALLED");

        foreach (var stack in items)
        {
            Debug.Log("Chest.TakeAll CALLED");
            Inventory.Instance.AddItem(stack.item, stack.quantity);
        }

        items.Clear();
        CloseChest();
        //ChestUI.Instance.UpdateUI();
    }

}
