using DefaultNamespace;
using UnityEngine;

namespace ForWeapon
{
    public class DefaultLongRangeWeapon : Weapon
    {
        // Visuals
        public WeaponGraphics weaponGraphics;

        // Realisation
        public int AmmoMaxInMag { get; private set;}
        public int AmmoTotal { get; private set; }
        public int AmmoCurrentMag{ get; private set; }

        private LongRangeWeaponStatsSo _longRangeWeaponStats;
        private WeaponGraphicsMono currentWeaponGraphics;
        public DefaultLongRangeWeapon(WeaponStatsSo weaponStatsSo) : base(weaponStatsSo)
        {
            _longRangeWeaponStats = weaponStatsSo as LongRangeWeaponStatsSo;

            if (_longRangeWeaponStats != null)
            {
                AmmoMaxInMag = _longRangeWeaponStats.ammoMaxInMag;
                if (_longRangeWeaponStats.isGraphicsRandom)
                {
                    weaponGraphics = new WeaponGraphics(_longRangeWeaponStats.weaponGraphicsSo[Random.Range(0,_longRangeWeaponStats.weaponGraphicsSo.Length)]);
                }
                else
                {
                    weaponGraphics = new WeaponGraphics(_longRangeWeaponStats.weaponGraphicsSo[0]);
                }
            }
            else
            {
                Debug.LogError("THERE IS ERROR WITH WEAPON");
            }
        }
        public override void Initialize(WeaponManager weaponManager)
        {
            base.Initialize(weaponManager);
            if (weaponGraphics != null)
            {
                Transform weaponSlot = weaponManager.GetWeaponSlot();
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
            AmmoCurrentMag--;
            if (currentWeaponGraphics == null)
            {
                return;
            }

            currentWeaponGraphics.PlayMuzzle();
        }

        public override void Reload()
        {
            int ammoToFill = 0;
            if (AmmoTotal < AmmoMaxInMag)
            {
                ammoToFill = AmmoTotal;
                AmmoTotal -= ammoToFill;
            }
            else if (AmmoTotal >= AmmoMaxInMag)
            {
                ammoToFill = AmmoMaxInMag;
                AmmoTotal -= ammoToFill;
            }

            if (ammoToFill <= 0)
            {
                ammoToFill = 0;
                Debug.Log("You have no ammo");
            }
            AmmoCurrentMag = ammoToFill;
        }
        
    }
}