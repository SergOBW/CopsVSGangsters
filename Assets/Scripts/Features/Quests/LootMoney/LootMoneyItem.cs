using UnityEngine;
using Random = UnityEngine.Random;

namespace Quests.LootMoney
{
    public class LootMoneyItem : InteractableWithHealth 
    {
        [SerializeField] private GameObject[] _visuals;
        
        [SerializeField] private int moneyAmount;

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
            Debug.Log($"Money amount {moneyAmount} was looted");
            Destroy(gameObject);
        }

        public int GetMoneyAmount()
        {
            return moneyAmount;
        }
    }
}