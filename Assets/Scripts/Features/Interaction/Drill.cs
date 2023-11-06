using Abstract.Inventory;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Drill : MonoBehaviour
{
    [SerializeField] private TMP_Text timeRemaining;
    [SerializeField] private Slider slider;
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip drillIn;
    [SerializeField] private AudioClip drillLoop;
    [SerializeField] private AudioClip drillEnd;

    private DrilledDoor _drilledDoor;

    private float _timer;
    private bool _isDrilled;

    [SerializeField] private float drillTimer = 60;

    private float _drillSpeed;

    public void StartDrill(DrilledDoor drilledDoor)
    {
        drillTimer = Random.Range(10, 15);
        slider.minValue = 0;
        slider.maxValue = drillTimer;
        _timer = 0;
        _isDrilled = true;
        _drilledDoor = drilledDoor;

        _drillSpeed = Inventory.Instance.HasItem("Big drill") ? 2 : 1;
        
        audioSource.clip = drillIn;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Update()
    {
        if (!_isDrilled)
        {
            return;
        }
        if (_timer < drillTimer)
        {
            _timer += Time.deltaTime * _drillSpeed;
            if (_timer >= drillIn.length && audioSource.clip != drillLoop && _timer < drillTimer - drillEnd.length)
            {
                audioSource.clip = drillLoop;
                audioSource.Play();
            }

            if (_timer >= drillTimer - drillEnd.length && audioSource.clip != drillEnd)
            {
                audioSource.clip = drillEnd;
                audioSource.loop = false;
                audioSource.Play();
            }
        }
        else
        {
            _drilledDoor.DrillCompleted();
            Destroy(gameObject);
        }

        slider.value = _timer;
        int timerInt = (int)Mathf.Abs(_timer - drillTimer);
        timeRemaining.text = timerInt.ToString();
    }
}
