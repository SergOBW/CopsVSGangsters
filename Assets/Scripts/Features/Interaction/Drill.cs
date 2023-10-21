using Abstract.Inventory;
using EnemyCore;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Drill : MonoBehaviour
{
    [SerializeField] private Transform drillKnife;
    [SerializeField] private TMP_Text timeRemaining;
    [SerializeField] private Slider slider;

    private DrilledDoor _drilledDoor;

    private float _timer;
    private bool _isDrilled;

    [SerializeField] private float drillTimer = 60;

    private float _drillSpeed;

    public void StartDrill(DrilledDoor drilledDoor)
    {
        slider.minValue = 0;
        slider.maxValue = drillTimer;
        _timer = drillTimer;
        _isDrilled = true;
        _drilledDoor = drilledDoor;
        EnemyHandleMechanic.Instance.SpawnEnemyWave();

        if (Inventory.Instance.HasItem("Big drill"))
        {
            _drillSpeed = 2;
        }
        else
        {
            _drillSpeed = 1;
        }
    }

    private void Update()
    {
        if (!_isDrilled)
        {
            return;
        }
        if (_timer > 0)
        {
            _timer -= Time.deltaTime * _drillSpeed;
        }
        else
        {
            _drilledDoor.DrillCompleted();
            Destroy(gameObject);
        }

        slider.value = _timer;
        int timerInt = (int)_timer;
        timeRemaining.text = timerInt.ToString();
    }
}
