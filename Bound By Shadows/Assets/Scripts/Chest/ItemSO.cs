using UnityEngine;
/**
 * ScriptableObject reprezentujący przedmiot w grze wraz z jego danymi
 * oraz dodatkowymi informacjami wykorzystywanymi w ekwipunku i hotbarze.
 *
 * @author Julia Bigaj
 */

public enum ItemType
{
    Letter,
    HealPotion,
    StaminaPotion,
    Key,
    Other
}

[CreateAssetMenu(fileName = "NewItem", menuName = "Items/Item")]
public class ItemSO : ScriptableObject
{
    public string itemName;
    public Sprite icon;
    public ItemType itemType;

    // Dodatkowe dane – np. dla mikstur
    public int healAmount;

    // dodatkowe dane dla staminy
    public int staminaAmount;

    // Dodatkowe dane dla listu
    [TextArea(5, 10)]
    public string letterText;

    public bool hasTextPreview;
    [TextArea(5, 15)]
    public string textPreview;

    public bool hasImagePreview;
    public Sprite imagePreview;

    [Header("Hotbar")]
    public bool canBeInHotbar;
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

