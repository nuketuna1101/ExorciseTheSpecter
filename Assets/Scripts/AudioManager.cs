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
    //public List<AudioClip> sfxClipList;
    //List<AudioSource> sfxSrcList;
    public AudioClip[] sfxClipList;
    AudioSource[] sfxSrcList;
    private float sfxVolume = 0.5f;

    private const int channels = 5;         // Ã¤³Î °¹¼ö
    int channelIndex;

    private void Awake()
    {
        InitialSetting();
    }

    private void InitialSetting()
    {
        DebugOpt.Log("ÇÏÀÌ·Õ");
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

}
