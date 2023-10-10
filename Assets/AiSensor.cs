using System.Collections.Generic;
using DefaultNamespace;
using EnemyCore;
using UnityEngine;

[ExecuteInEditMode]
public class AiSensor : MonoBehaviour
{
    public float distance = 10;

    public float angle = 30;

    public float height = 1;

    public Color meshColor = Color.red;
    public int scanFrequency = 30;
    public LayerMask occlusionLayers;
    public LayerMask targetLayers;

    public List<GameObject> objects = new List<GameObject>();
    public List<StatsController> statsControllers = new List<StatsController>();

    private Collider[] _colliders = new Collider[50];
    private Mesh _mesh;
    private int count;
    private float scanInterval;
    private float scanTimer;
    
    // Start is called before the first frame update
    void Start()
    {
        scanInterval = 1.0f / scanFrequency;
    }

    // Update is called once per frame
    void Update()
    {
        scanTimer -= Time.deltaTime;
        if (scanTimer < 0)
        {
            scanTimer += scanInterval;
            Scan();
        }
    }
    
    private void OnValidate()
    {
        _mesh = CreateWedgeMesh();
        scanInterval = 1.0f / scanFrequency;
    }

    private void OnDrawGizmos()
    {
        if (_mesh)
        {
            Gizmos.color = meshColor;
            Gizmos.DrawMesh(_mesh,transform.position, transform.rotation);
        }
        
        Gizmos.DrawWireSphere(transform.position , distance);
        for (int i = 0; i < count; i++)
        {
            Gizmos.DrawSphere(_colliders[i].transform.position, 0.2f);
        }

        Gizmos.color = Color.green;
        foreach (var obj in objects)
        {
            Gizmos.DrawSphere(obj.transform.position, 0.2f);
        }
    }

    private void Scan()
    {
        count = Physics.OverlapSphereNonAlloc(transform.position, distance, _colliders, targetLayers,
            QueryTriggerInteraction.Collide);
        
        objects.Clear();
        if (statsControllers.Count > 0)
        {
            foreach (var statsController in statsControllers)
            {
                statsController.OnStatsDead -= StatsDead;
            }
        }
        statsControllers.Clear();
        for (int i = 0; i < count; i++)
        {
            GameObject obj = _colliders[i].gameObject;
            if (obj.TryGetComponent(out StatsController controller))
            {
                statsControllers.Add(controller);
                controller.OnStatsDead += StatsDead;
            }
            
            objects.Add(obj);
            
        }
    }

    private void StatsDead(StatsController statsController)
    {
        if (statsControllers.Contains(statsController))
        {
            statsController.OnStatsDead -= StatsDead;
            statsControllers.Remove(statsController);
        }
    }

    public bool IsInSight(GameObject obj)
    {
        Vector3 origin = transform.position;
        Vector3 dest = obj.transform.position;
        Vector3 direction = dest - origin;

        if (obj.TryGetComponent(out PlayerCharacter character))
        {
            if (character.IsSeeEnemy(gameObject))
            {
                return true;
            }
        }
        
        /*
        
        if (direction.y < 0 || direction.y > height)
        {
            return false;
        }
        
        */
        
        direction.y = 0;
        float deltaAngle = Vector3.Angle(direction, transform.forward);
        if (deltaAngle > angle)
        {
            return  false;
        }

        origin.y += height / 2;
        dest.y = origin.y;
        
        if (Physics.Linecast(origin,dest, out RaycastHit hit,occlusionLayers))
        {
            return false;
        }
        
        
        return true;
    }

    Mesh CreateWedgeMesh()
    {
        Mesh mesh = new Mesh();
        int segments = 10;
        int nimTriangles = (segments * 4) + 2 + 2;
        int numVertices = nimTriangles * 3;
        Vector3[] vertices = new Vector3[numVertices];
        int[] triangles = new int[numVertices];

        Vector3 bottomCenter = Vector3.zero;
        Vector3 bottomLeft = Quaternion.Euler(0, -angle, 0) * Vector3.forward * distance;
        Vector3 bottomRight = Quaternion.Euler(0, angle, 0) * Vector3.forward * distance;

        Vector3 topCenter = bottomCenter + Vector3.up * height;
        Vector3 topLeft = bottomLeft + Vector3.up * height;
        Vector3 topRight = bottomRight + Vector3.up * height;

        int vert = 0;
        // left side
        vertices[vert++] = bottomCenter;
        vertices[vert++] = bottomLeft;
        vertices[vert++] = bottomRight;
        
        vertices[vert++] = topLeft;
        vertices[vert++] = topCenter;
        vertices[vert++] = bottomCenter;
        // right side
        
        vertices[vert++] = bottomCenter;
        vertices[vert++] = topCenter;
        vertices[vert++] = topRight;
        
        vertices[vert++] = topRight;
        vertices[vert++] = bottomRight;
        vertices[vert++] = bottomCenter;

        float currentAngle = -angle;
        float deltaAngle = (angle * 2) / segments;

        for (int i = 0; i < segments; i++)
        {
            bottomLeft = Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * distance;
            bottomRight = Quaternion.Euler(0, currentAngle + deltaAngle, 0) * Vector3.forward * distance;
            
            topLeft = bottomLeft + Vector3.up * height;
            topRight = bottomRight + Vector3.up * height;
            currentAngle += deltaAngle;
            
            // far side
        
            vertices[vert++] = bottomLeft;
            vertices[vert++] = bottomRight;
            vertices[vert++] = topRight;
        
            vertices[vert++] = topRight;
            vertices[vert++] = topLeft;
            vertices[vert++] = bottomLeft;
        
            // top
        
            vertices[vert++] = topCenter;
            vertices[vert++] = topLeft;
            vertices[vert++] = topRight;
        
        
            // bot
        
            vertices[vert++] = bottomCenter;
            vertices[vert++] = bottomRight;
            vertices[vert++] = bottomLeft;
        }
        

        for (int i = 0; i < numVertices; i++)
        {
            triangles[i] = i;
        }

        mesh.vertices = vertices;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();
        return mesh;

    }

    public void Initialize(FovContainer fovContainer)
    {
        distance = fovContainer.radius;
        occlusionLayers = fovContainer.obstructionMask;
        targetLayers = fovContainer.targetMask;
        angle = fovContainer.angle;
        height = fovContainer.height;
    }
}
