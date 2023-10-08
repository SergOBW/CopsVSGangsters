using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum WeaponStatType
{
    Damage = 0,
    ReloadSpeed = 1,
    BulletCount = 2,
    Accuracy = 3
}

public class WeaponStatUpgradeUi : MonoBehaviour
{
    [SerializeField] private WeaponStatType _weaponStatType;
    [SerializeField] private Button purchaseButton;

    [SerializeField] private TMP_Text itemCost;
    [SerializeField] private TMP_Text currentItemValue;

    [SerializeField] private Sprite[] itemLevelSprites;
    [SerializeField] private Image itemLevelImage;
    
    private PlayerWeaponStats _currentPlayerWeaponStats;

    private string weaponName;
    

    public void Setup(string weaponName)
    {
        this.weaponName = weaponName;
        Refresh();
        purchaseButton.onClick.AddListener(TryToBuy);
        WeaponManagerMechanic.Instance.OnNewWeaponValues += Refresh;
    }

    private void Refresh()
    {
        _currentPlayerWeaponStats = WeaponManagerMechanic.Instance.GetWeaponStats(weaponName);
        switch (_weaponStatType)
        {
            case WeaponStatType.Damage:
                itemLevelImage.sprite = itemLevelSprites[_currentPlayerWeaponStats.DamageLevel];
                currentItemValue.text = _currentPlayerWeaponStats.Damage.ToString();
                itemCost.text = _currentPlayerWeaponStats.DamageUpgradePrice.ToString();
                break;
            case WeaponStatType.ReloadSpeed:
                itemLevelImage.sprite = itemLevelSprites[_currentPlayerWeaponStats.ReloadSpeedLevel];
                currentItemValue.text = _currentPlayerWeaponStats.ReloadSpeed.ToString();
                itemCost.text = _currentPlayerWeaponStats.ReloadSpeedPrice.ToString();
                break;
            case WeaponStatType.BulletCount:
                itemLevelImage.sprite = itemLevelSprites[_currentPlayerWeaponStats.BulletCountLevel];
                currentItemValue.text = _currentPlayerWeaponStats.BulletCount.ToString();
                itemCost.text = _currentPlayerWeaponStats.BulletCountPrice.ToString();
                break;
            case WeaponStatType.Accuracy:
                itemLevelImage.sprite = itemLevelSprites[_currentPlayerWeaponStats.AccuracyLevel];
                currentItemValue.text = (100 - _currentPlayerWeaponStats.Accuracy * 10) + " %";
                itemCost.text = _currentPlayerWeaponStats.AccuracyPrice.ToString();
                break;
        }

        if (WeaponManagerMechanic.Instance.IsMaxLevel(_weaponStatType,_currentPlayerWeaponStats.WeaponName))
        {
            purchaseButton.gameObject.SetActive(false);
        }
        else
        {
            purchaseButton.gameObject.SetActive(true);
        }
    }

    private void TryToBuy()
    {
        WeaponManagerMechanic.Instance.TryToUpgradeStat(_weaponStatType);
        Refresh();
    }

    public void Deinitialize()
    {
        purchaseButton.onClick.RemoveListener(TryToBuy);
        WeaponManagerMechanic.Instance.OnNewWeaponValues -= Refresh;
    }
}
