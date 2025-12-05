using UnityEngine;

public class TestZombie : Target
{
    public float health = 100f;
    int hitCount = 0;
    float currentHealth;

    private void Start()
    {
        currentHealth = health;
    }

    public override void Process(RaycastHit hit, float dmg)
    {
        if (gameObject.tag == "TestZombie")
        {
            hitCount += 1;
            currentHealth = health - hitCount*dmg;
            if (currentHealth <= 0f)
            {
                Debug.Log("Dies");
                Destroy(gameObject);
            }
        }
    }
}
