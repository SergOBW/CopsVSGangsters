using EnemyCore;
using UnityEngine;

public class EnemySoundContainer
{
    public AudioClip[] dieClip;
    public AudioClip[] defaultAttackClips;
    public AudioClip[] hitClips;

    public EnemySoundContainer(EnemySoundSo enemySoundSo)
    {
        dieClip = enemySoundSo.dieClip;
        defaultAttackClips = enemySoundSo.longRangeAttack;
        hitClips = enemySoundSo.hitClips;
    }
}

public class EnemySoundManager : MonoBehaviour
{
    private AudioClip[] dieClip;
    private AudioClip attackClip;
    private AudioClip[] hitClips;
    private AudioSource audioSource;

    private EnemySoundContainer enemySoundContainer;

    public void Initialize(EnemySoundSo enemySoundSo)
    {
        enemySoundContainer = new EnemySoundContainer(enemySoundSo);
        
        audioSource = gameObject.AddComponent<AudioSource>();
        
        dieClip = enemySoundContainer.dieClip;
        attackClip = enemySoundContainer.defaultAttackClips[Random.Range(0,enemySoundContainer.defaultAttackClips.Length)];
        hitClips = enemySoundContainer.hitClips;
    }
    
    public void Initialize(EnemySoundSo enemySoundSo, AudioClip[] attackClips)
    {
        enemySoundContainer = new EnemySoundContainer(enemySoundSo);
        
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.volume = 1;
        audioSource.spatialBlend = 1;
        audioSource.maxDistance = 50;
        audioSource.outputAudioMixerGroup = SoundMonoMechanic.Instance.GetAudioMixerGroup();
        
        dieClip = enemySoundContainer.dieClip;
        attackClip = attackClips[Random.Range(0,attackClips.Length)];
        hitClips = enemySoundContainer.hitClips;
    }
    
    public void PlayDieSound()
    {
        if (CanPlay() && dieClip.Length > 0)
        {
            Stop();
            audioSource.clip = dieClip[Random.Range(0,dieClip.Length)];
            audioSource.Play();
        }
    }
    
    public void PlayAttack()
    {
        if (CanPlay() && attackClip!= null)
        {
            if (audioSource.isPlaying)
            {
                Stop();
                audioSource.clip = attackClip;
                audioSource.Play();
            }
            else
            {
                audioSource.clip = attackClip;
                audioSource.Play();
            }
        }
    }

    private void Stop()
    {
        if (audioSource.clip != null)
        {
            audioSource.Stop();
        }
    }

    public void PlayHitSound()
    {
        if (CanPlay() && hitClips.Length > 0)
        {
            Stop();
            audioSource.clip = hitClips[Random.Range(0,hitClips.Length)];
            audioSource.Play();
        }
    }

    private bool CanPlay()
    {
        if (audioSource == null)
        {
            Debug.LogError("There is no audioSourse on enemy");
            return false;
        }

        return true;
    }
    
}
