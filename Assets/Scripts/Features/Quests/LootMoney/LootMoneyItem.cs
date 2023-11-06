using UnityEngine;
using Random = UnityEngine.Random;

namespace Quests.LootMoney
{
    public enum LootMoneyType
    {
        Default = 0,
        Small = 1,
        Big = 2,
        Bonus = 3
    }
    public class LootMoneyItem : InteractableWithHealth 
    {
        [SerializeField] private GameObject[] _visuals;
        
        private float _moneyAmount;

        public LootMoneyType lootMoneyType;

        protected override void Initialize()
        {
            base.Initialize();
            foreach (var gameObject in _visuals)
            {
                gameObject.SetActive(false);
            }
            _visuals[Random.Range(0,_visuals.Length)].SetActive(true);
        }

        protected override void Handle()
        {
            base.Handle();
            EconomyMonoMechanic.Instance.AddTempMoney(_moneyAmount);
            SoundMonoMechanic.Instance.PlayBuy();
            Destroy(gameObject);
        }

        public float GetMoneyAmount()
        {
            return _moneyAmount;
        }

        public void ChangeMoneyAmount(float moneyOnEachLootItem)
        {
            _moneyAmount = moneyOnEachLootItem;
        }
    }
}