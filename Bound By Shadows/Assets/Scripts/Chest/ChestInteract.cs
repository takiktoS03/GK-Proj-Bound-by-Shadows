using UnityEngine;
/**
 * Skrypt umożliwiający graczowi interakcję ze skrzynią
 * po wejściu w jej obszar i naciśnięciu klawisza akcji.
 *
 * @author Julia Bigaj
 */
public class ChestInteract : MonoBehaviour
{
    private Chest chest;
    private bool playerInRange;
    //private bool isOpen;

    private void Start()
    {
        chest = GetComponentInParent<Chest>();
    }

    private void Update()
    {
        if(!playerInRange)
            return;

        if (Input.GetKeyDown(KeyCode.F))
        {
            ToggleChest();
        }
    }

    private void ToggleChest()
    {
        if (!chest.isOpened)
        {
            chest.OpenChest();
            Time.timeScale = 0f;
        }
        else
        {
            chest.TakeAll();
            //chest.CloseChest();
            Time.timeScale = 1f;
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
