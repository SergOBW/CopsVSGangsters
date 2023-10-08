using System.Collections.Generic;
using DefaultNamespace;
using ForWeapon;
using UnityEngine;

namespace EnemyCore
{
    public class EnemyWeaponManager : WeaponManager
    {
        private Weapon currentBaseWeapon;
        
        private List<Weapon> weaponStatsList;

        private Transform weaponSlot;
        private Transform meleeSlot;

        public override void Initialize()
        {
            weaponSlot = GetComponentInChildren<EnemyVisualsController>().GetWeaponSlot();
            meleeSlot = GetComponentInChildren<EnemyVisualsController>().GetMeleeSlot();
            weaponStatsList = new List<Weapon>();
        }

        public override void EquipWeapon(WeaponStatsSo weaponStats)
        {
            Weapon baseWeapon = weaponStats.Initialize();

            if (currentBaseWeapon != null)
            {
                UnEquipWeapon();
            }
            
            currentBaseWeapon = baseWeapon;
            currentBaseWeapon.Initialize(this);
            weaponStatsList.Add(currentBaseWeapon);
        }
        public override void Shoot()
        {
            currentBaseWeapon.Attack();
        }
        
        public override void UnEquipWeapon()
        {
            currentBaseWeapon = null;
        }
        
        public float GetAttackSpeed()
        {
            return currentBaseWeapon.FireRate;
        }

        public float GetShootingRange()
        {
            return currentBaseWeapon.Range;
        }

        public float GetDamage()
        {
            return currentBaseWeapon.Damage;
        }

        public override Transform GetWeaponSlot()
        {
            return weaponSlot;
        }

        public override Transform GetMeleeSlot()
        {
            return meleeSlot;
        }

        public override void SetWeaponAnimatorController(AnimatorOverrideController animatorOverrideController)
        {
            GetComponent<EnemyAnimationManager>().SetNewAnimatorController(animatorOverrideController);
        }

        public float GetDistanceOffest()
        {
            return currentBaseWeapon.DistanceOffset;
        }
    }
}