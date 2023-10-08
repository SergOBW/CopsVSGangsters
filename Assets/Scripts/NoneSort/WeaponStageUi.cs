using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class WeaponStageUi : MonoBehaviour
{
    [SerializeField] private Button nextWeaponButton;
    [SerializeField] private Button previousWeaponButton;

    [SerializeField] private Button buyWeaponButton;
    [SerializeField] private TMP_Text weaponPriceText;

    [SerializeField] private GameObject weaponOpenBotBar;
    [SerializeField] private GameObject weaponCanBuyBar;
    [SerializeField] private GameObject weaponLockedBotBar;
    

    private int _currentWeapon;
    private List<PlayerWeaponStats> _playerWeaponStatsList = new List<PlayerWeaponStats>();
    private WeaponStatUpgradeUi[] _weaponStatUpgradeUi;


    public void Initialize()
    {
        nextWeaponButton.onClick.AddListener(NextWeapon);
        previousWeaponButton.onClick.AddListener(PreviousWeapon);
        buyWeaponButton.onClick.AddListener(TryToBuyWeapon);
        _weaponStatUpgradeUi = weaponOpenBotBar.GetComponentsInChildren<WeaponStatUpgradeUi>();
        WeaponManagerMechanic.Instance.OnNewWeaponValues += NewWeaponLoaded;
        _playerWeaponStatsList = new List<PlayerWeaponStats>();
        _playerWeaponStatsList = WeaponManagerMechanic.Instance.GetWeaponsStats();
        if (_playerWeaponStatsList.Count > 0)
        {
            if (_currentWeapon <= 0)
            {
                _currentWeapon = 0;
            }
            SetWeapon();
        }
        else
        {
            Debug.Log("Some error with weaponStats");
        }
    }

    private void NewWeaponLoaded()
    {
        SetWeapon();
    }

    private void NextWeapon()
    {
        if (_currentWeapon + 1 <= _playerWeaponStatsList.Count - 1)
        {
            _currentWeapon += 1;
        }
        else
        {
            _currentWeapon = 0;
        }
        
        SetWeapon();
    }

    private void PreviousWeapon()
    {
        if (_currentWeapon - 1 >= 0)
        {
            _currentWeapon -= 1;
        }
        else
        {
            _currentWeapon = _playerWeaponStatsList.Count - 1;
        }
        
        SetWeapon();
    }

    private void SetWeapon()
    {
        Refresh();
        
        WeaponManagerMechanic.Instance.SetWeapon(_playerWeaponStatsList[_currentWeapon].WeaponName);
        
        FindObjectOfType<WeaponsPreview>().SetWeapon(_currentWeapon);
    }

    private void Refresh()
    {
        _playerWeaponStatsList = new List<PlayerWeaponStats>();
        _playerWeaponStatsList = WeaponManagerMechanic.Instance.GetWeaponsStats();
        if (_playerWeaponStatsList[_currentWeapon].IsOpen == 1)
        {
            weaponOpenBotBar.SetActive(true);
            weaponCanBuyBar.SetActive(false);
            weaponLockedBotBar.SetActive(false);
        }
        else
        {
            if (_playerWeaponStatsList[_currentWeapon - 1].IsOpen == 1)
            {
                weaponOpenBotBar.SetActive(false);
                weaponCanBuyBar.SetActive(true);
                weaponLockedBotBar.SetActive(false);
            }
            else
            {
                weaponOpenBotBar.SetActive(false);
                weaponCanBuyBar.SetActive(false);
                weaponLockedBotBar.SetActive(true);
            }
        }
        
        // Refresh old
        if (_weaponStatUpgradeUi.Length > 0)
        {
            foreach (var weaponStatUpgradeUi in _weaponStatUpgradeUi)
            {
                weaponStatUpgradeUi.Deinitialize();
            }
        }
        // Setup new
        foreach (var weaponStatUpgradeUi in _weaponStatUpgradeUi)
        {
            weaponStatUpgradeUi.Setup(_playerWeaponStatsList[_currentWeapon].WeaponName);
        }

        weaponPriceText.text = _playerWeaponStatsList[_currentWeapon].WeaponBuyPrice.ToString();
    }

    public void DeInitialize()
    {
        nextWeaponButton.onClick.RemoveListener(NextWeapon);
        previousWeaponButton.onClick.RemoveListener(PreviousWeapon);
        buyWeaponButton.onClick.RemoveListener(TryToBuyWeapon);
        foreach (var weaponStatUpgradeUi in _weaponStatUpgradeUi)
        {
            weaponStatUpgradeUi.Deinitialize();
        }
        WeaponManagerMechanic.Instance.OnNewWeaponValues -= NewWeaponLoaded;
    }

    private void TryToBuyWeapon()
    {
        if (WeaponManagerMechanic.Instance.TryToBuyWeapon(_playerWeaponStatsList[_currentWeapon].WeaponName))
        {
            SetWeapon();
        }
    }
}
