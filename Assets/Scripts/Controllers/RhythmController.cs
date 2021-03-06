﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;

public class RhythmController : MonoBehaviour
{

    //歌曲开始时间(用来处理当前节奏)
    [HideInInspector] public float songStartTime;
    //每一拍长度
    [HideInInspector] public float secPerBeat;

    //歌曲当前时间
    [HideInInspector] public float songPos;
    //歌曲当前到第几拍
    [HideInInspector] public float songPosInBeats;

    //当前小节的输入队列是否已经清除过队列
    public bool isCurBarCleaned = false;
    //当前小节进入最后一拍，且已清理之前输入的的锁
    public bool isCurBarAtFinalBeat = false;
    //当前小节是否已经处理过输入超时
    public bool isInputMissed=false;


    //判定阈值 一个比一个大
    public float commentCoolTime;
    public float commentGoodTime;
    //输入错误锁定时间
    public float badDelayTime;
    public float badTime;



    //BPM
    public int mBpm = 70;

    //拍子锁(保证每拍只执行一次操作)
    public int beatIndex = 0;
    public int decayIndex = 0;

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
        badTime = -1;
        //SoundController.Instance.FMODmusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        //此处有个坑，FMODInstance必须创建完成才能获取channelgroup
        //SoundController.Instance.FMODMusicChange(SuperController.Instance.levelData.BGMPath);

        //yield return new WaitForSeconds(2.0f);
        UIBarController.Instance.InitController();

        BpmCalc();

