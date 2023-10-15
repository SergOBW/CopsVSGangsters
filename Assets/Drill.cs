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

    public void StartDrill(DrilledDoor drilledDoor)
    {
        slider.minValue = 0;
        slider.maxValue = drillTimer;
        _timer = drillTimer;
        _isDrilled = true;
        _drilledDoor = drilledDoor;
    }

    private void Update()
    {
        if (!_isDrilled)
        {
            return;
        }
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
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
