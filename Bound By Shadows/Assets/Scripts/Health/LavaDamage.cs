using UnityEngine;

public class LavaDamage : MonoBehaviour
{
    private Health health;
    [SerializeField] private float damageInterval = 0.5f;
    [SerializeField] private float nextDamageTime = 0f;

    private void Awake()
    {
        health = GetComponent<Health>();
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Lava") && Time.time >= nextDamageTime)
        {
            health.TakeDamage(15);
            nextDamageTime = Time.time + damageInterval;
        }
    }
}
