using EnemyCore;
using UnityEngine;

public class EnemySoundContainer
{
    public AudioClip[] dieClip;
    public AudioClip[] longRangeAttack;
    public AudioClip[] shortAtackClip;
    public AudioClip[] woundringClip;
    public AudioClip[] hitClips;

    public EnemySoundContainer(EnemySoundSo enemySoundSo)
    {
        dieClip = enemySoundSo.dieClip;
        longRangeAttack = enemySoundSo.longRangeAttack;
        shortAtackClip = enemySoundSo.shortAtackClip;
        woundringClip = enemySoundSo.woundringClip;
        hitClips = enemySoundSo.hitClips;
    }
}

public class EnemySoundManager : MonoBehaviour
{
    private AudioClip[] dieClip;
    private AudioClip[] longRangeAttack;
    private AudioClip[] shortAtackClip;
    private AudioClip[] woundringClip;
    private AudioClip[] hitClips;
    private AudioSource audioSource;

    private EnemySoundContainer enemySoundContainer;

    public void Initialize(EnemySoundSo enemySoundSo)
    {
        enemySoundContainer = new EnemySoundContainer(enemySoundSo);
        
        audioSource = gameObject.AddComponent<AudioSource>();
        
        dieClip = enemySoundContainer.dieClip;
        longRangeAttack = enemySoundContainer.longRangeAttack;
        shortAtackClip = enemySoundContainer.shortAtackClip;
        woundringClip = enemySoundContainer.woundringClip;
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
    
    public void PlayShortAttack()
    {
        Stop();
        if (CanPlay() && shortAtackClip.Length > 0)
        {
            var newClip = shortAtackClip[Random.Range(0, shortAtackClip.Length)];
            audioSource.clip = newClip;
            audioSource.Play();
        }
    }
    
    public void PlayLongAttack()
    {
        if (CanPlay() && longRangeAttack.Length > 0)
        {
            Stop();
            audioSource.clip = longRangeAttack[Random.Range(0,longRangeAttack.Length)];
            audioSource.Play();
        }
    }
    
    public void PlayWounder()
    {
        if (CanPlay() && woundringClip.Length >0)
        {
            Stop();
            audioSource.clip = woundringClip[Random.Range(0,woundringClip.Length)];
            audioSource.Play();
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
