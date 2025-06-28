using UnityEngine;

/**
 * @class LavaDamage
 * @brief Skrypt zadaj¹cy obra¿enia graczowi przy kontakcie z law¹.
 *
 * Po wejœciu w strefê lawy, odejmuje graczowi okreœlon¹ iloœæ zdrowia i odrzuca go.
 *
 * @author Filip Kud³a
 */
public class LavaDamage : MonoBehaviour
{
    /// @brief Iloœæ obra¿eñ zadanych przez lawê.
    [SerializeField] private float damage = 25f;
    /// @brief Minimalny odstêp czasowy miêdzy kolejnymi obra¿eniami.
    [SerializeField] private float damageInterval = 0.25f;
    /// @brief Czas, po którym mo¿liwe bêdzie kolejne zadanie obra¿eñ.
    [SerializeField] private float nextDamageTime = 0f;

    private Health health;

    //[Header("Layers to Collide")]
    //[SerializeField] private LayerMask playerLayer;
    //[SerializeField] private LayerMask enemyLayer;

    /**
     * Inicjalizacja komponentu Health przypisanego do obiektu.
     * Umo¿liwia póŸniejsze wywo³ywanie metody TakeDamage().
     */
    private void Awake()
    {
        health = GetComponent<Health>();
    }

    /**
     * Reakcja na przebywanie w strefie lawy — cykliczne odbieranie zdrowia.
     *
     * @param collision Obiekt koliduj¹cy z law¹.
     */
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Lava") && Time.time >= nextDamageTime)
        {
            health.TakeDamage(damage);
            nextDamageTime = Time.time + damageInterval;
        }
    }
}