namespace ForWeapon.New
{
    public class SaveWeapon
    {
        public string name;
        public string attachments;
        public int IsOpen;
        
        public int DamageLevel;
        public int ReloadSpeedLevel;
        public int AccuracyLevel;
        public int BulletCountLevel;

        public SaveWeapon(string name, string attachments, int isOpen)
        {
            this.name = name;
            
            this.attachments = attachments;
            
            this.IsOpen = isOpen;

            DamageLevel = 0;
            ReloadSpeedLevel = 0;
            AccuracyLevel = 0;
            BulletCountLevel = 0;
        }

        public void SetAttachments(string attachments)
        {
            this.attachments = attachments;
        }
    }
}