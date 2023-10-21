namespace DefaultNamespace
{
    public class EnemySpawnPoint : SpawnPoint
    {
        public EnemyBehaviour enemyBehaviour;

        public bool CanSpawnEnemyHere(EnemySo enemySo)
        {
            if (!isEmpty)
            {
                return false;
            }

            foreach (var enemyBehaviour in enemySo.enemyBehaviour)
            {
                if (enemyBehaviour == this.enemyBehaviour)
                {
                    return true;
                }
            }

            return false;
        }
    }
}