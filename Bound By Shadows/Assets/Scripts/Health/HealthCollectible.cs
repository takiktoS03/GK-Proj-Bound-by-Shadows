using UnityEngine;

/**
 * @class HealthCollectible
 * @brief Skrypt odpowiedzialny za przedmiot przywracający zdrowie graczowi.
 *
 * Po zebraniu przez gracza, przywraca określoną ilość zdrowia i usuwa się ze sceny.
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
        collision.GetComponent<Health>().Heal(healthValue);
        gameObject.SetActive(false);
    }
}

