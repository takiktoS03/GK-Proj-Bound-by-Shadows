using UnityEngine;
using UnityEngine.UI;

public class InventorySlotSpawner : MonoBehaviour
{
    public GameObject slotPrefab;
    public Transform slotsParent;

    public void RefreshSlots()
    {
        Debug.Log("Od�wie�am sloty w ekwipunku!");

        // Usu� stare sloty
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        // Dla ka�dego listu generujemy slot
        foreach (var letter in InventoryManager.Instance.collectedLetters)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);
            slot.SetActive(true);

            // Ustaw ikonk� w slocie (zak�adam, �e w InventoryButton jest Image jako pierwsze dziecko)
            Image iconImage = slot.transform.GetChild(0).GetComponent<Image>();
            iconImage.sprite = letter.icon;

            // Obs�uga klikni�cia � znajd� komponent Button
            Button button = slot.transform.GetChild(0).GetComponent<Button>();
            if (button != null)
            {
                // Potrzebujesz lokalnej kopii zmiennej "letter" w tej iteracji!
                LetterData capturedLetter = letter;

                button.onClick.AddListener(() => {
                    Debug.Log("Klikni�to w slot listu: " + capturedLetter.icon.name);
                    InventoryUI.Instance.ShowLetterContent(capturedLetter.content);
                });
            }
        }
    }
}
