using UnityEngine;

/**
 * Skrypt obsługujący pułapkę kolców, która zadaje obrażenia i odpycha gracza
 * po wejściu w jej obszar kolizji.
 *
 * @author Filip Kudła
 */
public class SpikesTrap : MonoBehaviour
{
    /** @brief Ilość obrażeń zadawanych graczowi. */
    [SerializeField] private float damage;

    /** @brief Siła odrzutu gracza po uderzeniu w kolce. */
    [SerializeField] private float pushForce;

    /**
     * @brief Obsługuje zdarzenie wejścia gracza w strefę kolców.
     * 
     * @param collision Obiekt kolidujący z pułapką (sprawdzany pod kątem tagu "Player").
     */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>()?.TakeDamage(damage);

            Vector2 direction = (collision.transform.position - transform.position).normalized;

            // Resetuje prędkość i odpycha gracza
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.linearVelocity = Vector2.zero;
                rb.AddForce(transform.up * direction * pushForce, ForceMode2D.Impulse);
            }
        }
    }
}

