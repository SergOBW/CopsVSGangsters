using UnityEngine;

namespace ForWeapon
{
    [CreateAssetMenu()]
    public class WeaponGraphicsSo : ScriptableObject
    {
        public ParticleSystem muzzleFlash;
        public ParticleSystem hitImpact;
        public GameObject weaponPrefab;
        public GameObject projectilePrefab;
        public AnimatorOverrideController animatorOverrideController;
        
        public Vector3 Scale = Vector3.one;
        public Vector3 Position = Vector3.zero;
        public Vector3 Rotation = Vector3.zero;
    }
}