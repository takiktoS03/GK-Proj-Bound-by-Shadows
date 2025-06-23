using UnityEngine;
using TMPro;
using UnityEngine.UI;

/* Ten skrypt obs�uguje interakcj� gracza ze skrzyni� zawieraj�c� list.
   - Gracz mo�e otworzy� skrzyni� klawiszem "F", gdy jest w pobli�u.
   - Po otwarciu wy�wietlany jest interfejs z listem, a UI zostaje zablokowane.
   - Po ponownym naci�ni�ciu "F" list trafia do ekwipunku.
   - Komponent wsp�pracuje z InventoryManager i UIStateManager.

   Autor: Julia Bigaj
*/

public class ChestController : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private bool isOpen = false;
    private bool isPlayerNear = false;

    public GameObject chestPanel;
    public LetterData letterData;

    public GameObject promptUI;

    [Header("Letter Icon")]
    public Image letterIcon;
    public bool isLetterTaken = false;

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

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            isPlayerNear = false;
            if (promptUI != null)
                promptUI.gameObject.SetActive(false);
        }
    }

    void TakeLetter()
    {
        if (letterIcon != null)
        {
            //InventoryUI.Instance.ShowLetterContent(letterData.content);
            InventoryManager.Instance.AddLetter(letterData);

            letterIcon.transform.parent.gameObject.SetActive(false); // je�li ikona ma rodzica np. slot
            isLetterTaken = true;
            Debug.Log("List zosta� zabrany!");

            UIStateManager.isUIOpen = false;
        }
    }

    public void CloseChestFromOutside()
    {
        isOpen = false;
    }
}
