using UnityEngine;

/**
 * @class HealthCollectible
 * @brief Skrypt odpowiedzialny za przedmiot przywracaj¹cy zdrowie graczowi.
 *
 * Po zebraniu przez gracza, przywraca okreœlon¹ iloœæ zdrowia i usuwa siê ze sceny.
 *
 * @author Filip Kud³a
 */
public class HealthCollectible : MonoBehaviour
{
    /// @brief Iloœæ zdrowia do przywrócenia.
    [SerializeField] private float healthValue;

    /**
     * Wykrywa wejœcie gracza i przywraca mu zdrowie.
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        collision.GetComponent<Health>().Heal(healthValue);
        gameObject.SetActive(false);
    }
}
