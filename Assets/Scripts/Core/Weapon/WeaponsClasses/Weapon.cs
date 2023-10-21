using DefaultNamespace;
using UnityEngine;

namespace ForWeapon
{
    public class Weapon
    {
        // Stats
        public float Damage { get; private set; }
        public float Range { get; private set;}
        public float FireRate { get; private set; }

        private AnimatorOverrideController _animatorOverrideController;
        public float DistanceOffset { get; private set; }

        public Weapon(WeaponStatsSo weaponStatsSo)
        {
            Damage = weaponStatsSo.damage;
            Range = weaponStatsSo.range;
            FireRate = weaponStatsSo.fireRate;
            _animatorOverrideController = weaponStatsSo.animatorOverrideController;
            DistanceOffset = weaponStatsSo.DistanceOffset;
        }
        public virtual void Initialize(WeaponManager weaponManager)
        {
            weaponManager.SetWeaponAnimatorController(_animatorOverrideController);
        }
        
        public virtual void Attack()
        {
            
        }

        public virtual void Reload()
        {

        }
    }
}