using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUIBinder : MonoBehaviour
{
    public Transform slotsParent;
    public GameObject slotPrefab;
    public Image previewImage;
    public TextMeshProUGUI previewText;
    public GameObject previewImageEthan;
    public Button showCharacterButton;
    public HotbarSlot[] hotbarSlots;
    public GameObject previewHotbarRoot;

    private void Start()
    {
        Inventory.Instance.BindUI(
            slotsParent,
            slotPrefab,
            previewImage,
            previewText,
            previewImageEthan,
            showCharacterButton,
            hotbarSlots,
            previewHotbarRoot
        );

        InventoryController.Instance.SetInventoryPanel(gameObject);

        showCharacterButton.onClick.RemoveAllListeners();
        showCharacterButton.onClick.AddListener(() =>
        {
            Inventory.Instance.ShowEthan();
        });

    }

}
