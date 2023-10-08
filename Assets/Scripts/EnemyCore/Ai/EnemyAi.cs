using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
namespace EnemyCore
{ 
    public class FovContainer
    {
        public float radius;
        public float angle;
        public LayerMask targetMask;
        public LayerMask obstructionMask;
        public float height;

        public FovContainer(float radius, float angle, LayerMask targetMask, LayerMask obstructionMask,float height = 3)
        {
            this.radius = radius;
            this.angle = angle;
            this.targetMask = targetMask;
            this.obstructionMask = obstructionMask;
            this.height = height;
        }
    }
    
    internal class EnemyAi : MonoBehaviour
    {
        private NavMeshAgent navMeshAgent;
        private Vector3 lastPosition;
        private AiSensor aiSensor;

        private float defaultSpeed;
        private float maxSpeed;
        private AiStats currentAiStats;

        private EnemySoundManager enemySoundManager;
        private DebugNavMsehAgent debugNavMsehAgent;

        private Transform endPoint;

        public void Initialize(AiStats aiStats,float _defaultSpeed, bool isEnemy, float _maxSpeed)
        {
            currentAiStats = aiStats;
            CreateScripts();
            
            aiSensor.Initialize(new FovContainer(currentAiStats.radius,currentAiStats.angle,
                currentAiStats.targetMask,currentAiStats.obstructionMask));
            
            defaultSpeed = _defaultSpeed;
            maxSpeed = _maxSpeed;
            NavMeshAgentInitialize();
            
            debugNavMsehAgent.Initialize(navMeshAgent, currentAiStats.radius);

            if (isEnemy)
            {
                GameObject endPointGameObject = GameObject.FindWithTag("EndPoint");
                if (endPointGameObject != null)
                {
                    endPoint = endPointGameObject.transform;
                }
            }

        }
        
        private void CreateScripts()
        {
            debugNavMsehAgent = gameObject.AddComponent<DebugNavMsehAgent>();
            for (int i = 0; i < transform.childCount; i++)
            {
                Transform child = transform.GetChild(i);
                if (child.tag == "EnemyDetector")
                {
                    aiSensor = child.AddComponent<AiSensor>();
                    break;
                }
            }
        }
        
        public void NavMeshAgentInitialize()
        {
            var agentId = GetNavMeshAgentID("Enemy");
            NavMeshHit closestHit;
            if (NavMesh.SamplePosition (transform.position, out closestHit, 100f, NavMesh.AllAreas)) {
                transform.position = closestHit.position;
                navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
                navMeshAgent.agentTypeID = agentId.Value;
            }

            if (navMeshAgent == null)
            {
                Debug.LogError("There is no nav Mesh Agent");
                return;
            }
            
            navMeshAgent.speed = defaultSpeed;
            navMeshAgent.stoppingDistance = 0.5f;
            navMeshAgent.angularSpeed = 600;
            navMeshAgent.acceleration = 80;
        }
        
        private int? GetNavMeshAgentID(string name)
        {
            for (int i = 0; i < NavMesh.GetSettingsCount(); i++)
            {
                NavMeshBuildSettings settings = NavMesh.GetSettingsByIndex(index: i);
                if (name == NavMesh.GetSettingsNameFromID(agentTypeID: settings.agentTypeID))
                {
                    return settings.agentTypeID;
                }
            }
            return null;
        }
        public void ClearSpeed()
        {
            navMeshAgent.speed = defaultSpeed;
        }
        
        public Transform GetEndPoint()
        {
            return endPoint;
        }

        public bool IsSeeTarget(GameObject gameObject)
        {
            return aiSensor.IsInSight(gameObject);
        }

        public AiSensor GetAiSensor()
        {
            return aiSensor;
        }

        public float GetDefaultSpeed()
        {
            return defaultSpeed;
        }

        public float GetMaxSpeed()
        {
            return maxSpeed;
        }
    }
}
