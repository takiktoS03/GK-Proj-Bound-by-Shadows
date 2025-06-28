using UnityEngine;

/**
 * @class LavaDamage
 * @brief Skrypt zadający obrażenia graczowi przy kontakcie z lawą.
 *
 * Po wejściu w strefę lawy, odejmuje graczowi określoną ilość zdrowia i odrzuca go.
 *
 * @author Filip Kudła
 */
public class LavaDamage : MonoBehaviour
{
    /// @brief Ilość obrażeń zadanych przez lawę.
    [SerializeField] private float damage = 25f;
    /// @brief Minimalny odstęp czasowy między kolejnymi obrażeniami.
    [SerializeField] private float damageInterval = 0.25f;
    /// @brief Czas, po którym możliwe będzie kolejne zadanie obrażeń.
    [SerializeField] private float nextDamageTime = 0f;

    private Health health;

    //[Header("Layers to Collide")]
    //[SerializeField] private LayerMask playerLayer;
    //[SerializeField] private LayerMask enemyLayer;

    /**
     * Inicjalizacja komponentu Health przypisanego do obiektu.
     * Umożliwia późniejsze wywoływanie metody TakeDamage().
     */
    private void Awake()
    {
        health = GetComponent<Health>();
    }

    /**
     * Reakcja na przebywanie w strefie lawy — cykliczne odbieranie zdrowia.
     *
     * @param collision Obiekt kolidujący z lawą.
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
