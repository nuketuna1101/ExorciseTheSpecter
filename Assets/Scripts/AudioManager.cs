using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class AudioManager : Singleton<AudioManager>
{
    [Header("Background Music")]
    public AudioClip bgmClip;
    AudioSource bgmSrc;
    private float bgmVolume = 0.5f;

    [Header("Sound Effects")]
    public AudioClip[] sfxClipList;
    AudioSource[] sfxSrcList;
    private float sfxVolume = 0.5f;

    private const int channels = 5;         // 채널 갯수
    int channelIndex;

    public enum SFX { Test1, Test2 }

    private void Awake()
    {
        InitialSetting();
    }

    private void InitialSetting()
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
        //sfxSrcList = new List<AudioSource>();
        sfxSrcList = new AudioSource[channels];

        for (int i = 0; i < sfxSrcList.Length; i++)
        {
            sfxSrcList[i] = sfxObj.AddComponent<AudioSource>();
            sfxSrcList[i].playOnAwake = false;
            sfxSrcList[i].volume = sfxVolume;
            sfxSrcList[i].loop = true;
        }
    }

    public void PlayBGM()
    {
        bgmSrc.Play();
    }
    public void StopBGM()
    {
        bgmSrc.Stop();
    }
    public void PlaySFX(SFX _SFX)           // enum 타입에 해당하는 효과음 재생
    {
        for (int i = 0; i < sfxSrcList.Length; i++)
        {
            int loopIndex = (channelIndex + i) / sfxSrcList.Length;

            if (sfxSrcList[loopIndex].isPlaying)
                continue;


            // 효과음이 2개 이상인 것은 랜덤으로
            int ranIndex = 0;
            if (_SFX == SFX.Test1)
            {
                ranIndex = Random.Range(0, 3);
            }


            channelIndex = loopIndex;
            sfxSrcList[0].clip = sfxClipList[(int)_SFX + ranIndex];
            sfxSrcList[loopIndex].Play();
            break;
        }
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
