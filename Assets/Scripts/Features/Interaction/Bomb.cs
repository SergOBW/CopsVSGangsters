using Abstract.Inventory;
using EnemyCore;
using TMPro;
using UnityEngine;

public class Bomb : MonoBehaviour
{
    [SerializeField] private float timerBeforeBoom = 5;
    private float _timer;
    
    [SerializeField] private AudioSource audioSource;

    [SerializeField] private AudioClip bombIn;
    [SerializeField] private AudioClip bombLoop;
    [SerializeField] private AudioClip boomSound;
    
    
    [SerializeField] private TMP_Text _text;
    private BoomDoor _boomDoor;

    private bool _isUsed;
    private bool _hasBooster;
    public void Setup(BoomDoor boomDoor)
    {
        _boomDoor = boomDoor;
        _timer = 0;
        _isUsed = true;
        _hasBooster = Inventory.Instance.HasItem("Great Bomb");
        audioSource.clip = bombIn;
        audioSource.loop = true;
        audioSource.Play();
    }

    private void Update()
    {
        if (!_isUsed)
        {
            return;
        }
        if (_timer < timerBeforeBoom)
        {
            if (_hasBooster)
            {
                _timer += Time.deltaTime * 2;
            }
            else
            {
                _timer += Time.deltaTime;
            }

            if (_timer >= timerBeforeBoom - bombIn.length && audioSource.clip != bombLoop)
            {
                audioSource.clip = bombLoop;
                audioSource.Play();
            }
        }
        else
        {
            GameObject bombSound =  Instantiate(new GameObject("BombAudio"));
            AudioSource bombSoundAudioSource = bombSound.AddComponent<AudioSource>();
            bombSoundAudioSource.outputAudioMixerGroup = audioSource.outputAudioMixerGroup;
            bombSoundAudioSource.volume = audioSource.volume;
            bombSoundAudioSource.PlayOneShot(boomSound);
            _boomDoor.Boom();
            return;
        }
        
        _boomDoor.HandleAllBombs(Mathf.Abs(_timer - timerBeforeBoom));
        
    }

    public void HandleTime(float time)
    {
        int timer = (int)time;
        _text.text = timer.ToString();
    }
}
