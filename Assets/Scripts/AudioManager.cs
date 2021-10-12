using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    #region Singleton
    public static AudioManager instance;

    void Awake()
    {
        if (instance != null)
            return;
        instance = this;
    }
    #endregion
    
    private AudioSource audio;
    public AudioSource bgAudio;
    public AudioClip spawnEnemyClip;
    public AudioClip spawnBombClip;
    public AudioClip boomClip;
    public AudioClip backgroundMusicClip;
    public AudioClip eatingPhaseClip;
    public AudioClip fleeingPhaseClip;
    public AudioClip playerDeathClip;
    public AudioClip enemyDeathClip;
    void Start()
    {
        audio = GetComponent<AudioSource>();
    }
    public void PlaySpawnEnemySFX()
    {
        audio.PlayOneShot(spawnEnemyClip);
    }

    public void PlaySpawnBombSFX()
    {
        audio.PlayOneShot(spawnBombClip);
    }

    public void PlayBoomSFX()
    {
        audio.PlayOneShot(boomClip);
    }

    public void PlayBackgroundMusic()
    {
        bgAudio.clip =backgroundMusicClip;
        bgAudio.Play();
    }

    public void PlayEatingPhaseClipSFX()
    {
        audio.PlayOneShot(eatingPhaseClip);
    }

    public void PlayFleeingPhaseClipSFX()
    {
        audio.PlayOneShot(fleeingPhaseClip);
    }
    
    public void PlayPlayerDeathSFX()
    {
        audio.PlayOneShot(playerDeathClip);
    }

    public void PlayEnemyDeathSFX()
    {
        audio.PlayOneShot(enemyDeathClip);
    }
}
