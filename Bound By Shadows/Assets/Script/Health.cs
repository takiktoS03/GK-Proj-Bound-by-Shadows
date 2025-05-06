using Microlight.MicroBar;
using UnityEngine;

/// <summary>
///  Przechowuje i aktualizuje punkty zdrowia.
///  Teraz wystawia w�a�ciwo�� <c>CurrentHealth</c> oraz metod� <c>SetHealth</c> �
///  potrzebne do systemu zapisu.
/// </summary>
public class Health : MonoBehaviour
{
    [SerializeField] private MicroBar healthBar;
    [SerializeField] private MicroBar staminaBar;
    [SerializeField] private float startingHealth = 100f;

    private float currentHealth;
    /// <summary>Bie��ca warto�� HP (tylko do odczytu).</summary>
    public float CurrentHealth => currentHealth;

    private void Awake()
    {
        currentHealth = startingHealth;
        healthBar.Initialize(startingHealth);
        staminaBar.Initialize(startingHealth);
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.E))
            TakeDamage(20);
        else if (Input.GetKeyDown(KeyCode.R))
            Heal(20);
    }

    public void TakeDamage(float amount) => SetHealth(currentHealth - amount);
    public void Heal(float amount) => SetHealth(currentHealth + amount);

    /// <summary>Ustawia HP i od�wie�a oba paski.</summary>
    public void SetHealth(float value)
    {
        currentHealth = Mathf.Clamp(value, 0, startingHealth);

        // MicroBar.UpdateBar oczekuje docelowej warto�ci, a nie delty
        healthBar.UpdateBar(currentHealth);
        staminaBar.UpdateBar(currentHealth);
    }
}
