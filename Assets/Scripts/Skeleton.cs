using UnityEngine;
using UnityEngine.AI;

public class Skeleton : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator anim;
    public NavMeshAgent agent;

    private SkeletonRoundManager roundManager;

    [Header("Settings")]
    public float walkSpeed = 3.5f;
    public float speedRandomness = 0.5f; // +/- variation
    public float attackRange = 2f;
    public float spawnDuration = 2f;
    public float deathDuration = 1f;

    [Header("Stats")]
    public float health = 100f;

    bool isSpawning = true;
    bool isDead = false;

    void Start()
    {
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("No GameObject with tag 'Player' found!");
        }

        roundManager = FindFirstObjectByType<SkeletonRoundManager>();

        agent.speed = 0f;
        anim.SetBool("isSpawning", true);

        Invoke(nameof(FinishSpawn), spawnDuration);
    }

    void FinishSpawn()
    {
        isSpawning = false;
        anim.SetBool("isSpawning", false);

        // Randomize walk speed slightly
        float randomOffset = Random.Range(-speedRandomness, speedRandomness);
        agent.speed = walkSpeed + randomOffset;
    }

    void Update()
    {
        if (isDead || isSpawning || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            agent.SetDestination(player.position);
            anim.SetBool("isAttacking", false);
            anim.SetFloat("speed", agent.velocity.magnitude / agent.speed);
        }
        else
        {
            agent.SetDestination(transform.position);
            anim.SetBool("isAttacking", true);
            anim.SetFloat("speed", 0f);
        }
    }

    public void TakeDamage(float amount)
    {
        if (isDead) return;

        health -= amount;
        if (health <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        anim.SetBool("isDead", true);

        if (roundManager != null)
            roundManager.OnSkeletonKilled();

        Destroy(gameObject, deathDuration);
    }
}