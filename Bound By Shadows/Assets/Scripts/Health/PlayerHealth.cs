using EthanTheHero;
using Microlight.MicroBar;
using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;


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

    [Header("Damage Effects")]
    [SerializeField] private Volume damageVolume;
    [SerializeField] private CameraController cameraShake;
    [SerializeField] private float effectDuration = 2f;
    [SerializeField] private float maxShakeMagnitude = 0.3f;
    [SerializeField] private LightColorChange ghostLight;

    /// @brief Aktualna ilość staminy (dostępna tylko do odczytu).
    [HideInInspector] public float currentStamina { get; private set; }
    private PlayerMovement playerMovement;
    private PlayerAttackMethod playerAttack;
    private Coroutine damageEffectCoroutine;

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
        playerAttack = GetComponent<PlayerAttackMethod>();
        if (damageVolume != null) damageVolume.weight = 0;
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
            TakeDamage(50);
            TakeStamina(20);
        }
        else if (Input.GetKeyDown(KeyCode.R))
        {
            Heal(100);
            HealStamina(100);
        }
    }

    /**
     * Odbieranie HP wraz z zaimplementowanym dźwiękiem.
     */
    public override bool TakeDamage(float amount)
    {
        if (base.TakeDamage(amount))
        {
            SoundLibrary.Instance.PlayHurt();
            if (ghostLight != null)
            {
                ghostLight.SetDanger();
            }

            float damagePercent = Mathf.Clamp01(amount / startingHealth);

            if (damageEffectCoroutine != null) StopCoroutine(damageEffectCoroutine);
            damageEffectCoroutine = StartCoroutine(HandleDamageEffects(damagePercent));

            return true;
        }

        return false;
    }

    private IEnumerator HandleDamageEffects(float intensityPrct)
    {
        float shakeStrength = maxShakeMagnitude * Mathf.Clamp(intensityPrct, 0.2f, 1f);
        if (cameraShake != null) StartCoroutine(cameraShake.Shake(0.3f, shakeStrength));

        // Obsługa Post-processingu (rozmycie/kolory)
        if (damageVolume != null)
        {
            float targetWeight = Mathf.Clamp01(intensityPrct * 2f); // Mnożnik x2, żeby nawet małe uderzenia były widoczne
            float timer = 0f;

            // Faza 1: Szybkie wejście efektu (uderzenie)
            while (timer < 0.1f)
            {
                timer += Time.deltaTime;
                damageVolume.weight = Mathf.Lerp(0f, targetWeight, timer / 0.1f);
                yield return null;
            }

            // Faza 2: Powolne wygaszanie przez resztę czasu
            timer = 0f;
            float duration = effectDuration - 0.1f;

            while (timer < duration)
            {
                timer += Time.deltaTime;
                // Płynne zejście do zera
                damageVolume.weight = Mathf.Lerp(targetWeight, 0f, timer / duration);
                yield return null;
            }

            damageVolume.weight = 0f;
        }
    }

    /**
     * Przeciąża metodę śmierci: animacja i ekran końcowy.
     */
    public override void Die()
    {
        playerMovement.enabled = false;
        anim.SetTrigger("Death");
        //anim.SetTrigger("DeathEnded");
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
            if (!playerMovement.isDashing && !playerAttack.IsAttacking && currentStamina < startingStamina)
            {
                HealStamina(staminaRegenRate);
            }
            yield return new WaitForSeconds(staminaRegenTimeRate);
        }
    }
    public override void Revive()
    {
        base.Revive();

        if (playerMovement != null)
            playerMovement.enabled = true;

        if (anim != null)
        {
            anim.Rebind();
            anim.Update(0f);
        }
    }

}

