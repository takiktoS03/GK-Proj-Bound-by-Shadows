using UnityEngine;
using UnityEngine.UI;

/**
 * @class InventorySlotSpawner
 * @brief Generuje dynamiczne sloty listów w UI ekwipunku.
 *
 * Usuwa stare sloty, tworzy nowe na podstawie zebranych listów
 * oraz przypisuje odpowiednie przyciski do wyświetlania ich zawartości.
 *
 * @author Julia Bigaj
 */
public class InventorySlotSpawner : MonoBehaviour
{
    /// @brief Prefab pojedynczego slotu listu.
    public GameObject slotPrefab;
    /// @brief Rodzic (kontener) dla wygenerowanych slotów.
    public Transform slotsParent;

    /**
     * @brief Odświeża wszystkie sloty z listami w interfejsie użytkownika.
     *
     * Usuwa poprzednie sloty i generuje nowe na podstawie aktualnego stanu ekwipunku.
     */
    public void RefreshSlots()
    {
        Debug.Log("Odświeżam sloty w ekwipunku!");

        // Usuń stare sloty
        foreach (Transform child in slotsParent)
        {
            Destroy(child.gameObject);
        }

        // Dla każdego listu generujemy slot
        foreach (var letter in InventoryManager.Instance.collectedLetters)
        {
            GameObject slot = Instantiate(slotPrefab, slotsParent);
            slot.SetActive(true);

            // Znajdź ikonę
            Image iconImage = slot.transform.GetChild(0).Find("IconImage").GetComponent<Image>();
            iconImage.sprite = letter.icon;

            // Obsługa kliknięcia
            Button button = slot.transform.GetChild(0).GetComponent<Button>();
            if (button != null)
            {
                LetterData capturedLetter = letter; // Lokalna kopia dla lambdy
                button.onClick.AddListener(() => {
                    Debug.Log("Kliknięto w list: " + capturedLetter.icon.name);
                    // Przekazujesz całe LetterData
                    InventoryUI.Instance.ShowLetterContent(capturedLetter);
                });
            }

            if (button == null)
            {
                Debug.LogError("Nie znaleziono Button w slocie!");
            }
            else
            {
                Debug.Log("Znaleziono Button, przypisuję listener...");
            }
        }
    }
}

