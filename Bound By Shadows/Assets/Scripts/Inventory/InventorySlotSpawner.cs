using UnityEngine;
using UnityEngine.UI;

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

            // Ustaw ikonkê w slocie (zak³adam, ¿e w InventoryButton jest Image jako pierwsze dziecko)
            Image iconImage = slot.transform.GetChild(0).GetComponent<Image>();
            iconImage.sprite = letter.icon;

            // Obs³uga klikniêcia – znajdŸ komponent Button
            Button button = slot.transform.GetChild(0).GetComponent<Button>();
            if (button != null)
            {
                // Potrzebujesz lokalnej kopii zmiennej "letter" w tej iteracji!
                LetterData capturedLetter = letter;

                button.onClick.AddListener(() => {
                    Debug.Log("Klikniêto w slot listu: " + capturedLetter.icon.name);
                    InventoryUI.Instance.ShowLetterContent(capturedLetter.content);
                });
            }
        }
    }
}
