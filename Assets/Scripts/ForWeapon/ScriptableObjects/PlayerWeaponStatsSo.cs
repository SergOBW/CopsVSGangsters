using UnityEngine;

namespace ForWeapon
{
    [CreateAssetMenu()]
    public class PlayerWeaponStatsSo : ScriptableObject
    {
        [Header("Weapon name")]
        public string weaponName;

        public int weaponBuyPrice;
        
        [Header("Weapon stats price")]
        public int startedDamageCost;
        public int startedReloadSpeedCost;
        public int startedAccuracyCost;
        public int startedBulletCountCost;
        
        [Header("Stats")]

        public int startedDamage = 30;
        public float startedReloadSpeed = 1;
        public float startedAccuracy = 0.25f;
        public int startedBulletCount = 7;

        [Header("Stats multiplayer")]
        public float damageLevelMultiplayer = 5;
        public float reloadSpeedLevelMultiplayer = 0.2f;
        public float accuracyLevelMultiplayer = 0.2f;
        public int bulletCountLevelMultiplayer = 1;
        
        [Header("Price multiplayer")]
        public float damagePriceLevelMultiplayer = 1000;
        public float reloadSpeedPriceLevelMultiplayer = 1000;
        public float accuracyPriceLevelMultiplayer = 1000;
        public int bulletCountPriceLevelMultiplayer = 1000;

        [Header("Visuals")] 
        public GameObject weaponArms;

        [Header("Options")]
        public bool isStarted;
    }
}