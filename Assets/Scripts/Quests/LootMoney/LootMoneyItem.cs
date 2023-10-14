using Interaction;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Quests.LootMoney
{
    public class LootMoneyItem : QuestItem , IInteractable
    {
        [SerializeField] private Outline _outline;
        [SerializeField] private GameObject[] _visuals;
        
        [SerializeField] private int moneyAmount;
        [SerializeField] private float maxHealth;
        private float _currentHealth;

        private void Awake()
        {
            foreach (var gameObject in _visuals)
            {
                gameObject.SetActive(false);
            }
            _visuals[Random.Range(0,_visuals.Length)].SetActive(true);
        }

        public void Interact()
        {
            _currentHealth += Time.deltaTime;
            _outline.enabled = true;
            if (_currentHealth >= maxHealth)
            {
                QuestsMechanic.Instance.TryToProgressQuest(this);
                InteractionCanceled();
                Destroy(gameObject);
            }
        }

        public void InteractionCanceled()
        {
            _outline.enabled = false;
        }
        
        public float GetHealth()
        {
            return _currentHealth;
        }

        public float GetMaxHealth()
        {
            return maxHealth;
        }

        public int GetMoneyAmount()
        {
            return moneyAmount;
        }
    }
}