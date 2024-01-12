using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 사운드 BGM, SFX 관리
/// </summary>
public enum SFX_TYPE { BTN = 0, HIT }

public class AudioManager : Singleton<AudioManager>
{
    [Header("Background Music")]
    [SerializeField]
    private AudioClip bgmClip;               // 사전준비된 BGM 리소스
    private AudioSource bgmSrc;
    private float bgmVolume = 0.5f;
    [Header("Sound Effects")]
    private List<AudioClip[]> SFXlist;
    [SerializeField]    private AudioClip[] sfxClip_Btn;
    [SerializeField]    private AudioClip[] sfxClip_Hit;
    private AudioSource[] sfxSrcs;
    private float sfxVolume = 0.5f;
    private const int channels = 10;         // SFX 채널 : 여러 효과음 겹칠 수 있으므로 다중채널로 관리

    private new void Awake()
    {
        base.Awake();
        InitialSetting();
    }
    private void InitialSetting()               // BGM, SFX 초기화
    {
        GameObject bgmObj = new GameObject("BGMPlayer");
        bgmObj.transform.parent = transform;
        bgmSrc = bgmObj.AddComponent<AudioSource>();
        bgmSrc.playOnAwake = false;
        bgmSrc.volume = bgmVolume;
        bgmSrc.loop = true;
        bgmSrc.clip = bgmClip;

        GameObject sfxObj = new GameObject("SFXPlayer");
        sfxObj.transform.parent = transform;
        sfxSrcs = new AudioSource[channels];

        SFXlist = new List<AudioClip[]>();
        SFXlist.Clear();
        SFXlist.Add(sfxClip_Btn);
        SFXlist.Add(sfxClip_Hit);

        for (int i = 0; i < sfxSrcs.Length; i++)
        {
            sfxSrcs[i] = sfxObj.AddComponent<AudioSource>();
            sfxSrcs[i].playOnAwake = false;
            sfxSrcs[i].volume = sfxVolume;
            sfxSrcs[i].loop = false;
        }
    }
    public void PlayBGM()           // BGM 재생
    {
        if (bgmSrc.isPlaying) return;
        bgmSrc.Play();
    }
    public void StopBGM()           // BGM 중지
    {
        bgmSrc.Stop();
    }
    public void PlaySFX(SFX_TYPE _SFX_TYPE)             // 원하는 범주의 클립들 중 랜덤 하나를 비어있는 채널로 재생
    {
        var targetClips = SFXlist[(int)_SFX_TYPE];
        int rand = Random.Range(0, targetClips.Length - 1);
        AudioSource availableSfxSrc = null;
        foreach (var sfxSrc in sfxSrcs)
        {
            if (sfxSrc.isPlaying) continue;
            availableSfxSrc = sfxSrc;
            break;
        }
        availableSfxSrc.clip = targetClips[rand];
        availableSfxSrc.Play();
    }



    #region Volume fade in/out Coroutine
    [SerializeField] private AudioSource musicSource;
    IEnumerator VoluemFadeOut(float durationTime)
    {
        float from = musicSource.volume;
        float to = 0;
        float elapsedTime = 0f;
        while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime / durationTime;
            musicSource.volume = Mathf.Lerp(from, to, Mathf.Sin(elapsedTime * Mathf.PI * 0.5f));
            yield return null;
        }
    }

    IEnumerator VolumeFadeIn(float targetValue, float durationTime)
    {
        float from = 0;
        float to = targetValue;
        float elapsedTime = 0f;
        while (elapsedTime <= 1)
        {
            elapsedTime += Time.deltaTime / durationTime;
            musicSource.volume = Mathf.Lerp(from, to, 1f - Mathf.Cos(elapsedTime * Mathf.PI * 0.5f));
            yield return null;
        }
    }
    #endregion
}
