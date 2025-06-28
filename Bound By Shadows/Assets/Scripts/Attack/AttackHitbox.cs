using UnityEngine;


/**
 * @class AttackHitbox
 * @brief Odpowiada za detekcję kolizji i zadawanie obrażeń.
 *
 * Tworzony dynamicznie prefab hitboxa, który sprawdza kolizję z przeciwnikiem.
 * Gdy wykryje obiekt z komponentem Health, zadaje mu obrażenia.
 *
 * @author Filip Kudła
 */
public class AttackHitbox : MonoBehaviour
{
    /// @brief Warstwa przeciwników, z którymi może kolidować atak.
    [SerializeField] private LayerMask enemyLayer;

    private GameObject owner;
    private float damage;
    private float knockback;

    /**
   * @brief Sprawdzenie kolizji w momencie wejścia do triggera.
   * @param collision Collider obiektu wchodzącego w trigger.
   */
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject == owner) return;
        //if (collision.IsTouchingLayers(enemyLayer))
        {
            Health targetHealth = collision.GetComponent<Health>();
            if (targetHealth != null)
            {
                targetHealth.TakeDamage(damage);
            }
        }
    }

    /**
    * @brief Inicjalizuje hitbox danymi ataku.
    * @param dmg Obrażenia do zadania.
    * @param kb Siła odrzutu.
    * @param source Obiekt będący właścicielem ataku.
    */
    public void Init(float dmg, float kb, GameObject source)
    {
        damage = dmg;
        knockback = kb;
        owner = source;
    }
}
