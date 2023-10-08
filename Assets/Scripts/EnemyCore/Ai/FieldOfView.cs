using System.Collections;
using System.Collections.Generic;
using EnemyCore;
using Player;
using UnityEngine;
public class FieldOfView : MonoBehaviour
{
    public float radius;
    [Range(0, 360)] public float viewAngle = 110;

    public GameObject playerRef;

    public LayerMask targetMask;
    public LayerMask obstructionMask;

    public bool canSeePlayer;
    private float stepCount;

    [SerializeField] private float meshResolution = 0.2f;
    

    public void Initialize(FovContainer fovContainer)
    {
        radius = fovContainer.radius;
        viewAngle = fovContainer.angle;
        targetMask = fovContainer.targetMask;
        obstructionMask = fovContainer.obstructionMask;
        StartCoroutine(FOVRoutine());
        playerRef = FindObjectOfType<PlayerStatsController>().gameObject;
    }
    private IEnumerator FOVRoutine()
    {
        WaitForSeconds wait = new WaitForSeconds(0.2f);

        while (true)
        {
            yield return wait;
            FieldOfViewCheck();
        }
    }

    private void FieldOfViewCheck()
    {
        Collider[] rangeChecks = Physics.OverlapSphere(transform.position, radius, targetMask);
        if (rangeChecks.Length != 0)
        {
            Transform target = rangeChecks[0].transform;
            Vector3 directionToTarget = (target.position - transform.position).normalized;

            float _angle = Vector3.Angle(transform.forward, directionToTarget);
            if (_angle < viewAngle/2)
            {
                float distanceToTarget = Vector3.Distance(transform.position, target.position);
                if (!Physics.Raycast(transform.position ,directionToTarget,out RaycastHit hit,distanceToTarget ,obstructionMask))
                {
                    Debug.Log("I see player");
                    canSeePlayer = true;
                }
                else
                {
                    Debug.Log("Nah i cant see player");
                    Debug.Log("I see " + hit.transform.name);
                    canSeePlayer = false;
                }
            }
            else
            {
                Debug.Log("Angle is bad");
                canSeePlayer = false;
            }
            
        }
        else if (canSeePlayer)
        {
            canSeePlayer = false;
        }
    }

    private void Update()
    {
        DrawFieldOfView();
    }

    void DrawFieldOfView()
    {
        int stepCount = Mathf.RoundToInt(viewAngle * meshResolution);
        float stepAngleSize = viewAngle / stepCount;
        List<Vector3> viewPoints = new List<Vector3>();
        for (int i = 0; i < stepCount; i++)
        {
            float angle = transform.eulerAngles.y - viewAngle / 2 + stepAngleSize * i;
            ViewCastInfo newViewCastInfo = ViewCast(angle);
            viewPoints.Add(newViewCastInfo.point);
        }
    }

    public Vector3 DirFromAngle(float angleInDegrees, bool angleIsGlobal)
    {
        if (!angleIsGlobal)
        {
            angleInDegrees += transform.eulerAngles.y;
        }
        return new Vector3(Mathf.Sin(angleInDegrees * Mathf.Deg2Rad), 0, Mathf.Cos(angleInDegrees * Mathf.Deg2Rad));
    }
    
    public struct ViewCastInfo
    {
        public bool hit;
        public Vector3 point;
        public float dst;
        public float angle;

        public ViewCastInfo(bool _hit, Vector3 _point, float _dst, float _angle)
        {
            hit = _hit;
            point = _point;
            dst = _dst;
            angle = _angle;
        }
    }

    ViewCastInfo ViewCast(float globalAngle)
    {
        Vector3 dir = DirFromAngle(globalAngle, true);
        RaycastHit raycastHit;
        if (Physics.Raycast(transform.position, dir, out raycastHit , radius , obstructionMask))
        {
            return new ViewCastInfo(true, raycastHit.point, raycastHit.distance, globalAngle);
        }
        return new ViewCastInfo(false, transform.position + dir * radius, radius, globalAngle);
    }
}
