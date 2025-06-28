using UnityEngine;
using TMPro;
using UnityEngine.UI;

/**
 * @class ChestController
 * @brief Otwiera skrzynię po interakcji gracza i dodaje przedmiot (list) do ekwipunku.
 *
 * Odpowiada za animację otwierania skrzyni, interakcję z graczem,
 * wyświetlanie UI oraz dodanie listu do systemu ekwipunku.
 *
 * @author Julia Bigaj
 */
public class ChestController : MonoBehaviour
{
    /// @brief Referencja do komponentu Animator.
    private Animator animator;
    /// @brief Czy skrzynia jest otwarta.
    [SerializeField] private bool isOpen = false;
    /// @brief Czy gracz znajduje się w pobliżu skrzyni.
    private bool isPlayerNear = false;

    /// @brief Panel UI wyświetlany po otwarciu skrzyni.
    public GameObject chestPanel;
    /// @brief Dane przechowywanego listu.
    public LetterData letterData;

    /// @brief UI z podpowiedzią do interakcji (np. naciśnij "F").
    public GameObject promptUI;

    [Header("Letter Icon")]
    /// @brief Ikona reprezentująca list w UI skrzyni.
    public Image letterIcon;
    /// @brief Czy list został już zabrany przez gracza.
    public bool isLetterTaken = false;

    /**
     * @brief Inicjalizacja komponentów i domyślnych stanów UI.
     */
    void Start()
    {
        animator = GetComponent<Animator>();
        if (promptUI != null)
            promptUI.gameObject.SetActive(false);
        if (chestPanel != null)
            chestPanel.SetActive(false);
        if (letterIcon != null)
            letterIcon.gameObject.SetActive(true);
    }

    /**
     * @brief Obsługa interakcji gracza z obiektem (otwieranie skrzyni, zabieranie listu).
     */
    void Update()
    {
        if (isPlayerNear && !isOpen && Input.GetKeyDown(KeyCode.F))
        {
            animator.SetTrigger("Open");
            isOpen = true;

            SoundManager.Instance?.PlayChest();

            if (promptUI != null)
                promptUI.gameObject.SetActive(false);
            if (chestPanel != null)
                chestPanel.SetActive(true);

            UIStateManager.isUIOpen = true;
        }

        else if (isOpen && !isLetterTaken && Input.GetKeyDown(KeyCode.F))
        {
            TakeLetter();
        }
    }

    /**
     * @brief Wykrywa wejście gracza w obszar interakcji.
     * @param other Obiekt kolidujący z triggerem.
     */
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger enter: " + other.name);

        if (other.CompareTag("Player") && !isOpen)
        {
            isPlayerNear = true;
            if (promptUI != null)
                promptUI.SetActive(true);
        }
    }

    /**
     * @brief Wykrywa opuszczenie obszaru interakcji przez gracza.
     * @param other Obiekt opuszczający trigger.
     */
    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (promptUI != null)
                promptUI.gameObject.SetActive(false);
        }
    }

    /**
     * @brief Zbiera list i dodaje go do ekwipunku.
     */
    void TakeLetter()
    {
        if (letterIcon != null)
        {
            //InventoryUI.Instance.ShowLetterContent(letterData.content);
            InventoryManager.Instance.AddLetter(letterData);

            letterIcon.transform.parent.gameObject.SetActive(false); // jeśli ikona ma rodzica np. slot
            isLetterTaken = true;
            Debug.Log("List został zabrany!");

            UIStateManager.isUIOpen = false;
        }
    }

    /**
     * @brief Zamyka skrzynię z zewnętrznych skryptów.
     */
    public void CloseChestFromOutside()
    {
        isOpen = false;
    }
}

