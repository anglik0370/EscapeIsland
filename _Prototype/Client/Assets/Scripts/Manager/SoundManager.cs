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

    private Player p;

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

    [Header("BGM")]
    [SerializeField]
    private AudioClip titleBGM;
    [SerializeField]
    private AudioClip daylightBGM;
    public AudioClip DaylightBGM => daylightBGM;
    [SerializeField]
    private AudioClip nightBGM;
    [SerializeField]
    private AudioClip gameWinBGM;
    [SerializeField]
    private AudioClip gameLoseBGM;

    [Header("SFX")]
    [SerializeField]
    private AudioClip btnClickSfx;

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

        EventManager.SubGameOver(goc => {
            StopAllSFXSound();
            
            if(goc == GameOverCase.BlueWin)
            {
                PlaySFX(p.CurTeam == Team.BLUE ? gameWinBGM : gameLoseBGM);
            }
            else if(goc == GameOverCase.RedWin)
            {
                PlaySFX(p.CurTeam == Team.RED ? gameWinBGM : gameLoseBGM);
            }
        });

        EventManager.SubExitRoom(() => {
            StopAllSFXSound();
            PlayBGM(titleBGM);
        });

        EventManager.SubEnterRoom(p => {
            this.p = p;
            PlayBGM(daylightBGM);
        });

        EventManager.SubGameStart(p => {
            PlayBGM(daylightBGM);
        });

        EventManager.SubTimeChange(isLight => {
            if(isLight)
            {
                PlayBGM(daylightBGM);
            }
            else
            {
                PlayBGM(nightBGM);
            }
        });

        PlayBGM(titleBGM);
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
        if(bgmSource.clip == clip) bgmSource.Stop();

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
        footstepSource.clip = null;
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

    public void PlayBtnSfx()
    {
        PlaySFX(btnClickSfx);
    }

    private void StopAllSFXSound()
    {
        StopFootStep();
        sfxSourceList.ForEach(x => x.Stop());
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

    public void BGMVolumeControl(float value)
    {
        mainMixer.SetFloat(BGM_VOLUME_NAME, value);
    }

    public void SFXVolumeControl(float value)
    {
        mainMixer.SetFloat(SFX_VOLUME_NAME, value);
    }
}
