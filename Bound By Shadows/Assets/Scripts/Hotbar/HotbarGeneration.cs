using UnityEngine;
using TMPro;

public class HotbarGenerator : MonoBehaviour
{
    public GameObject slotPrefab;   // tu wrzucisz swój HotbarSlot prefab
    public int slotCount = 10;      // ile slotów wygenerowa? (1–0)

    void Start()
    {
        GenerateSlots();
    }

    void GenerateSlots()
    {
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(slotPrefab, transform);

            // pobieramy komponent TMP numeru klawisza:
            TextMeshProUGUI keyNumber = slot.transform.Find("KeyNumber")
                                                       .GetComponent<TextMeshProUGUI>();

            // numer w UI (1–9 i 0)
            int displayNumber = (i + 1) % 10;
            keyNumber.text = displayNumber.ToString();

            // opcjonalnie mo?na nazwa? slot w Hierarchy:
            slot.name = "Slot_" + displayNumber;
        }
    }
}
