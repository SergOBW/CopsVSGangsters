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
        
        [SerializeField] private float moneyAmount;

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
            EconomyMonoMechanic.Instance.AddTempMoney(moneyAmount);
            SoundMonoMechanic.Instance.PlayBuy();
            Debug.Log($"Money amount {moneyAmount} was looted");
            Destroy(gameObject);
        }

        public float GetMoneyAmount()
        {
            return moneyAmount;
        }

        public void ChangeMoneyAmount(float moneyOnEachLootItem)
        {
            moneyAmount = moneyOnEachLootItem;
        }
    }
}