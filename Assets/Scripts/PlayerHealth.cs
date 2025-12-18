using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 100f;
    public bool invincible = false;

    [Header("Regeneration Settings")]
    public float regenDelay = 3f;       // Seconds to wait after last damage
    public float regenRate = 10f;       // Health per second

    [Header("Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;

    private float currentHealth;
    private float lastDamageTime;

    void Awake()
    {
        currentHealth = maxHealth;
        lastDamageTime = -regenDelay; // ensures regen can start immediately if needed
    }

    void Start()
    {
        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }

    void Update()
    {
        HandleRegeneration();
    }

    /// <summary>
    /// Apply damage to the player
    /// </summary>
    public void TakeDamage(float amount)
    {
        if (invincible || currentHealth <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentHealth = Mathf.Round(currentHealth * 10f) / 10f;

        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);
        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        // Reset regeneration timer
        lastDamageTime = Time.time;

        onTakeDamage?.Invoke();

        if (currentHealth <= 0f)
            Die();
    }

    /// <summary>
    /// Heal the player by a fixed amount
    /// </summary>
    public void Heal(float amount)
    {
        if (currentHealth <= 0f) return;

        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentHealth = Mathf.Round(currentHealth * 10f) / 10f;

        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);
        Debug.Log($"{gameObject.name} healed {amount}. Current health: {currentHealth}");
    }

    /// <summary>
    /// Increase max health
    /// </summary>
    public void IncreaseMaxHP(float amount)
    {
        maxHealth += amount;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentHealth = Mathf.Round(currentHealth * 10f) / 10f;

        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }

    /// <summary>
    /// Handles smooth regeneration over time after regenDelay
    /// </summary>
    private void HandleRegeneration()
    {
        if (currentHealth <= 0f || currentHealth >= maxHealth) return;

        if (Time.time - lastDamageTime >= regenDelay)
        {
            float healThisFrame = regenRate * Time.deltaTime;
            currentHealth += healThisFrame;
            currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
            currentHealth = Mathf.Round(currentHealth * 10f) / 10f;

            HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);
        }
    }

    /// <summary>
    /// Handles player death
    /// </summary>
    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        onDeath?.Invoke();
    }

    /// <summary>
    /// Returns current health value
    /// </summary>
    public float GetHealth() => currentHealth;

    /// <summary>
    /// Returns current health as percentage (0-1)
    /// </summary>
    public float GetHealthPercent() => currentHealth / maxHealth;
}