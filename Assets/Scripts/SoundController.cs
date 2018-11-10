using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour {
    static SoundController _instance;

    private Dictionary<string, AudioClip> _soundDictionary;
    private AudioSource[] audioSources;
    private AudioSource audioSourceEffect;



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

        //存放到字典

        foreach (AudioClip item in audioArray)
        {
            _soundDictionary.Add(item.name, item);
        }

    }

    //播放音效
    public void PlayAudioEffect(string audioEffectName)
    {
        if (_soundDictionary.ContainsKey(audioEffectName))
        {
            //audioSourceEffect.clip = _soundDictionary[audioEffectName];
            //audioSourceEffect.Play();
            audioSourceEffect.PlayOneShot(_soundDictionary[audioEffectName],1f);
        }
    }
}
