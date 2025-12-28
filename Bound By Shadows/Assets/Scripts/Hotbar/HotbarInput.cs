using System.Diagnostics;
using UnityEngine;

public class HotbarInput : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Alpha1))
            UseSlot(0);

        if (Input.GetKeyDown(KeyCode.Alpha2))
            UseSlot(1);

        if (Input.GetKeyDown(KeyCode.Alpha3))
            UseSlot(2);

        if (Input.GetKeyDown(KeyCode.Alpha4))
            UseSlot(3);
    }

    private void UseSlot(int index)
    {
        ItemSO item = HotbarData.Instance.GetItem(index);

        if (item == null)
        {
            UnityEngine.Debug.Log($"Slot {index + 1} is empty");
            return;
        }

        switch (item.itemType)
        {
            case ItemType.HealPotion:
                UsePotion(item);
                break;

            case ItemType.StaminaPotion:
                UseStaminaPotion(item);
                break;

            default:
                UnityEngine.Debug.Log($"{item.itemName} cannot be used");
                break;
        }
    }

    private void UsePotion(ItemSO potion)
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth == null)
        {
            UnityEngine.Debug.LogError("PlayerHealth not found!");
            return;
        }

        bool consumed = Inventory.Instance.ConsumeItem(potion, 1);

        if (!consumed)
        {
            UnityEngine.Debug.Log("No potion left");
            return;
        }

        playerHealth.Heal(potion.healAmount);
    }

    private void UseStaminaPotion(ItemSO potion)
    {
        PlayerHealth playerHealth = FindObjectOfType<PlayerHealth>();

        if (playerHealth == null)
            return;

        // najpierw sprawdzamy i zu?ywamy item
        bool consumed = Inventory.Instance.ConsumeItem(potion, 1);
        if (!consumed)
            return;

        // potem regenerujemy stamin?
        playerHealth.HealStamina(potion.staminaAmount);
    }

}
