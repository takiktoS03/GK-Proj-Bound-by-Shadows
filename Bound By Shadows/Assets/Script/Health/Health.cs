using System.Collections;
using EthanTheHero;
using Microlight.MicroBar;
using UnityEngine;


/**
 * Filip Kudła
 * 
 * Skrypt obslugujacy zdrowie gracza i paski zdrowia i staminy
 */
public class Health : MonoBehaviour
{
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private MicroBar staminaBar;
    [SerializeField] private float startingHealth;
    private Animator anim;
    private SpriteRenderer playerSprite;
    public Sprite deathSprite;
    private bool dead = false;

    public float currentHealth { get; private set; }

    private void Awake()
    {
        currentHealth = startingHealth;
        healthBar.Initialize(startingHealth);
        staminaBar.Initialize(startingHealth);
        anim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            TakeDamage(20);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(20);
        }
    }

    public void TakeDamage(float amount)
    {
        if (dead)
        {
            return;
        }
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, startingHealth);

        if (currentHealth > 0)
        {
            anim.SetTrigger("Hurt");
        }
        else if (!dead)
        {
            anim.SetTrigger("Death");
            GetComponent<PlayerMovement>().enabled = false;
            StartCoroutine(FreezeOnDeath());
            playerSprite.sprite = deathSprite;
            dead = true;

            FindObjectOfType<PauseMenu>().ShowGameOver();
        }
        
        healthBar.UpdateBar(healthBar.CurrentValue - amount);
        staminaBar.UpdateBar(staminaBar.CurrentValue - amount);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        healthBar.UpdateBar(healthBar.CurrentValue + amount);
        staminaBar.UpdateBar(staminaBar.CurrentValue + amount);
    }

    private IEnumerator FreezeOnDeath()
    {
        yield return new WaitForSeconds(0.5f); // poczekaj aż klatki śmierci przelecą
        anim.enabled = false;
        playerSprite.sprite = deathSprite;
    }
}
