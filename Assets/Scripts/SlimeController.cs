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
    public float deathDuration = 1f;

    [Header("Slime Splitting")]
    public int slimeTier = 1; // 1 = big, 2 = small
    public int maxTier = 2;
    public GameObject slimePrefab;

    Collider slimeCollider;

    void Start()
    {
        if (!Application.isPlaying) return;

        if (player == null)
        {
            GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
            if (playerObj != null)
                player = playerObj.transform;
        }

        roundManager = FindFirstObjectByType<SkeletonRoundManager>();

        slimeCollider = GetComponent<Collider>();

        if (agent != null)
        {
            if (!agent.isOnNavMesh)
            {
                NavMeshHit hit;
                if (NavMesh.SamplePosition(transform.position, out hit, 5f, NavMesh.AllAreas))
                {
                    transform.position = hit.position;
                }
            }

            if (agent.isOnNavMesh)
            {
                agent.updatePosition = true;
                agent.updateRotation = true;
                agent.isStopped = true;
            }
        }
    }

    void Update()
    {
        if (!Application.isPlaying || isDead || player == null) return;

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
        direction.y = 0;

        if (direction.sqrMagnitude > 0.01f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(direction);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 8f);
        }
    }

    IEnumerator HopTowardPlayer()
    {
        if (agent == null || player == null || anim == null) yield break;
        if (!agent.enabled) yield break;

        isHopping = true;


        if (agent != null && agent.isOnNavMesh)
        {
            agent.isStopped = true;
        }

        agent.updatePosition = false;
        agent.updateRotation = false;
        agent.velocity = Vector3.zero;

        Vector3 startPos = transform.position;
        Vector3 direction = (player.position - transform.position).normalized;
        Vector3 targetPos = startPos + direction * hopDistance;

        // Clamp to NavMesh
        if (NavMesh.SamplePosition(targetPos, out NavMeshHit hit, hopDistance, NavMesh.AllAreas))
            targetPos = hit.position;

        anim.SetTrigger("hop");

        float duration = 0.6f;
        float elapsed = 0f;

        while (elapsed < duration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / duration;

            Vector3 flatPos = Vector3.Lerp(startPos, targetPos, t);
            float height = Mathf.Sin(t * Mathf.PI) * hopHeight;

            transform.position = flatPos + Vector3.up * height;
            yield return null;
        }

        anim.SetTrigger("land");
        DealLandingDamage();

        // Snap to true ground using collider bounds
        Vector3 groundedPos = GetGroundedPosition(transform.position);

        // Only warp if the position is on a valid NavMesh
        NavMeshHit navHit;
        if (NavMesh.SamplePosition(groundedPos, out navHit, 1f, NavMesh.AllAreas))
        {
            transform.position = navHit.position;
            if (agent.isOnNavMesh)
            {
                agent.Warp(navHit.position);
                agent.updatePosition = true;
                agent.updateRotation = true;
                agent.isStopped = true;
            }
        }
        else
        {
            // Just snap the transform, agent stays disabled
            transform.position = groundedPos;
        }


        isHopping = false;
    }

    Vector3 GetGroundedPosition(Vector3 position)
    {
        Vector3 origin = position + Vector3.up * 0.1f;
        if (Physics.Raycast(origin, Vector3.down, out RaycastHit hit, 10f))
            return hit.point; // no offset
        return position;
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

        if (slimeCollider != null) slimeCollider.enabled = false;

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

        Vector3 center = transform.position + Vector3.up * 0.1f; // near bottom
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
        Gizmos.color = Color.green;
        float bottomOffset = 0.1f;
        if (slimeCollider != null)
            bottomOffset = slimeCollider.bounds.extents.y;
        Vector3 center = transform.position - Vector3.up * bottomOffset + Vector3.up * 0.1f;
        Gizmos.DrawWireSphere(center, landingDamageRadius);
    }
}