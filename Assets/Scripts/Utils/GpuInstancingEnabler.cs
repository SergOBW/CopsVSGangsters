using UnityEngine;

public class GpuInstancingEnabler : MonoBehaviour
{
    private void Awake()
    {
        MaterialPropertyBlock materialPropertyBlock = new MaterialPropertyBlock();
        SkinnedMeshRenderer skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        MeshRenderer meshRenderer = GetComponent<MeshRenderer>();
        if (skinnedMeshRenderer != null)
        {
            skinnedMeshRenderer.SetPropertyBlock(materialPropertyBlock);
        }

        if (meshRenderer != null)
        {
            meshRenderer.SetPropertyBlock(materialPropertyBlock);
        }
    }
}
