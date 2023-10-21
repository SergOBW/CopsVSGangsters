using UnityEngine;

namespace ForWeapon
{
    public class WeaponGraphics
    {
        public ParticleSystem muzzleFlash;
        public ParticleSystem hitImpact;
        
        public GameObject projectilePrefab;
        public GameObject weaponPrefab;
        
        public Vector3 Scale = Vector3.one;
        public Vector3 Position = Vector3.zero;
        public Vector3 Rotation = Vector3.zero;

        public WeaponGraphics(WeaponGraphicsSo weaponGraphicsSo)
        {
            muzzleFlash = weaponGraphicsSo.muzzleFlash;
            hitImpact = weaponGraphicsSo.hitImpact;

            weaponPrefab = weaponGraphicsSo.weaponPrefab;
            projectilePrefab = weaponGraphicsSo.projectilePrefab;

            Scale = weaponGraphicsSo.Scale;
            Position = weaponGraphicsSo.Position;
            Rotation = weaponGraphicsSo.Rotation;
        }
        
    }
}
