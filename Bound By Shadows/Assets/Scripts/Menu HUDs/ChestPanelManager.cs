using UnityEngine;

/**
 * @class ChestPanelManager
 * @brief Globalny mened¿er odpowiedzialny za stan panelu skrzyni.
 *
 * Umo¿liwia sprawdzenie, czy skrzynia jest aktualnie otwarta, oraz jej zamkniêcie z zewnêtrznych skryptów (np. z menu pauzy).
 * Implementuje wzorzec singletonu do globalnego dostêpu.
 *
 * @author Julia Bigaj
 */
public class ChestPanelManager : MonoBehaviour
{
    /// @brief Statyczna instancja singletonu ChestPanelManager.
    public static ChestPanelManager Instance;

    /// @brief Referencja do panelu skrzyni w UI.
    public GameObject chestPanel;

    /// @brief Referencja do skryptu kontroluj¹cego logikê skrzyni.
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
     * @return `true` jeœli panel skrzyni jest aktywny, w przeciwnym razie `false`.
     */
    public bool IsChestOpen()
    {
        return chestPanel != null && chestPanel.activeSelf;
    }

    /**
     * @brief Zamyka skrzyniê i jej panel z poziomu zewnêtrznych systemów (np. menu pauzy).
     */
    public void CloseChest()
    {
        if (chestPanel != null)
            chestPanel.SetActive(false);

        if (chestController != null)
            chestController.CloseChestFromOutside();
    }
}
