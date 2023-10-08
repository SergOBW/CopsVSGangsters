using UnityEngine;

[CreateAssetMenu(menuName = "Visuals")]
public class VisualsSo : ScriptableObject
{
    public Transform VisualGameObject;
    public Vector3 Scale = Vector3.one;
    public Vector3 Position = Vector3.zero;
    public Vector3 Rotation = Vector3.zero;

    public AnimatorOverrideController animatorOverrideController;
}
