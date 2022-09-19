using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingPanel : Panel
{
    [SerializeField]
    private Slider sfxSlider;
    [SerializeField]
    private Slider bgmSlider;

    protected override void Awake() {
        sfxSlider.onValueChanged.AddListener(value => SoundManager.Instance.SFXVolumeControl(value));
        bgmSlider.onValueChanged.AddListener(value => SoundManager.Instance.BGMVolumeControl(value));

        base.Awake();
    }

    protected override void Start() {
        SoundManager.Instance.SFXVolumeControl(sfxSlider.value);
        SoundManager.Instance.BGMVolumeControl(bgmSlider.value);
        base.Start();
    }
}
