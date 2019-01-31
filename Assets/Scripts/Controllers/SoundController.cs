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
    private AudioSource audioSourceBgMusic;


    FMOD.Studio.EventInstance music;

    public FMOD.Studio.EventInstance snare;
    public FMOD.ChannelGroup channelGroup;
    public ulong dsp;
    public float dsptime;
    public ulong dsp2;
    FMOD.SPEAKERMODE sPEAKERMODE;
    int rawspeaker;

    public int samplerate;
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
        audioSourceEffect = audioSources[0];
        audioSourceBgMusic = audioSources[1];


        //存放到字典

        foreach (AudioClip item in audioArray)
        {
            _soundDictionary.Add(item.name, item);
        }
//        UnityEngine.Debug.Log(FMODUnity.RuntimeManager.LowlevelSystem.setDSPBufferSize(64, 2));

        snare = FMODUnity.RuntimeManager.CreateInstance("event:/music/testlevel");
        snare.getChannelGroup(out channelGroup);
        samplerate = 48000;


        FMODUnity.RuntimeManager.LowlevelSystem.getSoftwareFormat(out samplerate,out sPEAKERMODE,out rawspeaker);
    }

    //播放音效
    public void PlayAudioEffect(string audioEffectName)
    {
        if (_soundDictionary.ContainsKey(audioEffectName))
        {
            audioSourceEffect.clip = _soundDictionary[audioEffectName];
            audioSourceEffect.Play();
//            UnityEngine.Debug.Log("Unity playtime=" + Time.time);

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

    public void PlayOneShot(string path)
    {
             //   snare.start();

        FMODUnity.RuntimeManager.PlayOneShot(path,this.transform.position);
        UnityEngine.Debug.Log("fmod playtime="+Time.time);
    }

    public void testplay()
    {
           snare.start();
        
            snare.getChannelGroup(out channelGroup);
        channelGroup.getDSPClock(out dsp, out dsp2);
        RhythmController.Instance.songPosOffset -= CalcDSPtime();
    }

    public float CalcDSPtime()
    {

        SoundController.Instance.channelGroup.getDSPClock(out dsp, out dsp2);

        dsptime = (float)dsp / (float)samplerate;
        return dsptime;
    }
}
