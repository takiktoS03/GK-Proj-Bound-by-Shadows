using UnityEngine;
using UnityEngine.UI;

/**
 * @class InventorySlotSpawner
 * @brief Generuje dynamiczne sloty list�w w UI ekwipunku.
 *
 * Usuwa stare sloty, tworzy nowe na podstawie zebranych list�w
 * oraz przypisuje odpowiednie przyciski do wy�wietlania ich zawarto�ci.
 *
 * @author Julia Bigaj
 */
public class InventorySlotSpawner : MonoBehaviour
{
    /// @brief Prefab pojedynczego slotu listu.
    public GameObject slotPrefab;
    /// @brief Rodzic (kontener) dla wygenerowanych slot�w.
    public Transform slotsParent;

    /**
     * @brief Od�wie�a wszystkie sloty z listami w interfejsie u�ytkownika.
     *
     * Usuwa poprzednie sloty i generuje nowe na podstawie aktualnego stanu ekwipunku.
     */
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
