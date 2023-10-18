using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponUi : MonoBehaviour
{
    private PlayerWeaponStats _playerWeaponStats;

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
    
    public void SetupNewWeapon(PlayerWeaponStats playerWeaponStats)
    {
        _playerWeaponStats = playerWeaponStats;
        FindObjectOfType<WeaponPreview>().ShowWeapon(_currentIndex);
        RefreshUi();
    }

    private void RefreshUi()
    {
        damageText.text = _playerWeaponStats.Damage.ToString();
        accuracyText.text = _playerWeaponStats.Accuracy.ToString();
        reloadSpeed.text = _playerWeaponStats.ReloadSpeed.ToString();

        weaponPrice.text = _playerWeaponStats.WeaponBuyPrice.ToString();

        weaponName.text = _playerWeaponStats.WeaponName;

        if (_playerWeaponStats.IsOpen)
        {
            weaponBuyButton.gameObject.SetActive(false);
        }
        else
        {
            weaponBuyButton.gameObject.SetActive(true);
        }

        if (_playerWeaponStats.IsEquiped)
        {
            equipWeaponButton.gameObject.SetActive(false);
        }
        else
        {
            equipWeaponButton.gameObject.SetActive(true);
        }
        
        
    }

    public void Initialize()
    {
        _allPlayerWeaponStatsList = new List<PlayerWeaponStats>();
        _allPlayerWeaponStatsList = WeaponManagerMechanic.Instance.GetPlayerWeaponStats();
        
        for (int i = 0; i < _allPlayerWeaponStatsList.Count; i++)
        {
            if (_allPlayerWeaponStatsList[i].WeaponName == WeaponManagerMechanic.Instance.GetCurrentWeapon().WeaponName)
            {
                _currentIndex = i;
                break;
            }
        }
        
        nextButton.onClick.AddListener(Next);
        previousButton.onClick.AddListener(Previous);
        
        
        SetupNewWeapon(_allPlayerWeaponStatsList[_currentIndex]);
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
        
        
        SetupNewWeapon(_allPlayerWeaponStatsList[_currentIndex]);
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
        SetupNewWeapon(_allPlayerWeaponStatsList[_currentIndex]);
    }
}
