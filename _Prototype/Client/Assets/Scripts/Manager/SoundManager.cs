using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundManager : MonoBehaviour
{
    private const string MASTER_NAME = "Master";
    private const string MASTER_VOLUME_NAME = "MasterVolume";
    private const string SFX_VOLUME_NAME = "SFXVolume";
    private const string BGM_VOLUME_NAME = "BGMVolume";

    public static SoundManager Instance;

    [SerializeField]
    private AudioMixer mainMixer;

    private AudioMixerGroup sfxGroup;
    private AudioMixerGroup bgmGroup;

    [SerializeField]
    private AudioSource bgmSource; //배경음 재생기
    [SerializeField]
    private AudioSource footstepSource; //발소리 재생기
    [SerializeField]
    private List<AudioSource> sfxSourceList; //재생기 리스트

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        Instance = this;

        sfxGroup = mainMixer.FindMatchingGroups(MASTER_NAME)[1];
        bgmGroup = mainMixer.FindMatchingGroups(MASTER_NAME)[2];

        sfxSourceList = new List<AudioSource>();
    }

    private void Start()
    {   
        bgmSource = CreateAudioSource();
        bgmSource.outputAudioMixerGroup = bgmGroup;
        bgmSource.loop = true;

        footstepSource = CreateAudioSource();
        footstepSource.loop = true;

        CreateAudioSources(10);
    }

    private void CreateAudioSources(int count)
    {
        for(int i = 0; i < count; i++)
        {
            CreateAudioSource();
        }
    }

    private AudioSource CreateAudioSource()
    {
        GameObject go = new GameObject("Audio Soucre");
        go.transform.parent = this.transform;

        AudioSource audio = go.AddComponent<AudioSource>();
        audio.playOnAwake = false;
        audio.outputAudioMixerGroup = sfxGroup;
        sfxSourceList.Add(audio);

        return audio;
    }
    
    private AudioSource GetEmptyAudioSouce()
    {
        AudioSource audioSource = null;

        for(int i = 0; i < sfxSourceList.Count; i++)
        {
            if(!sfxSourceList[i].isPlaying)
            {
                audioSource = sfxSourceList[i];
            }
        }

        if(audioSource == null)
        {
            audioSource = CreateAudioSource();
        }

        return audioSource;
    }

    public void PlayBGM(AudioClip clip)
    {
        bgmSource.clip = clip;
        bgmSource.Play();
    }
    
    public void StopBGM()
    {
        bgmSource.Stop();
    }

    public void PlayFootStep(AudioClip clip)
    {
        if(footstepSource.clip == clip) return;

        footstepSource.clip = clip;
        footstepSource.Play();
    }   

    public void StopFootStep()
    {
        if(!footstepSource.isPlaying) return;

        footstepSource.Stop();
    }

    public void PlaySFX(AudioClip clip, float playTime = 0f)
    {
        AudioSource audio = GetEmptyAudioSouce();

        audio.clip = clip;
        audio.Play();

        if(playTime > 0f)
        {
            StartCoroutine(SoundStopTimer(audio, playTime));
        }
    }

    public void PlayCharacterSound(AudioClip clip, Player player)
    {
        AudioSource audio = player.AudioSource;

        audio.clip = clip;
        audio.Play();
    }

    private IEnumerator SoundStopTimer(AudioSource audioSource, float playTime)
    {
        yield return new WaitForSeconds(playTime);

        audioSource.Stop();
    }
}
