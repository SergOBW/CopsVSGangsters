using DefaultNamespace;
using UnityEngine;

namespace ForWeapon
{
    public class MeleeWeapon : Weapon
    {
        private MeleeWeaponStatsSo _weaponStatsSo;
        // Visuals
        public WeaponGraphics weaponGraphics;
        private WeaponGraphicsMono currentWeaponGraphics;
        public MeleeWeapon(WeaponStatsSo weaponStats) : base(weaponStats)
        {
            _weaponStatsSo = weaponStats as MeleeWeaponStatsSo;
            
            if (_weaponStatsSo == null)
            {
                return;
            }
            if (_weaponStatsSo.isGraphicsRandom)
            {
                weaponGraphics = new WeaponGraphics(_weaponStatsSo.weaponGraphicsSo[Random.Range(0,_weaponStatsSo.weaponGraphicsSo.Length)]);
            }
            else
            {
                weaponGraphics = new WeaponGraphics(_weaponStatsSo.weaponGraphicsSo[0]);
            }
        }

        public override void Initialize(WeaponManager weaponManager)
        {
            base.Initialize(weaponManager);
            if (weaponGraphics != null)
            {
                Transform weaponSlot = weaponManager.GetMeleeSlot();
                currentWeaponGraphics = weaponSlot.gameObject.AddComponent<WeaponGraphicsMono>();
                currentWeaponGraphics.Initialize(weaponGraphics);
            }
            else
            {
                Debug.LogError("THERE IS ERROR WITH WEAPON GFX");
                
            }
        }

        public override void Attack()
        {
            base.Attack();
        }
    }
}