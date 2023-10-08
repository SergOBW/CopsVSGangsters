
using UnityEngine;

namespace ForWeapon
{
    [CreateAssetMenu()]
    public class LongRangeWeaponStatsSo : WeaponStatsSo
    {
        public int ammoMaxInMag;
        
        public WeaponGraphicsSo[] weaponGraphicsSo;

        public bool isGraphicsRandom;

        public override Weapon Initialize()
        {
            Weapon weapon = new DefaultLongRangeWeapon(this);
            return weapon;
        }
    }
}