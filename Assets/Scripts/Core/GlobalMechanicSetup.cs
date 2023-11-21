using System.Collections.Generic;
using Abstract;
using UnityEngine;

public class GlobalMechanicSetup : MonoBehaviour
{
    [SerializeField] private List<GlobalMonoMechanic> _monoMechanics;

    public void InitializeMechanics()
    {
        for (int i = 0; i < _monoMechanics.Count; i++)
        {
            _monoMechanics[i].Initialize();
        }
    }
    
    private bool d1;
    private bool d2;
    private void Update()
    {
        d1 = (Input.GetKey(KeyCode.LeftShift) ? true : false);
        d2 = (Input.GetKey(KeyCode.Z) ? true : false); 
 
        if (d1 && d2)
        {
            SaveGameMechanic.Instance.ResetData();
        }
        
        
        if (d1 && Input.GetKeyDown(KeyCode.M))
        {
            EconomyMonoMechanic.Instance.AddMoney(1000000);
            SaveGameMechanic.Instance.SaveMoney();
        }
        
        if (d1 && Input.GetKeyDown(KeyCode.L))
        {
            LevelsMonoMechanic.Instance.OpenAllLevels();
        }
        
    }
}


