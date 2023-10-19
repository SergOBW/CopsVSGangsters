using System;
using System.Collections.Generic;
using StarterAssets;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCharacter : MonoBehaviour
{
    private ArmControllerScript _currentWeapon;
    private FirstPersonController _firstPersonController;
    private AiSensor _aiSensor;
    [SerializeField] private Camera armsCamera;
    private List<ArmControllerScript> _armControllerScripts = new List<ArmControllerScript>();
    private int _currentWeaponIndex;
    
    private void Awake()
    {
        _aiSensor = GetComponent<AiSensor>();
        _firstPersonController = GetComponent<FirstPersonController>();
    }


    public bool IsSeeEnemy(GameObject gameObject)
    {
        if (_aiSensor != null)
        {
            return _aiSensor.IsInSight(gameObject);
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
            _armControllerScripts.Add(armControllerScript);
        }

        _currentWeaponIndex = 0;
        PickWeapon(_currentWeaponIndex);
    }


    private void PickWeapon(int index)
    {
        if (_currentWeapon != null)
        {
            _currentWeapon.OnFire -= OnOnFireEvent;
        }

        foreach (var armControllerScript in _armControllerScripts)
        {
            armControllerScript.gameObject.SetActive(false);
        }
        _currentWeapon = _armControllerScripts[index];
        _currentWeapon.gameObject.SetActive(true);
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
                PickWeapon(_currentWeaponIndex);
                break;
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
}
