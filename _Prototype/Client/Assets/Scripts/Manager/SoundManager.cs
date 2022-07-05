using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    public static SoundManager instance;

    //[SerializeField]
    //private AudioSource bgmSource;

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(this.gameObject);
            return;
        }
        DontDestroyOnLoad(this.gameObject);
        instance = this;
    }
    private void Start()
    {
        
    }
    //public void PlayBgmSound(AudioClip clip, float volume = 0.3f)
    //{
    //    if (bgmSource.isPlaying) bgmSource.Stop();
    //    bgmSource.clip = clip;
    //    bgmSource.volume = volume;
    //    bgmSource.Play();
    //}

    //public void ChangeBgmSound(float volume = 0.3f)
    //{
    //    bgmSource.volume = volume;
    //}

    public void PlaySfxSound(Player p,AudioClip clip, float volume)
    {
        p.AudioSource.PlayOneShot(clip, volume);
    }

    public void ChangeSfxSound(float volume)
    {
        NetworkManager.instance.User.AudioSource.volume = volume;

        foreach (Player p in NetworkManager.instance.GetPlayerList())
        {
            p.AudioSource.volume = volume;
        }
    }
}
