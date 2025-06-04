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
    [SerializeField] private float startingStamina;
    [SerializeField] private float staminaRegenRate = 1f;
    [SerializeField] private float staminaRegenTimeRate = 1f;

    public float currentHealth { get; private set; }
    public float currentStamina { get; private set; }

    private Animator anim;
    private SpriteRenderer playerSprite;
    private PlayerMovement playerMovement;
    public Sprite deathSprite;

    private bool dead = false;
    private bool hasConsumedStamina = false;

    private void Awake()
    {
        currentHealth = startingHealth;
        currentStamina = startingStamina;
        healthBar.Initialize(startingHealth);
        staminaBar.Initialize(startingStamina);
        anim = GetComponent<Animator>();
        playerSprite = GetComponent<SpriteRenderer>();
        playerMovement = GetComponent<PlayerMovement>();
    }

    private void Start()
    {
        StartCoroutine(RegenerateStaminaCoroutine());
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            TakeDamage(20);
            TakeStamina(20);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(20);
            HealStamina(20);
        }

        if (playerMovement.isDashing)
        {
            if (!hasConsumedStamina)
            {
                TakeStamina(20);
                hasConsumedStamina = true;
            }
        }
        else
        {
            hasConsumedStamina = false;
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
    }

    public void TakeStamina(float amount)
    {
        if (currentStamina <= 0)
        {
            return;
        }
        currentStamina = Mathf.Clamp(currentStamina - amount, 0, startingStamina);
        staminaBar.UpdateBar(staminaBar.CurrentValue - amount);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        healthBar.UpdateBar(currentHealth);
    }

    public void HealStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, startingStamina);
        staminaBar.UpdateBar(currentStamina);
    }

    public void SetHealth(float value)
    {
        Debug.Log(value);
        currentHealth = Mathf.Clamp(value, 0, startingHealth);
        healthBar.Initialize(currentHealth);
        staminaBar.Initialize(currentStamina);
        healthBar.UpdateBar(currentHealth);
        staminaBar.UpdateBar(currentStamina);
    }

    private IEnumerator FreezeOnDeath()
    {
        yield return new WaitForSeconds(0.5f);
        anim.enabled = false;
        playerSprite.sprite = deathSprite;
    }

    private IEnumerator RegenerateStaminaCoroutine()
    {
        while (true)
        {
            if (!playerMovement.isDashing && currentStamina < startingStamina)
            {
                HealStamina(staminaRegenRate);
            }
            yield return new WaitForSeconds(staminaRegenTimeRate);
        }
    }
}
