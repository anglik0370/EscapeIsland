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
    private List<AudioSource> audioSourceList; //재생기 리스트

    private Dictionary<int, AudioSource> bgmSourceDic; //배경음 재생기 Dictionary

    private int bgmIdCount = 0; //BGM의 고유 ID

    private void Awake()
    {
        if (Instance != null)
        {
            Destroy(this.gameObject);
            return;
        }

        DontDestroyOnLoad(this.gameObject);
        Instance = this;

        sfxGroup = mainMixer.FindMatchingGroups(MASTER_NAME)[1];
        bgmGroup = mainMixer.FindMatchingGroups(MASTER_NAME)[2];

        audioSourceList = new List<AudioSource>();
    }

    private void Start()
    {
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
        audioSourceList.Add(audio);

        return audio;
    }
    
    private AudioSource GetEmptyAudioSouce()
    {
        AudioSource audioSource = null;

        for(int i = 0; i < audioSourceList.Count; i++)
        {
            if(!audioSourceList[i].isPlaying)
            {
                audioSource = audioSourceList[i];
            }
        }

        if(audioSource == null)
        {
            audioSource = CreateAudioSource();
        }

        return audioSource;
    }

    public int PlayBGM(AudioClip clip)
    {
        AudioSource audio = GetEmptyAudioSouce();

        audio.loop = true;
        audio.clip = clip;
        audio.Play();

        bgmSourceDic.Add(bgmIdCount, audio);

        return bgmIdCount++;
    }

    public void StopBGM(int id)
    {
        AudioSource audio = bgmSourceDic[id];

        audio.Stop();
        audio.loop = false;

        bgmSourceDic.Remove(id);
    }

    public void PlaySFX(AudioClip clip)
    {
        AudioSource audio = GetEmptyAudioSouce();

        audio.clip = clip;
        audio.Play();
    }

    public void PlayCharacterSound(AudioClip clip, Player player)
    {
        AudioSource audio = player.AudioSource;

        audio.clip = clip;
        audio.Play();
    }
}
