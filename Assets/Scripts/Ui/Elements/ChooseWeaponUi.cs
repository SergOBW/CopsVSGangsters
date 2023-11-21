using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ChooseWeaponUi : MonoBehaviour
{
    private PlayerWeaponStats _currentWepaonStats;

    [SerializeField] private TMP_Text damageText;
    [SerializeField] private TMP_Text accuracyText;
    [SerializeField] private TMP_Text reloadSpeed;

    [SerializeField] private Button weaponBuyButton;
    [SerializeField] private TMP_Text weaponPrice;

    [SerializeField] private TMP_Text weaponName;
    [SerializeField] private Button equipWeaponButton;

    [SerializeField] private Button nextButton;
    [SerializeField] private Button previousButton;

    private List<PlayerWeaponStats> _allPlayerWeaponStatsList = new List<PlayerWeaponStats>();
    private int _currentIndex;
    
    public void Initialize()
    {
        _allPlayerWeaponStatsList = new List<PlayerWeaponStats>();
        _allPlayerWeaponStatsList = WeaponManagerMechanic.Instance.GetPlayerWeaponStats();
        
        _currentIndex = 0;

        WeaponManagerMechanic.Instance.OnPlayerLoadOutChanges += RefreshUi;
        
        nextButton.onClick.AddListener(Next);
        previousButton.onClick.AddListener(Previous);
        equipWeaponButton.onClick.AddListener(EquipWeapon);
        weaponBuyButton.onClick.AddListener(BuyWeapon);
        RefreshUi();
    }
    
    public void DeInitialize()
    {
        WeaponManagerMechanic.Instance.OnPlayerLoadOutChanges -= RefreshUi;
        nextButton.onClick.RemoveListener(Next);
        previousButton.onClick.RemoveListener(Previous);
        equipWeaponButton.onClick.RemoveListener(EquipWeapon);
        weaponBuyButton.onClick.RemoveListener(BuyWeapon);
    }
    
    public void SetupNewWeapon(PlayerWeaponStats playerWeaponStats)
    {
        _currentWepaonStats = playerWeaponStats;
        FindObjectOfType<WeaponPreview>().ShowWeapon(_currentIndex);
    }

    private void RefreshUi()
    {
        _allPlayerWeaponStatsList = WeaponManagerMechanic.Instance.GetPlayerWeaponStats();
        SetupNewWeapon(_allPlayerWeaponStatsList[_currentIndex]);
        
        damageText.text = _currentWepaonStats.Damage.ToString();
        accuracyText.text = _currentWepaonStats.Accuracy.ToString();
        reloadSpeed.text = _currentWepaonStats.ReloadSpeed.ToString();

        weaponPrice.text = _currentWepaonStats.WeaponBuyPrice.ToString();

        switch (LanguageManager.Instance.GetLanguage())
        {
            case Language.en:
                weaponName.text = _currentWepaonStats.WeaponName;
                break;
            case Language.ru:
                weaponName.text = _currentWepaonStats.WeaponNameRu;
                break;
            case Language.tr:
                weaponName.text = _currentWepaonStats.WeaponNameTr;
                break;
            default:
                weaponName.text = _currentWepaonStats.WeaponName;
                break;
        }

        weaponBuyButton.gameObject.SetActive(!_currentWepaonStats.IsOpen);

        equipWeaponButton.gameObject.SetActive(!_currentWepaonStats.IsEquiped);
    }

    private void Next()
    {
        if (_currentIndex + 1 > _allPlayerWeaponStatsList.Count - 1)
        {
            _currentIndex = 0;
        }
        else
        {
            _currentIndex++;
        }
        
        
        RefreshUi();
    }
    
    private void Previous()
    {
        if (_currentIndex - 1 < 0)
        {
            _currentIndex = _allPlayerWeaponStatsList.Count - 1;
        }
        else
        {
            _currentIndex--;
        }
        RefreshUi();
    }

    private void BuyWeapon()
    {
        WeaponManagerMechanic.Instance.TryToBuy(_currentWepaonStats);
    }


    private void EquipWeapon()
    {
        WeaponManagerMechanic.Instance.TryToEquip(_currentWepaonStats);
    }
    
}
