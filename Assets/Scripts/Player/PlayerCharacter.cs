using System;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool isSeeEnemy(GameObject o)
    {
        throw new System.NotImplementedException();
    }

    public event Action OnFireEvent;

    public void SetupSensitivity(float getPlayerSensitivity)
    {
        Debug.Log("SETUP SENSITIVITY");
        throw new NotImplementedException();
    }

    public bool IsAiming()
    {
        Debug.Log("IS AIMING");
        throw new NotImplementedException();
    }

    public string GetWeaponName()
    {
        Debug.Log("GetWeaponName");
        throw new NotImplementedException();
    }

    public int GetAmmunitionCurrent()
    {
        throw new NotImplementedException();
    }
}
