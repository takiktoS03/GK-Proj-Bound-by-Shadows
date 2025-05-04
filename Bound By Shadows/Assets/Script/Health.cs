using Microlight.MicroBar;
using UnityEngine;

public class Health : MonoBehaviour
{
    [SerializeField] MicroBar healthBar;
    [SerializeField] MicroBar staminaBar;
    [SerializeField] private float startingHealth;
    public float currentHealth {  get; private set; }

    private void Awake()
    {
        currentHealth = startingHealth;
        healthBar.Initialize(startingHealth);
        staminaBar.Initialize(startingHealth);
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
        currentHealth = Mathf.Clamp(currentHealth - amount, 0, startingHealth);
        healthBar.UpdateBar(healthBar.CurrentValue - amount);
        staminaBar.UpdateBar(staminaBar.CurrentValue - amount);
    }

    public void Heal(float amount)
    {
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, startingHealth);
        healthBar.UpdateBar(healthBar.CurrentValue + amount);
        staminaBar.UpdateBar(staminaBar.CurrentValue + amount);
    }
}
