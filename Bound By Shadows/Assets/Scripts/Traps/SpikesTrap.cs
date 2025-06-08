using EthanTheHero;
using UnityEngine;

public class SpikesTrap : MonoBehaviour
{
    [SerializeField] private float damage;
    [SerializeField] private float pushForce;

    //private void OnTriggerStay2D(Collider2D collision)
    //{
    //    if (collision.tag == "Player")
    //    {
    //        collision.GetComponent<Health>().TakeDamage(damage);
    //    }
    //}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            collision.GetComponent<Health>().TakeDamage(damage);
            Vector2 direction = (collision.transform.position - transform.position).normalized;

            // resetowanie domyslnego 'uklucia' i nadanie sily
            Rigidbody2D rb = collision.GetComponent<Rigidbody2D>();
            rb.linearVelocity = Vector2.zero;
            rb.AddForce(transform.up * direction * pushForce, ForceMode2D.Impulse);
        }
    }
}