        songStartTime = (float)(SoundController.Instance.CalcDSPtime());
        SoundController.Instance.FMODMusicPlay();
        //        Debug.Log("start time =" + songStartTime+"   fmod time ="+ SoundController.Instance.CalcDSPtime());
    }
    #endregion

    #region 计算输入判定
    public static int InputComment(List<Note> notes)
    {
        //0:cool
        //1:good
        //2:bad
        //3:无效

        if (notes.Count == 0)
        {
            return 3;
        }

  //      Debug.Log("songPosInBeats:" + RhythmController.Instance.songPosInBeats);
//        Debug.Log("beatInSong:" + notes[0].beatInSong);
        float inputError = Mathf.Abs(RhythmController.Instance.songPosInBeats - notes[0].beatInSong);

        if (inputError <= RhythmController.Instance.commentCoolTime)
        {
            return 0;
        }
        else if (inputError <= RhythmController.Instance.commentGoodTime)
        {
            return 1;
        }
        else
        {
            return 2;
        }
    }
    #endregion

    #region 更新节拍
    private void BeatUpdate()
    {
                //        UnityEngine.Debug.Log("dsptime="+dsptime);
        //获得当前歌曲位置
        songPos = SoundController.Instance.CalcDSPtime() - songStartTime + songPosOffset;
        //计算出当前在哪一拍
        songPosInBeats = songPos / secPerBeat;
//             Debug.Log("distime="+ AudioSettings.dspTime+ "    startTime=" + songStartTime+ "    songPos=" +songPos+   "beats ="+songPosInBeats);



        //判定是否播放第N拍 pos 和 fmod同时判定
        if ((beatIndex != (int)UIBarController.Instance.playingBarPosInBeats+1)&& (beatIndex != SoundController.Instance.timelineInfo.currentMusicBeat))
        {
            //            Debug.Log("beat"+(SoundController.Instance.timelineInfo.currentMusicBeat - 1));

            OnBeat((int)UIBarController.Instance.playingBarPosInBeats);
            beatIndex = (int)UIBarController.Instance.playingBarPosInBeats + 1;
        }

        //时间超过判定点，此时BUFF持续时间衰减
        if (songPosInBeats>UIBarController.Instance.finishedBeats + beatIndex - 1+commentGoodTime && decayIndex!=beatIndex)
        {
            Player.Instance.BuffsDecay(beatIndex-1);
            if (Player.Instance.mTarget != null)
                Player.Instance.mTarget.GetComponent<AI>().BuffsDecay(beatIndex - 1);
            decayIndex = beatIndex;
//            Debug.Log("decay");
        }


        //滤掉已经过期的能量音符
        if (UIBarController.Instance.currentEnergyNotes.Count > 0)
        {
            if (songPosInBeats - UIBarController.Instance.currentEnergyNotes[0].beatInSong > commentGoodTime)
            {
                //Debug.Log("songPosInBeat:" + songPosInBeats + "note[0].beat:" + UIBarController.Instance.currentEnergyNotes[0].beatInSong);
                //Debug.Log("delete a energy note:"+ UIBarController.Instance.currentEnergyNotes[0].beatInSong);
                UIBarController.Instance.currentEnergyNotes.RemoveAt(0);
                foreach (var buff in Player.Instance.buffs)
                {
                    buff.MissEnergy();
                }
            }
        }


        //清除已经过期的QTE音符
        if (UIBarController.Instance.currentQTENotes.Count > 0)
        {
            if (songPosInBeats - UIBarController.Instance.currentQTENotes[0].beatInSong > commentGoodTime)
            {
                Player.Instance.enemyList[0].GetComponent<AI>().QTEAction(UIBarController.Instance.currentQTENotes[0].MissSkill);
                SuperController.Instance.ShowInputTip("MISS");
                //Debug.Log("songPosInBeat:" + songPosInBeats + "note[0].beat:" + UIBarController.Instance.currentEnergyNotes[0].beatInSong);
                //Debug.Log("delete a energy note:"+ UIBarController.Instance.currentEnergyNotes[0].beatInSong);
                UIBarController.Instance.currentQTENotes[0].note.GetComponent<VFX>().StartCoroutine("NoteInputBad");

                UIBarController.Instance.currentQTENotes.RemoveAt(0);
            }
        }



        //第三拍之后 清除已经过期的输入音符，AI发招，发招状态重置

        //if (songPosInBeats - UIBarController.Instance.finishedBeats - 2 > commentGoodTime)
        if ((UIBarController.Instance.playingBarPosInBeats - 2f > commentGoodTime)&& isInputMissed == false)
        {
//            Debug.Log("bar pos = " + UIBarController.Instance.playingBarPosInBeats);

            if (isCurBarCleaned == false && isCurBarAtFinalBeat == false)
            {
                if (InputSequenceController.Instance.CurInputSequence.Count != 0)
                {
                    //Debug.Log("输入超时BAD");
                    InputSequenceController.Instance.Bad("BAD");

                }
                isCurBarAtFinalBeat = true;

            }
            if (DuelController.Instance.isActedAt3rdBeat == false)
            {
                if (DuelController.Instance.GetCurAI())
                {
                    Player.Instance.UpdateCDs();

                    //Debug.Log("第三拍之后的发招" + "空");
                    if (DuelController.Instance.GetCurAI())
                    {
                        if (DuelController.Instance.GetCurAI().HasBuff("stunned"))
                        {
                            Debug.Log("晕菜");

                        }
                        else
                        {
                            DuelController.Instance.SkillJudge("", DuelController.Instance.GetCurAI().GetNextSkill(3));
                            DuelController.Instance.GetCurAI().Action(3);
                        }
                    }


                }
            }

            isCurBarCleaned = false;
            isInputMissed = true;

        }


        //判断锁定是否结束
        if (songPosInBeats-badDelayTime>badTime)
        {
            UnlockPin();
        }


        //判断QTE是否结束
        QTEEnd();
    }


    #endregion





    // Use this for initialization
    void Start()
    {
        //调试用 暂时放这
        //  Reset();
    }

    private void FixedUpdate()
    {
        if (SuperController.Instance.state != GameState.Start && SuperController.Instance.state != GameState.QTE && SuperController.Instance.state != GameState.Ulti)
        {
            return;
        }
        BeatUpdate();


        //  ReplayBGM();

    }


    // Update is called once per frame





    #region 普通节拍触发事件OnNormalBeat
    public void OnBeat(int beatNum)
    {



        AI nowAI = DuelController.Instance.GetCurAI();



        if (Player.Instance.animator.GetCurrentAnimatorStateInfo(0).IsName("player-idle"))
            Player.Instance.animator.Play("idlebeat", 0, 0);

        if (Player.Instance.mTarget != null)
        {
            if(nowAI.animator.GetCurrentAnimatorStateInfo(0).IsName("idle")|| nowAI.animator.GetCurrentAnimatorStateInfo(0).IsName("form1-idle")|| nowAI.animator.GetCurrentAnimatorStateInfo(0).IsName("form2-idle"))
                nowAI.animator.SetTrigger("idlebeat");
        }


        //everybody beat!
        if (beatNum == 0)
        {
            //重置发招状态为未发招
            DuelController.Instance.isActedAt3rdBeat = false;
            isInputMissed = false;


            if (nowAI)
            {
                if (nowAI.HasBuff("stunned"))
                {
                    Debug.Log("晕菜");
                }
                else
                {
                    nowAI.Action(1);
                }
            }



            if (Player.Instance.automode) Player.Instance.CastAutoSkill();

        }

        if (beatNum == 1)
        {

            if (nowAI)
            {
                if (nowAI.HasBuff("stunned"))
                {
                    Debug.Log("晕菜");
                }
                else
                {
                    nowAI.Action(2);
                }
            }
            if (Player.Instance.automode) Player.Instance.CastAutoSkill();


        }

        //已经BAD的情况，直接触发AI技能，auto状态 直接触发节能

        if (beatNum == 2)
        {
            if (Player.Instance.automode&&DuelController.Instance.isActedAt3rdBeat==false)
            {




                if (nowAI)
                {
                    if (nowAI.HasBuff("stunned"))
                    {
                        Debug.Log("晕菜");
                        DuelController.Instance.SkillJudge(Player.Instance.autoSkills.Dequeue(), "");

                    }
                    else
                    {
                        DuelController.Instance.SkillJudge(Player.Instance.autoSkills.Dequeue(), DuelController.Instance.GetCurAI().GetNextSkill(3));

                        nowAI.Action(3);
                    }
                }

                if (Player.Instance.autoSkills.Count == 0)
                {
                    Player.Instance.LeaveAutoMode();
                }
            }

            if (isCurBarCleaned == true&&DuelController.Instance.isActedAt3rdBeat==false&& DuelController.Instance.GetCurAI())
            {
                Player.Instance.UpdateCDs();

                //Debug.Log("BAD之后正点AI出招" + "空");

                if (nowAI)
                {
                    if (nowAI.HasBuff("stunned"))
                    {
                        Debug.Log("晕菜");
                    }
                    else
                    {
                        DuelController.Instance.SkillJudge("", DuelController.Instance.GetCurAI().GetNextSkill(3));
                        DuelController.Instance.GetCurAI().Action(3);
                    }
                }

               //Debug.Log("INPUT BAD ,ENEMY ACT AT TIME");
            }

        }

        if (beatNum == 3)
        {
            if(SuperController.Instance.state == GameState.Start)
            {
                DuelController.Instance.EnemyRespawn();
            }


            if (nowAI)
            {
                if (nowAI.HasBuff("stunned"))
                {
                    Debug.Log("晕菜");
                }
                else
                {
                    nowAI.Action(4);
                }
            }
            if (Player.Instance.automode) Player.Instance.CastAutoSkill();


        }


        //BUFF跳 TODO:时序问题？？？
        Player.Instance.BuffsBeat(beatNum);
        if (Player.Instance.mTarget != null)
            Player.Instance.mTarget.GetComponent<AI>().BuffsBeat(beatNum);




        //所有需要跟节奏闪的东西
        SuperController.Instance.Blink();

    }
    #endregion




    //交换小节时触发
    public void NewBarInit()
    {
        //小节开始
        isCurBarAtFinalBeat = false;
    }

    //test
    private void OnGUI()
    {
        //UnityEngine.Debug.Log("flag="+ (string)timelineInfo.lastMarker);
        Text text = GameObject.Find("songposmonitor").GetComponent<Text>();
        text.text = "QTE notes count: " + UIBarController.Instance.currentQTENotes.Count;
    }


    //进入QTE状态
    public void QTEStart(OneSongScore qtescore)
    {
        InputSequenceController.Instance.CleanInputSequence();

        UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.playingBar.GetComponent<UIBar>(), qtescore.QTEscore[0].notes);
        UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.preBar.GetComponent<UIBar>(), qtescore.QTEscore[1].notes);
        SuperController.Instance.state = GameState.QTE;
        UIBarController.Instance.QTEscore = qtescore;
        UIBarController.Instance.QTEbarIndex = 1;
    }

    //进入主角大招QTE状态
    public void UltiQTEStart(OneSongScore qtescore)
    {
        InputSequenceController.Instance.ClnInpSeqWhenCastSkill();

        ///UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.playingBar.GetComponent<UIBar>(), qtescore.QTEscore[0].notes);
        UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.preBar.GetComponent<UIBar>(), qtescore.QTEscore[0].notes);
        SuperController.Instance.state = GameState.Ulti;
        UIBarController.Instance.QTEscore = qtescore;
        UIBarController.Instance.QTEbarIndex = 0;
    }

    //退出各种QTE状态
    public void QTEEnd()
    {
        if(SuperController.Instance.state==GameState.QTE|| SuperController.Instance.state ==GameState.Ulti)
        if (UIBarController.Instance.QTEbarIndex < 0 && UIBarController.Instance.playingBarPosInBeats>=4-commentGoodTime)
        {
            //           Debug.Log("back to start");
            SuperController.Instance.state = GameState.Start;

        }
    }
    #region 错误导致的锁定输入
    public void LockPin()
    {
        badTime = songPosInBeats;
        UIBarController.Instance.preBar.GetComponent<UIBar>().pin.GetComponent<UIBarPin>().SetSprite(1);
        UIBarController.Instance.postBar.GetComponent<UIBar>().pin.GetComponent<UIBarPin>().SetSprite(1);
        UIBarController.Instance.playingBar.GetComponent<UIBar>().pin.GetComponent<UIBarPin>().SetSprite(1);
        //if (m_Eff) m_Eff.isShake = true;
        //Camera.main.gameObject.transform.DOShakePosition(1,0.5f);
        SuperController.Instance.ShowInputTip("BAD");
    }
    #endregion

    #region 错误导致的锁定输入,自然解锁
    public void UnlockPin()
    {
        badTime = -1;
        UIBarController.Instance.preBar.GetComponent<UIBar>().pin.GetComponent<UIBarPin>().SetSprite(0);
        UIBarController.Instance.postBar.GetComponent<UIBar>().pin.GetComponent<UIBarPin>().SetSprite(0);
        UIBarController.Instance.playingBar.GetComponent<UIBar>().pin.GetComponent<UIBarPin>().SetSprite(0);
    }
    #endregion
}
