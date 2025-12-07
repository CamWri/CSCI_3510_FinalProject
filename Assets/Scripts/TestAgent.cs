using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class TestAgent : MonoBehaviour
{
    private NavMeshAgent agent;
    public Transform target;           // Target to follow (e.g., player)
    public float radius = 20f;         // How far to search for random points if needed
    public int maxAttempts = 10;       // Max attempts to find a valid NavMesh point

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();

        // Assign target automatically based on Player tag
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            target = playerObj.transform;
        }
        else
        {
            Debug.LogError("No GameObject with tag 'Player' found!");
            enabled = false;
            return;
        }

        // Ensure the agent starts on a NavMesh
        if (!agent.isOnNavMesh)
            SnapToNavMesh();
    }

    private void Update()
    {
        if (agent == null || !agent.enabled || target == null) return;

        // Always try to follow the target
        MoveToTarget(target.position);
    }

    /// <summary>
    /// Snaps the agent to the nearest point on the NavMesh.
    /// </summary>
    private void SnapToNavMesh()
    {
        NavMeshHit hit;
        if (NavMesh.SamplePosition(transform.position, out hit, 10f, NavMesh.AllAreas))
        {
            transform.position = hit.position;
        }
        else
        {
            Debug.LogError($"Agent '{name}' could not find a valid NavMesh nearby!");
            enabled = false;
        }
    }

    /// <summary>
    /// Moves the agent to the target. If unreachable, moves to the nearest reachable NavMesh point.
    /// </summary>
    /// <param name="targetPosition"></param>
    private void MoveToTarget(Vector3 targetPosition)
    {
        NavMeshPath path = new NavMeshPath();
        agent.CalculatePath(targetPosition, path);

        if (path.status == NavMeshPathStatus.PathComplete)
        {
            agent.SetDestination(targetPosition);
        }
        else
        {
            // Target is unreachable → snap to nearest point on NavMesh
            NavMeshHit hit;
            if (NavMesh.SamplePosition(targetPosition, out hit, 5f, NavMesh.AllAreas))
            {
                agent.SetDestination(hit.position);
            }
            else
            {
                // Optional fallback: pick a random nearby point
                MoveToRandomPoint();
            }
        }
    }

    /// <summary>
    /// Picks a random reachable point on the NavMesh and sets it as the agent's destination.
    /// </summary>
    private void MoveToRandomPoint()
    {
        for (int i = 0; i < maxAttempts; i++)
        {
            Vector3 randomDirection = Random.insideUnitSphere * radius + transform.position;
            NavMeshHit hit;

            if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
            {
                NavMeshPath path = new NavMeshPath();
                agent.CalculatePath(hit.position, path);

                if (path.status == NavMeshPathStatus.PathComplete)
                {
                    agent.SetDestination(hit.position);
                    return;
                }
            }
        }

        // Fallback
        Debug.LogWarning($"Agent '{name}' could not find a valid destination after {maxAttempts} attempts.");
    }
}