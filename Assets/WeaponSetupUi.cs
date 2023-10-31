using System;
using UnityEngine;
using UnityEngine.UI;

public class WeaponSetupUi : MonoBehaviour
{
    [SerializeField] private Image primaryImage;
    [SerializeField] private Image secondaryImage;
    [SerializeField] private Image meleeImage;
    private void OnEnable()
    {
        WeaponManagerMechanic.Instance.OnCurrentWeaponListChanged += Refresh;
        Refresh();
    }

    private void OnDisable()
    {
        WeaponManagerMechanic.Instance.OnCurrentWeaponListChanged -= Refresh;
    }

    private void Refresh()
    {
        foreach (var playerWeaponStats in WeaponManagerMechanic.Instance.GetCurrentWeapons())
        {
            switch (playerWeaponStats.weaponType)
            {
                case WeaponType.Melee:
                    meleeImage.sprite = playerWeaponStats.WeaponIcon;
                    break;
                case WeaponType.Secondary:
                    secondaryImage.sprite = playerWeaponStats.WeaponIcon;
                    break;
                case WeaponType.Primary:
                    primaryImage.sprite = playerWeaponStats.WeaponIcon;
                    break;
            }
        }
    }
}
