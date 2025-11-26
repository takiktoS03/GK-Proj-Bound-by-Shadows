using UnityEngine;

public enum ItemType
{
    Letter,
    Potion,
    Key,
    Other
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    [TextArea]
    public string description;

    // Dodatkowe dane – np. dla mikstur
    public int healAmount;

    // Dodatkowe dane dla listu
    [TextArea(5, 10)]
    public string letterText;
}

[System.Serializable]
public class ItemStack
{
    public ItemSO item;
    public int quantity;

    public ItemStack(ItemSO item, int quantity)
    {
        this.item = item;
        this.quantity = quantity;
    }
}

