using UnityEngine;

public class ChestPanelManager : MonoBehaviour
{
    public static ChestPanelManager Instance;

    public GameObject chestPanel;
    public ChestController chestController;

    void Awake()
    {
        Instance = this;
    }

    public bool IsChestOpen()
    {
        return chestPanel != null && chestPanel.activeSelf;
    }

    public void CloseChest()
    {
        if (chestPanel != null)
            chestPanel.SetActive(false);

        if (chestController != null)
            chestController.CloseChestFromOutside();
    }
}
