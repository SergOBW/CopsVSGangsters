namespace ForWeapon.New
{
    public class SaveWeapon
    {
        public string weaponName;
        public bool IsOpen;
        public bool isEquiped;
        
        public int DamageLevel;
        public int ReloadSpeedLevel;
        public int AccuracyLevel;
        public int BulletCountLevel;

        public SaveWeapon(string weaponName, bool isOpen, bool isEquiped)
        {
            this.weaponName = weaponName;
            
            IsOpen = isOpen;
            this.isEquiped = isEquiped;

            DamageLevel = 0;
            ReloadSpeedLevel = 0;
            AccuracyLevel = 0;
            BulletCountLevel = 0;
        }
        
    }
}