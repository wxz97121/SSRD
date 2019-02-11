﻿using System.Collections.Generic;
using UnityEngine;
using FMOD;
using FMODUnity;
using System.Runtime.InteropServices;
using UnityEngine.UI;

using System;


public class SoundController : MonoBehaviour {
    static SoundController _instance;

    private Dictionary<string, AudioClip> _soundDictionary;
    private AudioSource[] audioSources;
    private AudioSource audioSourceEffect;
    private AudioSource audioSourceEffect_M;

    private AudioSource audioSourceEffect_X;
    private AudioSource audioSourceEffect_Z;
    private AudioSource audioSourceBgMusic;

    //fmod相关
    public FMOD.Studio.EventInstance FMODmusic;
    public ChannelGroup channelGroup;
    private ulong dsp;
    private float dsptime;
    private ulong dsp2;
    FMOD.SPEAKERMODE sPEAKERMODE;
    int rawspeaker;

    private int samplerate;


    //fmod callback相关

    // Variables that are modified in the callback need to be part of a seperate class.
    // This class needs to be 'blittable' otherwise it can't be pinned in memory.
    [StructLayout(LayoutKind.Sequential)]
    public class TimelineInfo
    {
        public int currentMusicBeat = 0;
        public FMOD.StringWrapper lastMarker = new FMOD.StringWrapper();
    }
    public TimelineInfo timelineInfo;
    GCHandle timelineHandle;

    FMOD.Studio.EVENT_CALLBACK beatCallback;




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


        //fmod callback相关
        timelineInfo = new TimelineInfo();

        // Explicitly create the delegate object and assign it to a member so it doesn't get freed
        // by the garbage collected while it's being used
        beatCallback = new FMOD.Studio.EVENT_CALLBACK(BeatEventCallback);


        // Pin the class that will store the data modified during the callback
        timelineHandle = GCHandle.Alloc(timelineInfo, GCHandleType.Pinned);
        // Pass the object through the userdata of the instance
       
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
        FMODmusic.setUserData(GCHandle.ToIntPtr(timelineHandle));

        FMODmusic.setCallback(beatCallback, FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT | FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER);



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


    [AOT.MonoPInvokeCallback(typeof(FMOD.Studio.EVENT_CALLBACK))]
    static FMOD.RESULT BeatEventCallback(FMOD.Studio.EVENT_CALLBACK_TYPE type, FMOD.Studio.EventInstance instance, IntPtr parameterPtr)
    {
        // Retrieve the user data
        IntPtr timelineInfoPtr;
        FMOD.RESULT result = instance.getUserData(out timelineInfoPtr);
        if (result != FMOD.RESULT.OK)
        {
            UnityEngine.Debug.LogError("Timeline Callback error: " + result);
        }
        else if (timelineInfoPtr != IntPtr.Zero)
        {
            // Get the object to store beat and marker details
            GCHandle timelineHandle = GCHandle.FromIntPtr(timelineInfoPtr);
            TimelineInfo timelineInfo = (TimelineInfo)timelineHandle.Target;

            switch (type)
            {
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_BEAT:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_BEAT_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_BEAT_PROPERTIES));
                        timelineInfo.currentMusicBeat = parameter.beat;
                    }
                    break;
                case FMOD.Studio.EVENT_CALLBACK_TYPE.TIMELINE_MARKER:
                    {
                        var parameter = (FMOD.Studio.TIMELINE_MARKER_PROPERTIES)Marshal.PtrToStructure(parameterPtr, typeof(FMOD.Studio.TIMELINE_MARKER_PROPERTIES));
                        timelineInfo.lastMarker = parameter.name;
                    }
                    break;
            }
        }
        return FMOD.RESULT.OK;
    }

    void OnDestroy()
    {
        FMODmusic.setUserData(IntPtr.Zero);
        FMODmusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        FMODmusic.release();
        timelineHandle.Free();
    }

    //test
    private void OnGUI()
    {
        //UnityEngine.Debug.Log("flag="+ (string)timelineInfo.lastMarker);
        Text text = GameObject.Find("flag").GetComponent<Text>();
        text.text = (string)timelineInfo.lastMarker+timelineInfo.currentMusicBeat;
    }
}
