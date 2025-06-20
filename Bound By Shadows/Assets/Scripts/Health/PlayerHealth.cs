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
    [Header ("Additional bars")]
    [SerializeField] private MicroBar staminaBar;

    [Header("Stamina Parameters")]
    [SerializeField] private float startingStamina;
    [SerializeField] private float staminaRegenRate = 1f;
    [SerializeField] private float staminaRegenTimeRate = 1f;

    [HideInInspector] public float currentStamina { get; private set; }
    private PlayerMovement playerMovement;

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
    }

    //public override void TakeDamage(float amount)
    //{
    //    base.TakeDamage(amount);
    //    healthBar.UpdateBar(currentHealth);        
    //}

    public override void Die()
    {
        playerMovement.enabled = false;
        anim.SetTrigger("Death");
        anim.SetTrigger("DeathEnded");
        StartCoroutine(FindFirstObjectByType<PauseMenu>().ShowGameOver());
    }

    public void TakeStamina(float amount)
    {
        if (currentStamina < amount)
        {
            return;
        }
        currentStamina = Mathf.Clamp(currentStamina - amount, 0, startingStamina);
        staminaBar.UpdateBar(currentStamina - amount);
    }

    //public override void Heal(float amount)
    //{
    //    base.Heal(amount);
    //    healthBar.UpdateBar(currentHealth);
    //}

    public void HealStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, startingStamina);
        staminaBar.UpdateBar(currentStamina);
    }

    public override void SetBarsValue(float value)
    {
        base.SetBarsValue(value);
        staminaBar.Initialize(currentStamina);
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
