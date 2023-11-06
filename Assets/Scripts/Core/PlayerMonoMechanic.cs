using Abstract;
using DefaultNamespace;
using Player;
using UnityEngine;

namespace Core
{
    public class PlayerMonoMechanic : GlobalMonoMechanic
    {
        [SerializeField] private Stats playerStats;
        [SerializeField] private PlayerCharacter characterPrefab;
        public static PlayerMonoMechanic Instance;
        public override void Initialize()
        {
            Instance = this;
        }

        public void SpawnNewCharacter()
        {
            PlayerCharacter playerGameObject =  Instantiate(characterPrefab, FindObjectOfType<PlayerSpawnPoint>().transform.position,FindObjectOfType<PlayerSpawnPoint>().transform.rotation);
            PlayerStatsController playerStatsController = playerGameObject.GetComponent<PlayerStatsController>();
            playerGameObject.GetComponent<PlayerCharacter>().SetupArms();
            playerStatsController.Initialize(playerStats);
            playerStatsController.OnPlayerDie += LooseLevel;
        }

        private void LooseLevel()
        {
            LevelsMonoMechanic.Instance.LooseLevel();
        }

        public void ReviveBonus()
        {
            FindObjectOfType<PlayerStatsController>().ReviveBonus();
        }

        public void SetWeapon(WeaponType weaponType)
        {
            FindObjectOfType<PlayerCharacter>().PickWeapon(weaponType);
        }
    }
}
