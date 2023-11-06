using System;
using Abstract;
using Level.States;
using UnityEngine;
using UnityEngine.Audio;

public class SoundMonoMechanic : GlobalMonoMechanic
{
    public static SoundMonoMechanic Instance;
    [SerializeField] private AudioSource _effectsAudioSource;
    [SerializeField] private AudioSource _musicAudioSource;

    [SerializeField] private AudioClip inGameMusicClip;
    
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip waveSound;
    [SerializeField] private AudioClip buyClip;

    [SerializeField] private AudioMixer mainMixer;
    private const string MASTER_VOLUME_NAME = "MasterVolume";
    private float volume;

    public override void Initialize()
    {
        Instance = this;
        LevelStateMachine.Instance.OnStateChangedEvent += OnStateChangedEvent;
        SetupVolume();
    }

    private void OnStateChangedEvent(IState previous, IState current)
    {
        switch (current)
        {
            case LevelMonoPlayState :
                OnLevelLoaded();
                break;
            case LevelMonoEndState :
                OnLevelUnLoaded();
                break;
            case LevelMonoPauseState:
                PauseMusic();
                break;
        }
    }

    private void OnLevelLoaded()
    {
        if (_musicAudioSource.clip != null)
        {
            _musicAudioSource.Play();
            return;
        }
        _musicAudioSource.clip = inGameMusicClip;
        _musicAudioSource.loop = true;
        _musicAudioSource.Play();
    }
    private void OnLevelUnLoaded()
    {
        _musicAudioSource.Stop();
    }

    private void PauseMusic()
    {
        _musicAudioSource.Pause();
    }

    public void SetupVolume()
    {
        volume = SaveGameMechanic.Instance.GetGameSaves().sound;
        Debug.Log("SetupVolume " + volume);
        mainMixer.SetFloat(MASTER_VOLUME_NAME,volume);
    }

    public void DisableSound()
    {
        volume = -300;
        mainMixer.SetFloat(MASTER_VOLUME_NAME,volume);
    }

    public void OnSound()
    {
        Debug.Log("On sound");
        volume = 1;
        mainMixer.SetFloat(MASTER_VOLUME_NAME,volume);
        SaveGameMechanic.Instance.SaveSound(volume);
    }

    public void OffSound()
    {
        Debug.Log("Off sound");
        volume = -300;
        mainMixer.SetFloat(MASTER_VOLUME_NAME,volume);
        SaveGameMechanic.Instance.SaveSound(volume);
    }

    public void PlayHit()
    {
        _effectsAudioSource.volume = 0.5f;
        if (_effectsAudioSource.isPlaying)
        {
            _effectsAudioSource.Stop();
            _effectsAudioSource.PlayOneShot(hitClip);
        }
        else
        {
            _effectsAudioSource.PlayOneShot(hitClip);
        }
    }

    public void PlayBuy()
    {
        _effectsAudioSource.volume = 1;
        _effectsAudioSource.PlayOneShot(buyClip);
    }

    public void PlayWaveSpawn()
    {
        
        _effectsAudioSource.volume = 0.5f;
        if (_effectsAudioSource.isPlaying)
        {
            _effectsAudioSource.Stop();
            _effectsAudioSource.PlayOneShot(waveSound);
        }
        else
        {
            _effectsAudioSource.PlayOneShot(waveSound);
        }
        
    }

    public AudioMixer GetAudioMixer()
    {
        return mainMixer;
    }
}
