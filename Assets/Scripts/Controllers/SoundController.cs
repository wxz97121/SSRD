using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;

public class SoundController : MonoBehaviour {
    static SoundController _instance;

    private Dictionary<string, AudioClip> _soundDictionary;
    private AudioSource[] audioSources;
    private AudioSource audioSourceEffect;
    private AudioSource audioSourceEffect_M;

    private AudioSource audioSourceEffect_X;
    private AudioSource audioSourceEffect_Z;
    private AudioSource audioSourceBgMusic;


    public FMOD.Studio.EventInstance FMODmusic;
    public ChannelGroup channelGroup;
    private ulong dsp;
    private float dsptime;
    private ulong dsp2;
    FMOD.SPEAKERMODE sPEAKERMODE;
    int rawspeaker;

    private int samplerate;
    public static SoundController Instance
    {
        get
        {
            return _instance;
        }
    }


    private void Awake()
    {
        _instance = this;

        //本地加载 
        _soundDictionary = new Dictionary<string, AudioClip>();
        AudioClip[] audioArray = Resources.LoadAll<AudioClip>("Audio/SE");
        audioSources = GetComponents<AudioSource>();
        audioSourceEffect = audioSources[1];

        audioSourceBgMusic = audioSources[0];

        audioSourceEffect_Z = audioSources[2];
        audioSourceEffect_X = audioSources[3];
        audioSourceEffect_M = audioSources[4];

        //存放到字典

        foreach (AudioClip item in audioArray)
        {
            _soundDictionary.Add(item.name, item);
        }

        audioSourceEffect_Z.clip=_soundDictionary["KICK"];
        audioSourceEffect_X.clip = _soundDictionary["SNARE"];
        audioSourceEffect_M.clip = _soundDictionary["HIHAT"];


        //fmod init
        samplerate = 48000;


        RuntimeManager.LowlevelSystem.getSoftwareFormat(out samplerate,out sPEAKERMODE,out rawspeaker);
    }

    //播放音效
    public void PlayAudioEffect(string audioEffectName)
    {
        switch (audioEffectName)
        {
            case "SNARE":
                audioSourceEffect_X.Stop();

                audioSourceEffect_X.Play();
                break;
            case "KICK":
                audioSourceEffect_Z.Stop();

                audioSourceEffect_Z.Play();
                break;
            case "HIHAT":
                audioSourceEffect_M.Stop();

                audioSourceEffect_M.Play();
                break;
            default:
                if (_soundDictionary.ContainsKey(audioEffectName))
                {
                    audioSourceEffect.clip = _soundDictionary[audioEffectName];
                    audioSourceEffect.Play();
                }
                break;
        }


    }


    //播放音乐
    public void PlayBgMusic(AudioClip audioClip)
    {
        audioSourceBgMusic.clip = audioClip;
        audioSourceBgMusic.loop = true;
        audioSourceBgMusic.Stop();

        audioSourceBgMusic.Play();
        //audioSourceBgMusic.PlayScheduled(0);
    }
    //播放音乐
    public void SetBGMTime(float time)
    {
//        Debug.Log("secPerBeat=" + RhythmController.Instance.secPerBeat);
        if (time < 0)
            time = 0;
        time = time % (RhythmController.Instance.secPerBeat*128);
        audioSourceBgMusic.time=time;
        //audioSourceBgMusic.Play();
        //audioSourceBgMusic.PlayScheduled(0);
    }

    public void FMODPlayOneShot(string path)
    {
        FMODUnity.RuntimeManager.PlayOneShot(path,this.transform.position);
        UnityEngine.Debug.Log("fmod playtime="+Time.time);
    }


    public void FMODMusicChange(string path)
    {
        FMODmusic = FMODUnity.RuntimeManager.CreateInstance(path);
        //TODO:GET PARAMETERS
    }


    //播放FMOD音乐
    public void FMODMusicPlay()
    {
        FMODmusic.getChannelGroup(out channelGroup);

        FMODmusic.start();
        channelGroup.getDSPClock(out dsp, out dsp2);
        UnityEngine.Debug.Log("dsp "+dsp);
        RhythmController.Instance.songPosOffset -= CalcDSPtime();
    }

    //修改FMOD参数
    public void FMODSetParameter(string paraname,float value)
    {
        FMODmusic.setParameterValue(paraname, value);
    }

    public float CalcDSPtime()
    {

        channelGroup.getDSPClock(out dsp, out dsp2);
//        UnityEngine.Debug.Log("dsp " + dsp);

        dsptime = (float)dsp / (float)samplerate;
        return dsptime;
    }
}
