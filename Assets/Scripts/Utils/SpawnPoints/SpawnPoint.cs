using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    public bool isEmpty;
    void Start()
    {
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (meshRenderer != null)
        {
            meshRenderer.enabled = false;
        }

        isEmpty = true;
    }

    public void BindSpawnPoint()
    {
        isEmpty = false;
    }

    public void ClearSpawnPoint()
    {
        isEmpty = true;
    }
}
