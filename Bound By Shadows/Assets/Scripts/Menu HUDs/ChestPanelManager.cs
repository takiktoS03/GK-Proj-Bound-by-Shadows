using UnityEngine;

/**
 * @class ChestPanelManager
 * @brief Globalny menedżer odpowiedzialny za stan panelu skrzyni.
 *
 * Umożliwia sprawdzenie, czy skrzynia jest aktualnie otwarta, oraz jej zamknięcie z zewnętrznych skryptów (np. z menu pauzy).
 * Implementuje wzorzec singletonu do globalnego dostępu.
 *
 * @author Julia Bigaj
 */
public class ChestPanelManager : MonoBehaviour
{
    /// @brief Statyczna instancja singletonu ChestPanelManager.
    public static ChestPanelManager Instance;

    /// @brief Referencja do panelu skrzyni w UI.
    public GameObject chestPanel;

    /// @brief Referencja do skryptu kontrolującego logikę skrzyni.
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
     * @return `true` jeśli panel skrzyni jest aktywny, w przeciwnym razie `false`.
     */
    public bool IsChestOpen()
    {
        return chestPanel != null && chestPanel.activeSelf;
    }

    /**
     * @brief Zamyka skrzynię i jej panel z poziomu zewnętrznych systemów (np. menu pauzy).
     */
    public void CloseChest()
    {
        if (chestPanel != null)
            chestPanel.SetActive(false);

        if (chestController != null)
            chestController.CloseChestFromOutside();
    }
}

