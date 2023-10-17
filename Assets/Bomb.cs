using TMPro;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float timerBeforeBoom = 5;
    private float _timer;
    
    
    [SerializeField] private TMP_Text _text;
    private BoomDoor _boomDoor;

    private bool _isUsed;
    public void Setup(BoomDoor boomDoor)
    {
        _boomDoor = boomDoor;
        _timer = timerBeforeBoom;
        _isUsed = true;
    }

    private void Update()
    {
        if (!_isUsed)
        {
            return;
        }
        if (_timer > 0)
        {
            _timer -= Time.deltaTime;
        }
        else
        {
            _boomDoor.Boom();
            return;
        }
        
        _boomDoor.HandleAllBombs(_timer);
        
    }

    public void HandleTime(float time)
    {
        int timer = (int)time;
        _text.text = timer.ToString();
    }
}
