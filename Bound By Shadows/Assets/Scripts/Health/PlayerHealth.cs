using System.Collections;
using EthanTheHero;
using Microlight.MicroBar;
using UnityEngine;


/**
 * Filip Kudla
 * 
 * Skrypt obslugujacy zdrowie gracza i paski zdrowia oraz staminy
 */
public class PlayerHealth : Health
{
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private MicroBar staminaBar;
    [SerializeField] private float startingStamina;
    [SerializeField] private float staminaRegenRate = 1f;
    [SerializeField] private float staminaRegenTimeRate = 1f;

    public float currentStamina { get; private set; } //private?
    private PlayerMovement playerMovement;
    private bool hasConsumedStamina = false;

    protected override void Awake()
    {
        base.Awake();
        currentStamina = startingStamina;
        healthBar.Initialize(startingHealth);
        staminaBar.Initialize(startingStamina);
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

    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        healthBar.UpdateBar(currentHealth);        
    }

    public override void Die()
    {
        playerMovement.enabled = false;
        anim.SetTrigger("Death");
        anim.SetTrigger("DeathEnded");
        StartCoroutine(FindFirstObjectByType<PauseMenu>().ShowGameOver());
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

    public override void Heal(float amount)
    {
        base.Heal(amount);
        healthBar.UpdateBar(currentHealth);
    }

    public void HealStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, startingStamina);
        staminaBar.UpdateBar(currentStamina);
    }

    public override void SetHealth(float value)
    {
        base.SetHealth(value);
        healthBar.Initialize(currentHealth);
        staminaBar.Initialize(currentStamina);
        healthBar.UpdateBar(currentHealth);
        staminaBar.UpdateBar(currentStamina);
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
