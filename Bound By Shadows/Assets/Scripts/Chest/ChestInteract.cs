using UnityEngine;

public class ChestInteract : MonoBehaviour
{
    private Chest chest;
    private bool playerInRange;

    private void Start()
    {
        chest = GetComponentInParent<Chest>();
    }

    private void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.F))
        {
            chest.OpenChest();
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
