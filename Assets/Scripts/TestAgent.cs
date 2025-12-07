using UnityEngine;
using UnityEngine.AI;

public class TestAgent : MonoBehaviour
{
    NavMeshAgent agent;
    public float radius = 20f;   // how far from the agent it can choose points

    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        MoveToRandomPoint();
    }

    void Update()
    {
        if (agent == null || !agent.enabled) return;  // ← prevents the error
        
        // When agent reaches the destination, pick a new one
        if (!agent.pathPending && agent.remainingDistance <= agent.stoppingDistance)
        {
            if (!agent.hasPath || agent.velocity.sqrMagnitude == 0f)
            {
                MoveToRandomPoint();
            }
        }
    }

    void MoveToRandomPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * radius;
        randomDirection += transform.position;

        NavMeshHit hit;
        if (NavMesh.SamplePosition(randomDirection, out hit, radius, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }
    }
}
