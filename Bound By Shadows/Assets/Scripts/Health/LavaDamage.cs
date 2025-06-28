using UnityEngine;

/**
 * @class LavaDamage
 * @brief Skrypt zadaj�cy obra�enia graczowi przy kontakcie z law�.
 *
 * Po wej�ciu w stref� lawy, odejmuje graczowi okre�lon� ilo�� zdrowia i odrzuca go.
 *
 * @author Filip Kud�a
 */
public class LavaDamage : MonoBehaviour
{
    /// @brief Ilo�� obra�e� zadanych przez law�.
    [SerializeField] private float damage = 25f;
    /// @brief Minimalny odst�p czasowy mi�dzy kolejnymi obra�eniami.
    [SerializeField] private float damageInterval = 0.25f;
    /// @brief Czas, po kt�rym mo�liwe b�dzie kolejne zadanie obra�e�.
    [SerializeField] private float nextDamageTime = 0f;

    private Health health;

    //[Header("Layers to Collide")]
    //[SerializeField] private LayerMask playerLayer;
    //[SerializeField] private LayerMask enemyLayer;

    /**
     * Inicjalizacja komponentu Health przypisanego do obiektu.
     * Umo�liwia p�niejsze wywo�ywanie metody TakeDamage().
     */
    private void Awake()
    {
        health = GetComponent<Health>();
    }

    /**
     * Reakcja na przebywanie w strefie lawy � cykliczne odbieranie zdrowia.
     *
     * @param collision Obiekt koliduj�cy z law�.
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