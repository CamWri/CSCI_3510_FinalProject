using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 100f;
    public bool invincible = false;

    [Header("Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;

    private float currentHealth;

    void Awake()
    {
        currentHealth = maxHealth;
    }

    void Start()
    {
        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }

    /// <summary>
    /// Apply damage to the player
    /// </summary>
    /// <param name="amount">Amount of damage to take</param>
    public void TakeDamage(float amount)
    {
        if (invincible || currentHealth <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentHealth = Mathf.Round(currentHealth * 10f) / 10f;

        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);

        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        // Trigger any events (like UI update or sound)
        onTakeDamage?.Invoke();

        if (currentHealth <= 0f)
        {
            Die();
        }
    }

    /// <summary>
    /// Heal the player
    /// </summary>
    /// <param name="amount">Amount of healing</param>
    public void Heal(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentHealth = Mathf.Round(currentHealth * 10f) / 10f;
        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);

        Debug.Log($"{gameObject.name} healed {amount}. Current health: {currentHealth}");
    }

    public void IncreaseMaxHP(float amount)
    {
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentHealth = Mathf.Round(currentHealth * 10f) / 10f;

        maxHealth += amount;
        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }

    /// <summary>
    /// Handles player death
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        onDeath?.Invoke();
    }

    public float GetHealth()
    {
        return currentHealth;
    }

    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}