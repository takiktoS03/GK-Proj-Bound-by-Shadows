using UnityEngine;

public class InventorySlotSpawner : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotsParent;
    public int numberOfSlots = 20;

    void Start()
    {
        Debug.Log("Spawnujemy sloty!");

        for (int i = 0; i < numberOfSlots; i++)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);
            slot.SetActive(true);
        }
    }
}
