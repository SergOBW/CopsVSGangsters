using UnityEngine;
using UnityEngine.AI;

public class DebugNavMsehAgent : MonoBehaviour
{
    [SerializeField]private bool velocity;
    [SerializeField]private bool desiredVelocity;
    [SerializeField]private bool path;
    [SerializeField]private bool shootRange;

    private NavMeshAgent navMeshAgent;
    private float range;

    public void Initialize(NavMeshAgent agent, float range)
    {
        navMeshAgent = agent;
        this.range = range;
    }

    private void OnDrawGizmos()
    {
        if (navMeshAgent == null)
        {
            return;
        }
        if (velocity)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position,transform.position + navMeshAgent.velocity);
        }
        
        if (desiredVelocity)
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position,transform.position + navMeshAgent.desiredVelocity);
        }

        if (path)
        {
            Gizmos.color = Color.black;
            var agentPath = navMeshAgent.path;
            Vector3 prevCorner = transform.position;
            foreach (var corner in agentPath.corners)
            {
                Gizmos.DrawLine(prevCorner,corner);
                Gizmos.DrawSphere(corner,0.1f);
                prevCorner = corner;
            }
        }

        if (shootRange)
        {
            Gizmos.color = Color.cyan;
            var pos = transform.position + new Vector3(0, 1, 0);
            Gizmos.DrawWireSphere(pos,range);
        }
    }
}
