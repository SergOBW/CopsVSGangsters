﻿using Interaction;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Quests.Item
{
    public class LootItem : QuestItem, IInteractable
    {
        public string ruName;
        public string enName;
        public string trName;

        [SerializeField] private Outline _outline;
        [SerializeField] private GameObject[] _visuals;
        
        private bool _isUsed;
        private bool _isSelected;

        private void Awake()
        {
            _isUsed = false;
            _isSelected = false;
            foreach (var gameObject in _visuals)
            {
                gameObject.SetActive(false);
            }
            _visuals[Random.Range(0,_visuals.Length)].SetActive(true);
        }
        public void Interact()
        {
            if (!_isUsed)
            {
                QuestsMechanic.Instance.TryToProgressQuest(this);
                Destroy(gameObject);
                _isUsed = true;
            }
        }

        private void Update()
        {
            if (_isSelected)
            {
                _outline.enabled = true;
            }
            else
            {
                _outline.enabled = false;
            }
        }
        
        
        public void HighLight()
        {
            _isSelected = true;
        }
    }
}