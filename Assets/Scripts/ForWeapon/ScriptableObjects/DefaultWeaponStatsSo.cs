using UnityEngine;

namespace ForWeapon
{
    [CreateAssetMenu()]
    public class DefaultWeaponStatsSo : WeaponStatsSo
    {
        public override Weapon Initialize()
        {
            Weapon weapon = new Weapon(this);
            return weapon;
        }
    }
}