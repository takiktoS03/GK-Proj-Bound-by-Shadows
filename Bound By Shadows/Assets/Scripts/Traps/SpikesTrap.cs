using EthanTheHero;
using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float pushForce;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            collision.GetComponent<Rigidbody2D>().AddForceY(pushForce, ForceMode2D.Impulse);
        }
    }
}
