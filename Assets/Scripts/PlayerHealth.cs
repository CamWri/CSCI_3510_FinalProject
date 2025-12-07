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

    /// <summary>
    /// Apply damage to the player
    /// </summary>
    /// <param name="amount">Amount of damage to take</param>
    public void TakeDamage(float amount)
    {
        if (invincible || currentHealth <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);

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

        Debug.Log($"{gameObject.name} healed {amount}. Current health: {currentHealth}");
    }

    /// <summary>
    /// Handles player death
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        onDeath?.Invoke();

        // Example: disable player movement & visuals (optional)
        // GetComponent<PlayerMovement>()?.enabled = false;
        // GetComponent<Renderer>().enabled = false;
    }

    /// <summary>
    /// Returns current health
    /// </summary>
    public float GetHealth()
    {
        return currentHealth;
    }

    /// <summary>
    /// Returns health as a percentage (0 to 1)
    /// </summary>
    public float GetHealthPercent()
    {
        return currentHealth / maxHealth;
    }
}