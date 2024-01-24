using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// ���� BGM, SFX ����
/// 
/// ** �� �� �׶� sfxClip�� �߰��� ���, ���� ***�ּ�ó�� �� �� �κп� �߰��ϱ�
/// </summary>
public enum SFX_TYPE { BTN = 0, HIT, FAIL, CARD_DRAW, TurnChange }         // *** sfxClip ���� �߰� �� ������ �ٿ��ֱ�

public class AudioManager : Singleton<AudioManager>
{
    [Header("Background Music")]
    [SerializeField]
    private AudioClip bgmClip;               // �����غ�� BGM ���ҽ�
    private AudioSource bgmSrc;
    private float bgmVolume = 0.5f;
    [Header("Sound Effects")]
    private List<AudioClip[]> SFXlist;                  // *** sfxClip ���� �߰� �� ������ �ٿ��ֱ�
    [SerializeField] private AudioClip[] sfxClip_Btn;
    [SerializeField] private AudioClip[] sfxClip_Hit;
    [SerializeField] private AudioClip[] sfxClip_Fail;
    [SerializeField] private AudioClip[] sfxClip_CardDraw;
    [SerializeField] private AudioClip[] sfxClip_TurnChange;
    private AudioSource[] sfxSrcs;
    private float sfxVolume = 0.5f;
    private const int channels = 10;         // SFX ä�� : ���� ȿ���� ��ĥ �� �����Ƿ� ����ä�η� ����

    protected new void Awake()
    {
        base.Awake();
        InitialSetting();
        PlayBGM();
    }
    private void InitialSetting()               // BGM, SFX �ʱ�ȭ
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

        SFXlist = new List<AudioClip[]>();                          // *** sfxClip ���� �߰� �� ������ �ٿ��ֱ�
        SFXlist.Clear();
        SFXlist.Add(sfxClip_Btn);
        SFXlist.Add(sfxClip_Hit);
        SFXlist.Add(sfxClip_Fail);
        SFXlist.Add(sfxClip_CardDraw);
        SFXlist.Add(sfxClip_TurnChange);

        for (int i = 0; i < sfxSrcs.Length; i++)
        {
            sfxSrcs[i] = sfxObj.AddComponent<AudioSource>();
            sfxSrcs[i].playOnAwake = false;
            sfxSrcs[i].volume = sfxVolume;
            sfxSrcs[i].loop = false;
        }
    }
    public void PlayBGM()           // BGM ���
    {
        if (bgmSrc.isPlaying) return;
        bgmSrc.Play();
    }
    public void StopBGM()           // BGM ����
    {
        bgmSrc.Stop();
    }
    public void PlaySFX(SFX_TYPE _SFX_TYPE)             // ���ϴ� ������ Ŭ���� �� ���� �ϳ��� ����ִ� ä�η� ���
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
}
