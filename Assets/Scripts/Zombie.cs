using UnityEngine;

public class Zombie : MonoBehaviour
{
    public int health = 10;
    private ZombieRoundManager manager;

    void Start()
    {
        manager = FindFirstObjectByType<ZombieRoundManager>();
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if (health <= 0)
            Die();
    }

    void Die()
    {
        manager.OnZombieKilled();
        Destroy(gameObject);
    }
}
