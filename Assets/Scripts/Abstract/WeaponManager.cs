using UnityEngine;

namespace DefaultNamespace
{
    public abstract class WeaponManager : MonoBehaviour
    {
        public virtual void Initialize(){}
        public virtual void EquipWeapon(WeaponStatsSo weaponStats){}
        public virtual void Shoot(){}
        public virtual void UnEquipWeapon(){}

        public virtual Transform GetWeaponSlot()
        {
            return new RectTransform();
        }
        
        public virtual Transform GetMeleeSlot()
        {
            return new RectTransform();
        }

        public virtual void SetWeaponAnimatorController(AnimatorOverrideController animatorOverrideController){}
    }
}