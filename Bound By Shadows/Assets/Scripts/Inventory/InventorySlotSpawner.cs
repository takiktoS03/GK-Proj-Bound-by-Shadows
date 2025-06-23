using UnityEngine;
using UnityEngine.UI;

/* Skrypt generuje dynamicznie sloty z listami w interfejsie ekwipunku.
   - Usuwa stare sloty i tworzy nowe na podstawie aktualnie zebranych list.
   - Ka�dy slot zawiera ikon� listu oraz przypisany przycisk, kt�ry wy�wietla tre�� listu po klikni�ciu.

   Autor: Julia Bigaj
*/

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

            // Znajd� ikon�
            Image iconImage = slot.transform.GetChild(0).Find("IconImage").GetComponent<Image>();
            iconImage.sprite = letter.icon;

            // Obs�uga klikni�cia
            Button button = slot.transform.GetChild(0).GetComponent<Button>();
            if (button != null)
            {
                LetterData capturedLetter = letter; // Lokalna kopia dla lambdy
                button.onClick.AddListener(() => {
                    Debug.Log("Klikni�to w list: " + capturedLetter.icon.name);
                    // Przekazujesz ca�e LetterData
                    InventoryUI.Instance.ShowLetterContent(capturedLetter);
                });
            }

            if (button == null)
            {
                Debug.LogError("Nie znaleziono Button w slocie!");
            }
            else
            {
                Debug.Log("Znaleziono Button, przypisuj� listener...");
            }
        }
    }
}
