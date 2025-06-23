using UnityEngine;
using TMPro;

/* Wyœwietla podpowiedŸ tekstow¹, gdy gracz wchodzi w okreœlon¹ strefê (trigger).
   - Wspó³pracuje z `HintController`, by pokazaæ/ukryæ wiadomoœæ.

   Autor: Julia Bigaj
*/

public class HintArea : MonoBehaviour
{
    public string message;
    private bool playerInside = false;
    private HintController hintController;

    private void Start()
    {
        hintController = FindObjectOfType<HintController>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            hintController.ShowHint(message);
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            hintController.HideHint();
        }
    }
}
