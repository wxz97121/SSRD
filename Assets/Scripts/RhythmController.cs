using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RhythmController : MonoBehaviour {

    //歌曲开始时间(用来处理当前节奏)
    [HideInInspector] public float songStartTime;
    //每一拍长度
    [HideInInspector] public float secPerBeat;

    //歌曲当前时间
    [HideInInspector] public float songPos;
    //歌曲当前到第几拍
    [HideInInspector] public float songPosInBeats;


    //BPM
    public int mBpm = 70;

    //校正(校准因为视觉产生的节拍误差)
    public float songPosOffset = 0f;

    #region 单例
    static RhythmController _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static RhythmController Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion


    #region 计算bpm
    private void BpmCalc()
    {
        secPerBeat = 60f / mBpm;
    }
    #endregion


    #region 重置节拍条(重新计算bpm,归零，播放歌曲，开始游戏等)
    public void Reset()
    {
        BpmCalc();

        //获取歌曲开始播放的时间点
        songStartTime = (float)AudioSettings.dspTime;


        SoundController.Instance.PlayBgMusic(LevelData.Instance.score.bgmusic);

    }
    #endregion


    //更新节拍
    private void BeatUpdate()
    {
        //获得当前歌曲位置
        songPos = (float)(AudioSettings.dspTime - songStartTime) + songPosOffset;
        //计算出当前在哪一拍
        songPosInBeats = songPos / secPerBeat;
//        Debug.Log("distime="+ AudioSettings.dspTime+ "    startTime=" + songStartTime+ "    songPos=" +songPos+   "beats ="+songPosInBeats);






    }
    // Use this for initialization
    void Start () {
        //调试用 暂时放这
        Reset();
	}
	
	// Update is called once per frame
	void Update () {
        BeatUpdate();
	}
}
