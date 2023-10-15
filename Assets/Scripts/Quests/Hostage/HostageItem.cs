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
                _isUsed = true;
            }
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