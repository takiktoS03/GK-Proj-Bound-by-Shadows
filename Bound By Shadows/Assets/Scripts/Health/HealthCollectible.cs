using UnityEngine;

/**
 * Skrypt przedmiotu kolekcjonerskiego, który po podniesieniu
 * przywraca graczowi określoną ilość zdrowia.
 *
 * @author Filip Kudła
 */

public class HealthCollectible : MonoBehaviour
{
    /// @brief Ilość zdrowia do przywrócenia.
    [SerializeField] private float healthValue;

    /**
     * Wykrywa wejście gracza i przywraca mu zdrowie.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            collision.GetComponent<Health>().Heal(healthValue);
            gameObject.SetActive(false);
        }
    }
}

