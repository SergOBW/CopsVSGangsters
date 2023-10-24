using Abstract;
using UnityEngine;
using UnityEngine.Audio;
using Yandex.Plugins.Login;

public class SoundMonoMechanic : GlobalMonoMechanic
{
    public static SoundMonoMechanic Instance;
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip hitClip;
    [SerializeField] private AudioClip waveSound;
    [SerializeField] private AudioClip buyClip;

    [SerializeField] private AudioMixer mainMixer;
    [SerializeField] private AudioMixerGroup mainOutputAudioMixerGroup;
    private const string MASTER_VOLUME_NAME = "MasterVolume";
    private float volume;

    public override void Initialize()
    {
        Instance = this;
        SetupVolume();
    }

    public void SetupVolume()
    {
        volume = SaveGameMechanic.Instance.GetSoundValue();
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

    public bool IsSoundOn()
    {
        Debug.Log(volume);
        Debug.Log(volume >= 0.99);
        return volume >= 0.99;
    }

    public void PlayHit()
    {
        _audioSource.volume = 0.5f;
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(hitClip);
        }
        else
        {
            _audioSource.PlayOneShot(hitClip);
        }
    }

    public void PlayBuy()
    {
        _audioSource.volume = 1;
        _audioSource.PlayOneShot(buyClip);
    }

    public AudioMixer GetCurrentMixer()
    {
        return mainMixer;
    }

    public AudioMixerGroup GetCurrentOutputMixer()
    {
        return mainOutputAudioMixerGroup;
    }

    public void PlayWaveSpawn()
    {
        _audioSource.volume = 0.5f;
        if (_audioSource.isPlaying)
        {
            _audioSource.Stop();
            _audioSource.PlayOneShot(waveSound);
        }
        else
        {
            _audioSource.PlayOneShot(waveSound);
        }
    }
}
