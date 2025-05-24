using UnityEngine;

public class BarrelHitbox : MonoBehaviour
{
    private IDamageable target;

    private void Awake()
    {
        target = GetComponentInParent<IDamageable>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("PlayerAttack"))
        {
            target?.OnHit();
        }
    }
}
