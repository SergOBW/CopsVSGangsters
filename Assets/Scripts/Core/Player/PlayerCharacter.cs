using System;
using System.Collections.Generic;
using DefaultNamespace;
using Player;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    private ArmControllerScript _currentWeapon;
    private FirstPersonController _firstPersonController;
    private Sensor _sensor;
    [SerializeField] private Camera armsCamera;
    private List<ArmControllerScript> _armControllerScripts = new List<ArmControllerScript>();
    private int _currentWeaponIndex;
    
    private void Awake()
    {
        _sensor = GetComponent<Sensor>();
        _firstPersonController = GetComponent<FirstPersonController>();
    }


    public bool IsSeeEnemy(GameObject gameObject)
    {
        if (_sensor != null)
        {
            return _sensor.IsInSight(gameObject);
        }
        return false;
    }

    public event Action OnFireEvent;

    public void SetupSensitivity(float getPlayerSensitivity)
    {
        _firstPersonController.SetSensitivity(getPlayerSensitivity);
    }

    public bool IsAiming()
    {
        return _currentWeapon.IsAiming;
    }

    public void Pause()
    {
        LevelStateMachine.Instance.Pause();
    }
    

    public int GetAmmunitionCurrent()
    {
        return _currentWeapon.currentAmmo;
    }

    protected virtual void OnOnFireEvent()
    {
        OnFireEvent?.Invoke();
    }

    public void SetupArms()
    {
        List<PlayerWeaponStats> avaibleWeaponStatsList = WeaponManagerMechanic.Instance.GetCurrentWeapons();
        _armControllerScripts = new List<ArmControllerScript>();
        foreach (var playerWeaponStats in avaibleWeaponStatsList)
        {
            GameObject gameObject =  Instantiate(playerWeaponStats.WeaponArms, armsCamera.transform);
            ArmControllerScript armControllerScript = gameObject.GetComponent<ArmControllerScript>();
            if (gameObject.TryGetComponent(out AimScript aimScript))
            {
                aimScript.SetPlayerCamera(armsCamera);
            }

            armControllerScript.SetupWeapon(playerWeaponStats);
            armControllerScript.SetCamera(armsCamera);
            _armControllerScripts.Add(armControllerScript);
        }

        _currentWeaponIndex = 0;
        PickWeapon(WeaponType.Secondary);
        SetupSensitivity(GlobalSettings.Instance.sensitivity);
        GlobalSettings.Instance.OnSettingsChanged += () =>  SetupSensitivity(GlobalSettings.Instance.sensitivity);
    }


    public void PickWeapon(WeaponType weaponType)
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.OnFire -= OnOnFireEvent;
        }

        foreach (var armControllerScript in _armControllerScripts)
        {
            armControllerScript.gameObject.SetActive(false);
        }

        foreach (var armController in _armControllerScripts)
        {
            if (armController.weaponType == weaponType)
            {
                _currentWeapon = armController;
                break;
            }
        }
        _currentWeapon.gameObject.SetActive(true);
        WeaponManagerMechanic.Instance.PickWeapon(_currentWeapon.currentWeaponStats);
        _currentWeapon.OnFire += OnOnFireEvent;
    }
    
    public void OnTryInventoryNext(InputAction.CallbackContext context)
    {
        if (_armControllerScripts.Count <= 0 )
        {
            return;
        }
        
        //Switch.
        switch (context)
        {
            //Performed.
            case {phase: InputActionPhase.Performed}:
                //Get the index increment direction for our inventory using the scroll wheel direction. If we're not
                //actually using one, then just increment by one.
                float scrollValue = context.valueType.IsEquivalentTo(typeof(Vector2)) ? Mathf.Sign(context.ReadValue<Vector2>().y) : 1.0f;
					
                //Get the next index to switch to.
                int indexNext = scrollValue > 0 ? GetNextIndex() : _armControllerScripts.Count - 1;
                //Get the current weapon's index.
					
                //Make sure we're allowed to change, and also that we're not using the same index, otherwise weird things happen!
                if (_currentWeaponIndex != indexNext)
                    _currentWeaponIndex = indexNext;
                //PickWeapon(_currentWeaponIndex);
                break;
        }
    }
    
    public void OnNumberInput(InputAction.CallbackContext callbackContext)
    {
        int numKeyValue;
        if (callbackContext.performed)
        {
            int.TryParse(callbackContext.control.name, out numKeyValue);
            switch (numKeyValue)
            {
                case 1:
                    PickWeapon(WeaponType.Primary);
                    break;
                case 2:
                    PickWeapon(WeaponType.Secondary);
                    break;
                case 3:
                    PickWeapon(WeaponType.Melee);
                    break;
            }
            
        }
    }

    private int GetNextIndex()
    {
        if (_currentWeaponIndex + 1 <= _armControllerScripts.Count - 1)
        {
            _currentWeaponIndex += 1;
        }
        else
        {
            _currentWeaponIndex = 0;
        }

        return _currentWeaponIndex;
    }

    public void DoHealthBonus()
    {
       GetComponent<PlayerStatsController>().Heal();
    }
}
