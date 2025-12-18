using UnityEngine;
using UnityEngine.Events;

public class PlayerHealth : MonoBehaviour
{
    [Header("Settings")]
    public float maxHealth = 100f;
    public bool invincible = false;

    [Header("Regeneration Settings")]
    public float regenDelay = 3f;
    public float regenRate = 10f; 
    [Header("Events")]
    public UnityEvent onTakeDamage;
    public UnityEvent onDeath;

    private float currentHealth;
    private float lastDamageTime;

    void Awake()
    {
        currentHealth = maxHealth;
        lastDamageTime = -regenDelay;
    }

    void Start()
    {
        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }

    void Update()
    {
        HandleRegeneration();
    }

    public void TakeDamage(float amount)
    {
        if (invincible || currentHealth <= 0f) return;

        currentHealth -= amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentHealth = Mathf.Round(currentHealth * 10f) / 10f;

        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);
        Debug.Log($"{gameObject.name} took {amount} damage. Current health: {currentHealth}");

        lastDamageTime = Time.time;

        onTakeDamage?.Invoke();

        if (currentHealth <= 0f)
            Die();
    }

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
        maxHealth += amount;
        currentHealth += amount;
        currentHealth = Mathf.Clamp(currentHealth, 0f, maxHealth);
        currentHealth = Mathf.Round(currentHealth * 10f) / 10f;

        HUDController.Instance.UpdateHealthBar(currentHealth, maxHealth);
    }

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

    private void Die()
    {
        Debug.Log($"{gameObject.name} has died.");
        onDeath?.Invoke();
    }

    public float GetHealth() => currentHealth;
    public float GetHealthPercent() => currentHealth / maxHealth;
}