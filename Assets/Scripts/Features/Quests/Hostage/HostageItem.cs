using EnemyCore;
using Interaction;
using UnityEngine;

namespace Quests.Hostage
{
    public class HostageItem : QuestItem, IInteractable
    {
        private bool _isUsed;

        private void Awake()
        {
            _isUsed = false;
        }

        public void Interact()
        {
            if (!_isUsed)
            {
                QuestsMechanic.Instance.TryToProgressQuest(this);
                GetComponent<Animator>().SetTrigger("Saved");
                EnemyHandleMechanic.Instance.SpawnEnemyWave();
                _isUsed = true;
            }
        }

        public float GetHealthNormalized()
        {
            return 0;
        }

        public bool CanInteract()
        {
            return true;
        }
        

        public void OnAnimationEnded()
        {
            Destroy(gameObject);
        }
    }
}