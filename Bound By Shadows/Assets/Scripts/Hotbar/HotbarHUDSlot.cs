using TMPro;
using UnityEngine;
using UnityEngine.UI;
/**
 * Skrypt aktualizujący wygląd pojedynczego slotu hotbara w interfejsie użytkownika.
 * Wyświetla ikonę przedmiotu oraz jego ilość w ekwipunku.
 *
 * @author Julia Bigaj
 */
public class HotbarHUDSlot : MonoBehaviour
{
    public int slotIndex;
    public Image icon;
    public TextMeshProUGUI countText;

    private void Update()
    {
        ItemSO item = HotbarData.Instance.GetItem(slotIndex);

        if (item == null)
        {
            icon.enabled = false;
            countText.gameObject.SetActive(false);
            return;
        }

        icon.sprite = item.icon;
        icon.enabled = true;

        int count = Inventory.Instance.GetItemCount(item);
        countText.text = count.ToString();
        countText.gameObject.SetActive(count > 1);
    }
}
