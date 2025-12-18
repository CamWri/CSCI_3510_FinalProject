using UnityEngine;
using UnityEngine.AI;
using System.Collections;

public class SlimeController : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Animator anim;
    public NavMeshAgent agent;
    private SkeletonRoundManager roundManager;

    [Header("Settings")]
    public float hopHeight = 2f;
    public float hopInterval = 1.2f;
    public float hopDistance = 3f;
    public float health = 40f;
    public float damage = 1f;
    public float landingDamageRadius = 1.5f;
    public LayerMask playerMask;

    float hopTimer;
    bool isHopping;
    bool isDead = false;
    bool isSpawning = false;
    public float deathDuration = 1f;

    [Header("Slime Splitting")]
    public int slimeTier = 1; // 1 = big, 2 = small
    public int maxTier = 2;
    public GameObject slimePrefab;

    void Start()
    {
        // Only run runtime code in Play Mode
        if (!Application.isPlaying) return;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        roundManager = FindFirstObjectByType<SkeletonRoundManager>();

        if (agent != null)
        {
            agent.isStopped = true;
            agent.updatePosition = true;
            agent.updateRotation = true;
            agent.updateUpAxis = false;
        }
    }

    void Update()
    {
        if (!Application.isPlaying) return;
        if (isDead || isSpawning || player == null) return;

        hopTimer += Time.deltaTime;
        FacePlayer();

        if (hopTimer >= hopInterval && !isHopping)
        {
            StartCoroutine(HopTowardPlayer());
            hopTimer = 0f;
        }
    }

    void FacePlayer()
    {
        if (player == null) return;

        Vector3 direction = (player.position - transform.position).normalized;
        direction.y = 0; // horizontal rotation only

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
    }

    IEnumerator HopTowardPlayer()
    {
        if (agent == null || player == null || anim == null) yield break;

        isHopping = true;

        agent.isStopped = false;
        agent.updatePosition = true;
        agent.updateRotation = true;

        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 target = transform.position + direction * hopDistance;

        if (NavMesh.SamplePosition(target, out NavMeshHit hit, hopDistance, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        anim.SetTrigger("hop");

        float startY = transform.position.y;
        float t = 0f;

        while (t < 1f)
        {
            t += Time.deltaTime * agent.speed;

            float height = Mathf.Sin(t * Mathf.PI) * hopHeight;
            transform.position = new Vector3(transform.position.x, startY + height, transform.position.z);

            yield return null;
        }

        agent.isStopped = true;
        agent.velocity = Vector3.zero;
        agent.nextPosition = transform.position;

        anim.SetTrigger("land");

        yield return new WaitForSeconds(0.05f);
        DealLandingDamage();

        isHopping = false;
    }

    public void TakeDamage(float amount)
    {
        if (!Application.isPlaying || isDead) return;

        health -= amount;
        if (health <= 0)
        {
            if (PlayerMoneyManager.Instance != null)
                PlayerMoneyManager.Instance.AddMoney(100);

            Die();
        }
    }

    void Die()
    {
        if (isDead) return;
        isDead = true;

        if (agent != null)
        {
            agent.isStopped = true;
            agent.enabled = false;
        }

        if (anim != null)
            anim.SetBool("isDead", true);

        Collider col = GetComponent<Collider>();
        if (col != null) col.enabled = false;

        if (slimeTier < maxTier)
            Split();

        if (roundManager != null)
            roundManager.OnSkeletonKilled();

        Destroy(gameObject, deathDuration);
    }

    void Split()
    {
        if (slimePrefab == null) return;

        int count = Random.Range(1, 3);
        for (int i = 0; i < count; i++)
        {
            Vector3 offset = Random.insideUnitSphere * 1.2f;
            offset.y = 0;

            GameObject child = Instantiate(slimePrefab, transform.position + offset, Quaternion.identity);
            SlimeController slime = child.GetComponent<SlimeController>();
            if (slime == null) continue;

            slime.slimeTier = slimeTier + 1;
            slime.health = health * 0.5f;
            slime.hopDistance = hopDistance * 1.2f;
            slime.hopInterval = hopInterval * 0.8f;
            child.transform.localScale = transform.localScale * 0.6f;
        }
    }

    void DealLandingDamage()
    {
        if (!Application.isPlaying) return;

        Vector3 center = transform.position + Vector3.up * 0.5f;
        Collider[] hits = Physics.OverlapSphere(center, landingDamageRadius, playerMask);

        foreach (Collider hit in hits)
        {
            PlayerHealth playerHealth = hit.GetComponentInParent<PlayerHealth>();
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damage);
                break;
            }
        }
    }

    void OnDrawGizmosSelected()
    {
        // Only draw gizmos in editor
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position + Vector3.up * 0.5f, landingDamageRadius);
    }
}
