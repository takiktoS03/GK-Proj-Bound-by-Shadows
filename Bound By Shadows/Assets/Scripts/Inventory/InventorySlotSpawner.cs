using UnityEngine;
using UnityEngine.UI;

/* Skrypt generuje dynamicznie sloty z listami w interfejsie ekwipunku.
   - Usuwa stare sloty i tworzy nowe na podstawie aktualnie zebranych list.
   - Ka¿dy slot zawiera ikonê listu oraz przypisany przycisk, który wyœwietla treœæ listu po klikniêciu.

   Autor: Julia Bigaj
*/

public class InventorySlotSpawner : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotsParent;

    public void RefreshSlots()
    {
        Debug.Log("Odœwie¿am sloty w ekwipunku!");

        // Usuñ stare sloty
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        // Dla ka¿dego listu generujemy slot
        foreach (var letter in InventoryManager.Instance.collectedLetters)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);
            slot.SetActive(true);

            // ZnajdŸ ikonê
            Image iconImage = slot.transform.GetChild(0).Find("IconImage").GetComponent<Image>();
            iconImage.sprite = letter.icon;

            // Obs³uga klikniêcia
            Button button = slot.transform.GetChild(0).GetComponent<Button>();
            if (button != null)
            {
                LetterData capturedLetter = letter; // Lokalna kopia dla lambdy
                button.onClick.AddListener(() => {
                    Debug.Log("Klikniêto w list: " + capturedLetter.icon.name);
                    // Przekazujesz ca³e LetterData
                    InventoryUI.Instance.ShowLetterContent(capturedLetter);
                });
            }

            if (button == null)
            {
                Debug.LogError("Nie znaleziono Button w slocie!");
            }
            else
            {
                Debug.Log("Znaleziono Button, przypisujê listener...");
            }
        }
    }
}
