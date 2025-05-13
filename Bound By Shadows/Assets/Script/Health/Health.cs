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
    public float currentHealth { get; private set; }
    private Animator anim;
    private SpriteRenderer playerSprite;
    public Sprite deathSprite;
    private bool dead = false;

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

            FindFirstObjectByType<PauseMenu>().ShowGameOver();
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

    public void SetHealth(float value)
    {
        Debug.Log(value);
        currentHealth = Mathf.Clamp(value, 0, startingHealth);
        healthBar.Initialize(currentHealth);
        staminaBar.Initialize(currentHealth);
        healthBar.UpdateBar(currentHealth);
        staminaBar.UpdateBar(currentHealth);
    }

    private IEnumerator FreezeOnDeath()
    {
        yield return new WaitForSeconds(0.5f);
        anim.enabled = false;
        playerSprite.sprite = deathSprite;
    }
}
