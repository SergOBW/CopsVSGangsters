using UnityEngine;
using UnityEngine.AI;

namespace DefaultNamespace
{
    public static class SergOBWUtils
    {
        public static Vector3 GetNearestNavMeshPosition(Vector3 positionToSpawn)
        {
            NavMesh.SamplePosition(positionToSpawn, out NavMeshHit navMeshHit, Mathf.Infinity, NavMesh.AllAreas);
            Vector3 myRandomPositionInsideNavMesh = navMeshHit.position;

            return myRandomPositionInsideNavMesh;
        }
    }
}