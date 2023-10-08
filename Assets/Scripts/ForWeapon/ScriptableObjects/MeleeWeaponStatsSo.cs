using UnityEngine;

namespace ForWeapon
{
    [CreateAssetMenu()]
    public class MeleeWeaponStatsSo : WeaponStatsSo
    {
        public WeaponGraphicsSo[] weaponGraphicsSo;
        public bool isGraphicsRandom;

        public override Weapon Initialize()
        {
            Weapon weapon = new MeleeWeapon(this);
            return weapon;
        }
    }
}