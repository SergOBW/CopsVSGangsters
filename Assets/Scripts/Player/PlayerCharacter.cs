using System;
using StarterAssets;
using UnityEngine;
public class PlayerCharacter : MonoBehaviour
{
    private ArmControllerScript _armControllerScript;
    private FirstPersonController _firstPersonController;
    private AiSensor _aiSensor;
    [SerializeField] private Camera armsCamera;
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
        return _armControllerScript.IsAiming;
    }

    public string GetWeaponName()
    {
        Debug.Log("GetWeaponName");
        throw new NotImplementedException();
    }

    public int GetAmmunitionCurrent()
    {
        return _armControllerScript.currentAmmo;
    }

    protected virtual void OnOnFireEvent()
    {
        OnFireEvent?.Invoke();
    }

    public void SetupArms(GameObject armControllerGo)
    {
        GameObject arms = Instantiate(armControllerGo, armsCamera.transform);
        arms.GetComponent<AimScript>().SetPlayerCamera(armsCamera);
        _armControllerScript = arms.GetComponent<ArmControllerScript>();
        _armControllerScript.OnFire += OnOnFireEvent;
    }
}
