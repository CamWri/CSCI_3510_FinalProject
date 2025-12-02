using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Linq;

public class Skeleton : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator anim;
    public NavMeshAgent agent;

    private SkeletonRoundManager roundManager;

    [Header("Settings")]
    public float walkSpeed = 3.5f;
    public float speedRandomness = 0.5f;
    public float attackRange = 2f;
    public float spawnDuration = 2f;
    public float deathDuration = 1f;

    [Header("Stats")]
    public float health = 100f;

    bool isSpawning = true;
    bool isDead = false;
    bool isAttacking = false;

    float attackAnimationLength;

    void Start()
    {
        // Ensure player reference
        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
            else
                Debug.LogError("No GameObject with tag 'Player' found!");
        }

        // Cache round manager
        roundManager = FindFirstObjectByType<SkeletonRoundManager>();

        // Cache attack animation length dynamically
        attackAnimationLength = anim.runtimeAnimatorController.animationClips
            .First(c => c.name == "root|slash01") // <-- Make sure this is the exact clip name!
            .length;

        // Spawn animation setup
        agent.speed = 0f;
        anim.SetBool("isSpawning", true);

        Invoke(nameof(FinishSpawn), spawnDuration);
    }

    void FinishSpawn()
    {
        isSpawning = false;
        anim.SetBool("isSpawning", false);

        float randomOffset = Random.Range(-speedRandomness, speedRandomness);
        agent.speed = walkSpeed + randomOffset;
    }

    void Update()
    {
        if (isDead || isSpawning || player == null) return;

        float distance = Vector3.Distance(transform.position, player.position);

        if (distance > attackRange)
        {
            MoveTowardPlayer();
        }
        else
        {
            TryAttack();
        }
    }

    void MoveTowardPlayer()
    {
        agent.isStopped = false;
        agent.SetDestination(player.position);
        anim.SetFloat("speed", agent.speed);
    }

    void TryAttack()
    {
        // Stop agent movement COMPLETELY
        agent.isStopped = true;
        agent.updatePosition = false;
        agent.updateRotation = false;

        anim.SetFloat("speed", 0f);

        if (!isAttacking)
        {
            anim.SetTrigger("attackTrigger");
            StartCoroutine(AttackRoutine());
        }
    }

    IEnumerator AttackRoutine()
    {
        isAttacking = true;

        yield return new WaitForSeconds(attackAnimationLength);

        // Sync agent's internal position to the model
        agent.nextPosition = transform.position;

        // Re-enable movement
        agent.updatePosition = true;
        agent.updateRotation = true;
        agent.isStopped = false;

        isAttacking = false;
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