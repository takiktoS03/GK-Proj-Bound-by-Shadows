using UnityEngine;

public class ChestInteract : MonoBehaviour
{
    public static bool IsChestOpen { get; private set; }

    private Chest chest;
    private bool playerInRange;
    private bool isOpen;

    private void Start()
    {
        chest = GetComponentInParent<Chest>();
    }

    private void Update()
    {
        if (!playerInRange)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleChest();
        }
    }

    private void ToggleChest()
    {
        isOpen = !isOpen;

        if (isOpen)
        {
            chest.OpenChest();
            Time.timeScale = 0f;
            IsChestOpen = true;
        }
        else
        {
            chest.CloseChest();
            Time.timeScale = 1f;
            IsChestOpen = false;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
            playerInRange = false;
    }
}
