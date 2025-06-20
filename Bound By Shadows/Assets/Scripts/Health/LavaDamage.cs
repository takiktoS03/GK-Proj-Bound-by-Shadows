using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    [SerializeField] private float damage = 25f;
    [SerializeField] private float damageInterval = 0.25f;
    [SerializeField] private float nextDamageTime = 0f;

    private Health health;

    //[Header("Layers to Collide")]
    //[SerializeField] private LayerMask playerLayer;
    //[SerializeField] private LayerMask enemyLayer;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Lava") && Time.time >= nextDamageTime)
        {
            health.TakeDamage(damage);
            nextDamageTime = Time.time + damageInterval;
        }
    }
}