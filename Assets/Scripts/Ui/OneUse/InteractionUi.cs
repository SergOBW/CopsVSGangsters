using DefaultNamespace;
using Quests.Item;
using Quests.LootMoney;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class InteractionUi : MonoBehaviour
{
    [SerializeField] private TMP_Text _text;
    [SerializeField] private Image _image;
    private PlayerDetector _playerDetector;
    public void Initialize()
    {
        _playerDetector = FindObjectOfType<PlayerDetector>();
        _image.fillAmount = 0;
    }

    private void Update()
    {
        if (_playerDetector == null)
        {
            return;
        }

        if (_playerDetector.CurrentInteractable != null)
        {
            switch (_playerDetector.CurrentInteractable)
            {
                case LootItem :
                    HandleLootItem();
                    break;
                case LootMoneyItem:
                    HandleMoneyItem();
                    break;
                default: Default();
                    break;
            }
        }
        else
        {
            _image.fillAmount = 0;
            _text.gameObject.SetActive(false);
        }

    }

    private void Default()
    {
        if (_playerDetector.CurrentInteractable.CanInteract())
        {
            _text.gameObject.SetActive(true);
        }
        else
        {
            _text.gameObject.SetActive(false);
        }
    }

    private void HandleMoneyItem()
    {
        LootMoneyItem lootItem = _playerDetector.CurrentInteractable as LootMoneyItem;
        if (lootItem != null)
        {
            if (lootItem.GetMaxHealth() <= 0)
            {
                _image.fillAmount = 0;
            }
            else
            {
                float fillAmount = SergOBWUtils.GetNormalizeNumber(lootItem.GetHealth(),0,lootItem.GetMaxHealth());
                _image.fillAmount = fillAmount;
            }
            _text.gameObject.SetActive(true);
        }
        else
        {
            _image.fillAmount = 0;
            _text.gameObject.SetActive(false);
        }
    }

    private void HandleLootItem()
    {
        LootItem lootItem = _playerDetector.CurrentInteractable as LootItem;
        if (lootItem != null)
        {
            _text.gameObject.SetActive(true);
        }
        else
        {
            _text.gameObject.SetActive(false);
        }
    }
}
