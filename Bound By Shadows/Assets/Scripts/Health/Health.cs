using System.Collections;
using EthanTheHero;
using Microlight.MicroBar;
using UnityEngine;


/**
 * Filip Kudla
 * 
 * Skrypt obslugujacy zdrowie wszystkich 'zyjacych' istot
 */
public class Health : MonoBehaviour
{
    [SerializeField] protected float startingHealth;
    [SerializeField] protected float damageCooldown = 0.75f;

    protected float currentHealth;
    protected Animator anim;
    protected SpriteRenderer spriteRend;

    private bool dead = false;
    protected bool canTakeDamage = true;

    protected virtual void Awake()
    {
        currentHealth = startingHealth;
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    public virtual void TakeDamage(float amount)
    {
        if (dead || !canTakeDamage)
        {
            return;
        }
        StartCoroutine(DamageCooldownCoroutine());

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
        }
        else
        {
            Die();
        }
    }
    public virtual void Die()
    {
        
        Destroy(gameObject);
    }

    public virtual void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
    }

    public virtual void SetHealth(float value)
    {
        Debug.Log(value);
        currentHealth = Mathf.Clamp(value, 0, startingHealth);
    }

    protected virtual IEnumerator DamageCooldownCoroutine()
    {
        canTakeDamage = false;
        spriteRend.color = new Color(1, 0, 0, 0.8f);
        yield return new WaitForSeconds(damageCooldown);
        spriteRend.color = Color.white;
        canTakeDamage = true;
        anim.SetTrigger("HurtEnded");
    }
}
