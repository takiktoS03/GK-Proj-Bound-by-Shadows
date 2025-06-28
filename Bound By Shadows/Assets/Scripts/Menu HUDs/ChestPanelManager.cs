using UnityEngine;

/**
 * @class ChestPanelManager
 * @brief Globalny mened�er odpowiedzialny za stan panelu skrzyni.
 *
 * Umo�liwia sprawdzenie, czy skrzynia jest aktualnie otwarta, oraz jej zamkni�cie z zewn�trznych skrypt�w (np. z menu pauzy).
 * Implementuje wzorzec singletonu do globalnego dost�pu.
 *
 * @author Julia Bigaj
 */
public class ChestPanelManager : MonoBehaviour
{
    /// @brief Statyczna instancja singletonu ChestPanelManager.
    public static ChestPanelManager Instance;

    /// @brief Referencja do panelu skrzyni w UI.
    public GameObject chestPanel;

    /// @brief Referencja do skryptu kontroluj�cego logik� skrzyni.
    public ChestController chestController;

    /**
     * @brief Inicjalizacja singletonu.
     */
    void Awake()
    {
        Instance = this;
    }

    /**
     * @brief Sprawdza, czy skrzynia jest aktualnie otwarta.
     * @return `true` je�li panel skrzyni jest aktywny, w przeciwnym razie `false`.
     */
    public bool IsChestOpen()
    {
        return chestPanel != null && chestPanel.activeSelf;
    }

    /**
     * @brief Zamyka skrzyni� i jej panel z poziomu zewn�trznych system�w (np. menu pauzy).
     */
    public void CloseChest()
    {
        if (chestPanel != null)
            chestPanel.SetActive(false);

        if (chestController != null)
            chestController.CloseChestFromOutside();
    }
}
