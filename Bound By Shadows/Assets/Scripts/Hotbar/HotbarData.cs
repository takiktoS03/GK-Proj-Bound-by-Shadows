using UnityEngine;

public class HotbarData : MonoBehaviour
{
    public static HotbarData Instance;

    public ItemSO[] items = new ItemSO[4];

    private void Awake()
    {
        Instance = this;
    }

    public void SetItem(int index, ItemSO item)
    {
        items[index] = item;
    }

    public ItemSO GetItem(int index)
    {
        return items[index];
    }
}
