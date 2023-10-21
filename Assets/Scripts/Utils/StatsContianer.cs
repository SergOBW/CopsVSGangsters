using System;

namespace DefaultNamespace
{
    [Serializable]
    public class Stats
    {
        public int startHealth = 100;
        public int startArmour;
        
        public float startSpeed = 2;
        public float maxSpeed = 2;
        
        public bool isDead;

        public Stats(int startHealth = 100, int startArmour = 0, float startSpeed = 2, float maxSpeed = 2)
        {
            this.startHealth = startHealth;
            this.startArmour = startArmour;
            isDead = false;
            this.startSpeed = startSpeed;
            this.maxSpeed = maxSpeed;
        }
        
        public Stats(Stats stats)
        {
            startHealth = stats.startHealth;
            startArmour = stats.startArmour;
            isDead = false;
            startSpeed = stats.startSpeed;
            maxSpeed = stats.maxSpeed;
        }
    }
}