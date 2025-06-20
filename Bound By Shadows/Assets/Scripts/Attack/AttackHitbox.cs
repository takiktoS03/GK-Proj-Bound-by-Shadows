using UnityEngine;


/**
 *  Skrypt inicjujący collider służący do ataku, zadający uszkodzenia jeśli przeciwnik jest w strefie
 *  
 *  Autor: Filip Kudła
 */
public class AttackHitbox : MonoBehaviour
{
    [SerializeField] private LayerMask enemyLayer;
    private GameObject owner;
    private float damage;
    private float knockback;

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

    public void Init(float dmg, float kb, GameObject source)
    {
        damage = dmg;
        knockback = kb;
        owner = source;
    }
}
