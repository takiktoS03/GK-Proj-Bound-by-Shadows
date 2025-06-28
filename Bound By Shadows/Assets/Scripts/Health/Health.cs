using System.Collections;
using Microlight.MicroBar;
using UnityEngine;

/**
 * @class Health
 * @brief Bazowa klasa obsługująca zdrowie dla wszelkich istot w grze.
 *
 * Oferuje podstawową logikę odbierania/leczenia obrażeń, obsługę nieśmiertelności oraz pasek zdrowia.
 * Może być dziedziczona przez inne klasy (np. `PlayerHealth`, `EnemyHealth`).
 *
 * @author Filip Kudła
 */
public class Health : MonoBehaviour
{
    /// @brief Początkowa wartość zdrowia.
    [Header("Health of Entity")]
    [SerializeField] protected float startingHealth;

    /// @brief Pasek zdrowia.
    [Header("Health Bar")]
    [SerializeField] protected MicroBar healthBar;

    /// @brief Czas nieśmiertelności po otrzymaniu obrażeń.
    [Header("Invulnerability Parameters")]
    [SerializeField] protected float damageCooldown = 0.75f;

    protected float currentHealth;
    protected Animator anim;
    protected SpriteRenderer spriteRend;

    private bool dead = false;
    protected bool canTakeDamage = true;

    /**
     * Inicjalizuje komponenty i pasek zdrowia.
     */
    protected virtual void Awake()
    {
        currentHealth = startingHealth;
        healthBar.Initialize(startingHealth);
        anim = GetComponent<Animator>();
        spriteRend = GetComponent<SpriteRenderer>();
    }

    /**
     * Odbiera obrażenia, aktualizuje pasek zdrowia i wywołuje śmierć, jeśli HP spadnie do zera.
     */
    public virtual void TakeDamage(float amount)
    {
        if (dead || !canTakeDamage)
        {
            return;
        }
        StartCoroutine(DamageCooldownCoroutine());

        currentHealth = Mathf.Clamp(currentHealth - amount, 0, startingHealth);
        healthBar.UpdateBar(currentHealth);
        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
        }
        else
        {
            Die();
            dead = true;
        }
    }

    /**
     * Domyślna logika śmierci - usunięcie obiektu.
     */
    public virtual void Die()
    {
        
        Destroy(gameObject);
    }

    /**
     * Leczy postać o podaną wartość.
     */
    public virtual void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        healthBar.UpdateBar(currentHealth);
    }

    /**
     * Ustawia konkretną wartość na pasku zdrowia.
     */
    public virtual void SetBarsValue(float value)
    {
        Debug.Log(value);
        currentHealth = Mathf.Clamp(value, 0, startingHealth);

        healthBar.Initialize(currentHealth);
        healthBar.UpdateBar(currentHealth);
    }

    /**
     * Coroutine - krótki czas nieśmiertelności po otrzymaniu obrażeń.
     */
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
