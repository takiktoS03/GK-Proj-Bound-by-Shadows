using System.Collections;
using EthanTheHero;
using Microlight.MicroBar;
using UnityEngine;


/**
 * @class PlayerHealth
 * @brief Klasa zarządzająca zdrowiem i wytrzymałością gracza.
 *
 * Dziedziczy po klasie `Health` i rozszerza ją o obsługę paska staminy, jej zużywania i regeneracji.
 * Integruje się z paskami zdrowia i staminy z Microlight.MicroBar, a także zatrzymuje ruch gracza po śmierci.
 *
 * @author Filip Kudła
 */
public class PlayerHealth : Health
{
    [Header ("Additional bars")]
    /// @brief Pasek staminy (wytrzymałości) gracza.
    [SerializeField] private MicroBar staminaBar;

    [Header("Stamina Parameters")]
    /// @brief Początkowa wartość staminy.
    [SerializeField] private float startingStamina;
    /// @brief Ile staminy regeneruje się na cykl.
    [SerializeField] private float staminaRegenRate = 1f;
    /// @brief Czas między regeneracjami staminy.
    [SerializeField] private float staminaRegenTimeRate = 1f;

    /// @brief Aktualna ilość staminy (dostępna tylko do odczytu).
    [HideInInspector] public float currentStamina { get; private set; }
    private PlayerMovement playerMovement;

    /**
     * Inicjalizuje zdrowie i staminy oraz komponent ruchu.
     */
    protected override void Awake()
    {
        base.Awake();
        currentStamina = startingStamina;
        healthBar.Initialize(startingHealth);
        staminaBar.Initialize(startingStamina);
        playerMovement = GetComponent<PlayerMovement>();
    }

    /**
     * Startuje proces pasywnej regeneracji staminy.
     */
    private void Start()
    {
        StartCoroutine(RegenerateStaminaCoroutine());
    }

    /**
    * Diagnostyczne skróty T/R do testowania staminy/leczenia.
    */
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
    //    dodanie dzwiekow Hurt     
    //}

    /**
     * Przeciąża metodę śmierci: animacja i ekran końcowy.
     */
    public override void Die()
    {
        playerMovement.enabled = false;
        anim.SetTrigger("Death");
        anim.SetTrigger("DeathEnded");
        StartCoroutine(FindFirstObjectByType<PauseMenu>().ShowGameOver());
    }

    /**
     * Odbiera określoną ilość staminy, aktualizując pasek.
     */
    public void TakeStamina(float amount)
    {
        if (currentStamina < amount)
        {
            return;
        }
        currentStamina = Mathf.Clamp(currentStamina - amount, 0, startingStamina);
        staminaBar.UpdateBar(currentStamina);
    }

    //public override void Heal(float amount)
    //{
    //    base.Heal(amount);
    //    dodanie dzwiekow Heal
    //}

    /**
     * Odbieranie HP wraz z zaimplementowanym dźwiękiem.
     */
    public override void TakeDamage(float amount)
    {
        base.TakeDamage(amount);
        SoundManager.Instance?.PlayHurt(); // ← DŹWIĘK OBRAŻEŃ
    }

    /**
     * Lepsza wersja odzyskiwania staminy.
     */
    public void HealStamina(float amount)
    {
        currentStamina = Mathf.Clamp(currentStamina + amount, 0, startingStamina);
        staminaBar.UpdateBar(currentStamina);
    }

    /**
     * Ustawia wartość pasków zdrowia i staminy.
     */
    public override void SetBarsValue(float value)
    {
        base.SetBarsValue(value);
        staminaBar.Initialize(currentStamina);
        staminaBar.UpdateBar(currentStamina);
    }

    /**
     * Coroutine odpowiedzialna za automatyczną regenerację staminy.
     */
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

