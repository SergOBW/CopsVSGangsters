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
}
