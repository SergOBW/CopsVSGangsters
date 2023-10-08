namespace DefaultNamespace
{
    public class StatsContainer
    {
        public int startHealth;
        public bool isDead;
        public int Startarmour;
        public float speed;
        public StatsContainer(StatsSo statsSo)
        {
            isDead = false;
            startHealth = statsSo.startHealth;
            Startarmour = statsSo.startArmour;
            speed = statsSo.speed;
        }
    }
}