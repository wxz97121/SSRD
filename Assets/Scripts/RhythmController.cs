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

    //判定阈值 一个比一个大
    public float commetCoolTime;
    public float commetGoodTime;
    public float commetMissTime;

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

    #region 计算输入判定
    public static int InputComment(List<Note> notes)
    {
        //0:cool
        //1:good
        //2:bad

        if (notes.Count == 0)
        {
            return 3;
        }


        float mBarPercent = Mathf.Abs(RhythmController.Instance.songPosInBeats - notes[0].beat);

        if (mBarPercent <= 0.1f)
        {
            return 0;
        }
        else if (mBarPercent <= 0.25f)
        {
            return 1;
        }
        else
        {
            return 2;
        }
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


        //todo:增加各种情况的处理，去掉已经滤掉的音符

        //判定是否播放第N拍，并告知各种物体POGO起来！
        //第三拍要特殊处理
        if (UIBarController.Instance.currentEnergyNotes.Count > 0)
        {
            ////拍子正中位置
            //if (songPosInBeats - mRunningNoteList_main[0].beat > 0f)
            //{
            //    //完成这一拍,刷新敌人(如果有敌人死了就算倒计时),下一拍开始
            //    BeatCenter();

            //}

            ////如果超出范围(0.5是表示以节拍中心向后的时间范围)
            //if (songPosInBeats - mRunningNoteList_main[0].beat > 0.5f)
            //{
            //    //完成这一拍,刷新敌人(如果有敌人死了就算倒计时),下一拍开始
            //    BeatEnd();
            //    EnemyUpdate();
            //    BeatStart();
            //}
        }

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

    //TODO：针对音符的输入判定，主要是收集能量和QTE

    //TODO:针对关键节拍的输入判定，主要是蓄力和发招
}
