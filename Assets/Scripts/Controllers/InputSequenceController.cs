﻿using System.Collections.Generic;
using UnityEngine;

public class InputSequenceController : MonoBehaviour
{
    //当前输入状态下，还有可能发出的技能列表
    public List<Skill> availableSkills;
    private float judgeBeat;




    //当前的输入序列
    public List<Note> CurInputSequence;

    #region 单例
    static InputSequenceController _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static InputSequenceController Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    // Start is called before the first frame update
    void Start()
    {
        CurInputSequence = new List<Note>();
    }

    // Update is called once per frame
    void Update()
    {

    }


    #region 吃能量
    public void CollectEnergy()
    {
        if (Player.Instance.hasBuff<Buff_autoenergy>())
        {
            if (RhythmController.InputComment(UIBarController.Instance.currentEnergyNotes) < 2)
            {
                Player.Instance.AddMp(1);
                SoundController.Instance.PlayAudioEffect("Hihat");

                UIBarController.Instance.currentEnergyNotes[0].note.GetComponent<VFX>().StartCoroutine("FadeOutLarger");
                UIBarController.Instance.currentEnergyNotes.RemoveAt(0);

                foreach (var buff in Player.Instance.buffs)
                {
                    buff.GainEnergy();
                }
            }
            return;
        }
        //Debug.Log("------------------------");

        //Debug.Log("START TRYING COLLECT");
        //Debug.Log("currentEnergyNotes.Count:" + UIBarController.Instance.currentEnergyNotes.Count);
        //Debug.Log("comment value:" + RhythmController.InputComment(UIBarController.Instance.currentEnergyNotes));
        if (RhythmController.Instance.badTime >= 0)
        {
            return;
        }

        if (RhythmController.InputComment(UIBarController.Instance.currentEnergyNotes) < 2)
        {
            Player.Instance.AddMp(1);
            SoundController.Instance.PlayAudioEffect("Hihat");

            UIBarController.Instance.currentEnergyNotes[0].note.GetComponent<VFX>().StartCoroutine("FadeOutLarger");
            UIBarController.Instance.currentEnergyNotes.RemoveAt(0);

            foreach (var buff in Player.Instance.buffs)
            {
                buff.GainEnergy();
            }
            //      Debug.Log("2");
        }
        else
        {
            RhythmController.Instance.LockPin();
        }
        //


    }
    #endregion

    #region 处理输入的音符
    public void CalcSkillInput(Note.NoteType inputType)
    {
        //        Debug.Log("UIBarController.Instance.playingBarPosInBeats!" + UIBarController.Instance.playingBarPosInBeats);
        //        Debug.Log("RhythmController.Instance.commentGoodTime!" + RhythmController.Instance.commentGoodTime);

        //锁定状态直接RETURN
        if (RhythmController.Instance.badTime >= 0)
        {
            return;
        }

        //自动状态直接RETURN
        if (Player.Instance.automode)
        {
            return;
        }

        //第三拍之后,改为不处理,仅处理下一小节第一拍之前的一小段时间
        if ((UIBarController.Instance.playingBarPosInBeats > -RhythmController.Instance.commentGoodTime + 4f))
        {
            //return;
            judgeBeat = UIBarController.Instance.preBarPosInBeats;
            //           Debug.Log("early beat!!!");
            InsertInputNote(inputType, judgeBeat, UIBarController.Instance.preBar);
        }

        //第三拍之前
        if ((UIBarController.Instance.playingBarPosInBeats < 2 - RhythmController.Instance.commentGoodTime))
        {

            judgeBeat = UIBarController.Instance.playingBarPosInBeats;
            //     Debug.Log("normal beat!!!");
            InsertInputNote(inputType, judgeBeat, UIBarController.Instance.playingBar);

        }



        //第三拍
        if ((UIBarController.Instance.playingBarPosInBeats < RhythmController.Instance.commentGoodTime + 2) && (UIBarController.Instance.playingBarPosInBeats > 2 - RhythmController.Instance.commentGoodTime))
        {

            judgeBeat = UIBarController.Instance.playingBarPosInBeats;
            //            Debug.Log("third beat!!!");
            InsertInputNote(inputType, judgeBeat, UIBarController.Instance.playingBar);

        }
    }
    #endregion



    //超级取消
    public void SuperCancel()
    {
        //搓招正确但是能量不足
        if (Player.Instance.Mp < 4)
        {
            //Debug.Log("能量不足");
            Bad("NOT ENOUGH ENERGY");
            return;
        }
        Player.Instance.Mp -= 4;
        ClnInpSeqWhenCastSkill();
        ResetAvailable();
        RhythmController.Instance.isCurBarCleaned = false;
        SuperController.Instance.ShowInputTip("Super Cancel", 1);

    }

    #region Insert Input Note 根据输入把新的NOTE增加进招式序列
    public void InsertInputNote(Note.NoteType inputType, float beat, GameObject bar)
    {

        //  Debug.Log("availableSkills.Count" + availableSkills.Count);

        //Debug.Log("CurInputSequence.Count"+ CurInputSequence.Count);


        //已经BAD的情况
        if (RhythmController.Instance.isCurBarCleaned == true)
        {
            Debug.Log("已经BAD(当前不可输入）");
            return;
        }
        List<Skill> tempskills = new List<Skill>();
        bool inputsuccess = false;
        foreach (Skill skill in availableSkills)
        {
            //if (skill == null) continue;
            if (CurInputSequence.Count < skill.inputSequence.Count)
            {

//                Debug.Log("skill.inputSequence.Count" + skill.inputSequence.Count);

                if ((beat >= skill.inputSequence[CurInputSequence.Count].beatInBar - RhythmController.Instance.commentGoodTime) && (judgeBeat <= skill.inputSequence[CurInputSequence.Count].beatInBar + RhythmController.Instance.commentGoodTime) && inputType == skill.inputSequence[CurInputSequence.Count].type)
                {
                    inputsuccess = true;
//                    Debug.Log("判定成功！！");
                    tempskills.Add(skill);
                    SuperController.Instance.skillTipBarController.AddRightOInBar(skill.m_name, CurInputSequence.Count);
                }
                else
                {
                    SuperController.Instance.skillTipBarController.RemoveRightO(skill.m_name);
                }
            }
        }


        CurInputSequence.Add(bar.GetComponent<UIBar>().AddInputNote(inputType, beat));

       

        if (!inputsuccess)
        {
            //Debug.Break();
            Bad("BAD");
            return;
        }
  




        availableSkills = tempskills;

        //如果完全输入，则发动招式
        foreach (Skill skill in availableSkills)
        {
            //if (skill == null) continue;
            if (skill.inputSequence.Count == CurInputSequence.Count)
            {
                //搓招正确但是能量不足
                if (Player.Instance.Mp < skill.cost)
                {
                    //Debug.Log("能量不足");
                    inputsuccess = false;
                    Bad("NOT ENOUGH ENERGY");
                    return;
                }
                //搓招正确但是技能CD
                if (skill.Cooldown>0)
                {
                    //Debug.Log("SKILL COOLDOWN");
                    inputsuccess = false;
                    Bad("SKILL COOLDOWN");
                    return;
                }
                //搓招正确但是魂不足
                if (Player.Instance.soulPoint < Player.Instance.soulMaxPoint && skill.type==SkillType.ulti)
                {
                    //Debug.Log("NOT ENOUGH SOUL");
                    inputsuccess = false;
                    Bad("NOT ENOUGH SOUL");
                    return;
                }
                else
                {
                    //cost小于0时在技能中特殊处理
                    if (skill.cost >= 0)
                    {
                        Player.Instance.Mp -= skill.cost;
                        Player.Instance.AddSoul(skill.cost);
                    }


                    //和AI同时出招
                    if (!DuelController.Instance.isActedAt3rdBeat)
                    {
                        //更新技能CD
                        Player.Instance.UpdateCDs();
                        //                        Debug.Log("输入正确之后的发招 " +skill.m_name+" "+ skill.EffectStr);
                        skill.Cooldown = skill.CooldownMax;


                        if (DuelController.Instance.GetCurAI())
                        {
                            if (DuelController.Instance.GetCurAI().HasBuff("stunned"))
                            {
                                Debug.Log("晕菜");
                                DuelController.Instance.SkillJudge(skill.EffectStr, "");

                            }
                            else
                            {
                                DuelController.Instance.SkillJudge(skill.EffectStr, DuelController.Instance.GetCurAI().GetNextSkill(3));
                                if (DuelController.Instance.GetCurAI()) DuelController.Instance.GetCurAI().Action(3);
                            }
                        }

                        //更新技能CD
                        //Player.Instance.UpdateCDs();
                        SuperController.Instance.skillTipBarController.UpdateCDs();
                    }
                    ClnInpSeqWhenCastSkill();
                    RhythmController.Instance.isCurBarCleaned = true;
                    return;
                }


            }
        }


        if (inputsuccess)
        {
            //vfx
            switch (inputType)
            {
                case Note.NoteType.inputBassdrum:
                //    SuperController.Instance.SRDTap.NewShape(10);
                    break;
                case Note.NoteType.inputSnare:
                //    SuperController.Instance.SRDTap.NewShape(10);
                    break;

            }
        }

    }
    #endregion

    #region BAD 后处理音符
    public void Bad(string tip)
    {

        SuperController.Instance.ShowInputTip(tip);

        SoundController.Instance.PlayAudioEffect("Side");
        if (Instance.CurInputSequence.Count > 0)
        {
            int tempcount = Instance.CurInputSequence.Count;
            for (int i = 0; i < tempcount; i++)
            {
                Instance.CurInputSequence[0].note.GetComponent<VFX>().StartCoroutine("NoteInputBad");
                Instance.CurInputSequence.RemoveAt(0);
            }
            int tempcountinbar = UIBarController.Instance.playingBar.GetComponent<UIBar>().noteList_main.Count;
            for (int i = 0; i < tempcountinbar; i++)
            {
                UIBarController.Instance.playingBar.GetComponent<UIBar>().noteList_main.RemoveAt(0);
            }
        }
        ResetAvailable();
        SuperController.Instance.skillTipBarController.RemoveAllRightO();
        RhythmController.Instance.isCurBarCleaned = true;
    }
    #endregion

    public void ResetAvailable()
    {
        if (availableSkills == null) availableSkills = new List<Skill>();
        else availableSkills.Clear();
        foreach (var slots in Player.Instance.skillSlots)
            if (slots.skill!=null) availableSkills.Add(slots.skill);
    }
    #region CleanInputSequence 清除输入记录
    public void CleanInputSequence()
    {
        //        Debug.Log("clean input sequence");
        if (Instance.CurInputSequence.Count > 0)
        {
            int tempcount = Instance.CurInputSequence.Count;
            for (int i = 0; i < tempcount; i++)
            {
                Destroy(Instance.CurInputSequence[0].note);
                Instance.CurInputSequence.RemoveAt(0);
            }
            int tempcountinbar = UIBarController.Instance.playingBar.GetComponent<UIBar>().noteList_main.Count;
            for (int i = 0; i < tempcountinbar; i++)
            {
                //Destroy(UIBarController.Instance.playingBar.GetComponent<UIBar>().noteList_main[0].note);
                UIBarController.Instance.playingBar.GetComponent<UIBar>().noteList_main.RemoveAt(0);
            }
        }
        ResetAvailable();
        SuperController.Instance.skillTipBarController.RemoveAllRightO();

        RhythmController.Instance.isCurBarCleaned = true;
    }
    #endregion

    #region CleanInputSequenceWhenCastSkill 清除输入记录
    public void ClnInpSeqWhenCastSkill()
    {
        //        Debug.Log("clean input sequence");
        if (Instance.CurInputSequence.Count > 0)
        {
            int tempcount = Instance.CurInputSequence.Count;
            for (int i = 0; i < tempcount; i++)
            {
                Instance.CurInputSequence[0].note.GetComponent<VFX>().StartCoroutine("FadeOutLarger");
                Instance.CurInputSequence.RemoveAt(0);
            }
            int tempcountinbar = UIBarController.Instance.playingBar.GetComponent<UIBar>().noteList_main.Count;
            for (int i = 0; i < tempcountinbar; i++)
            {
                UIBarController.Instance.playingBar.GetComponent<UIBar>().noteList_main.RemoveAt(0);
            }
        }
        ResetAvailable();
        SuperController.Instance.skillTipBarController.RemoveAllRightOWhenSuccess();
        RhythmController.Instance.isCurBarCleaned = true;
    }
    #endregion


    #region QTE时的按键判定
    public void QTEInput(Note.NoteType inputtype)
    {
        //锁定状态直接RETURN
        if (RhythmController.Instance.badTime >= 0)
        {
            return;
        }

        if (RhythmController.InputComment(UIBarController.Instance.currentQTENotes) < 2)
        {
            if (inputtype == UIBarController.Instance.currentQTENotes[0].type)
            {
                Debug.Log(UIBarController.Instance.currentQTENotes[0].SuccessSkill);
                UIBarController.Instance.currentQTENotes[0].note.GetComponent<VFX>().StartCoroutine("FadeOutLarger");
                SuperController.Instance.ShowInputTip("NICE",1);

                Player.Instance.enemyList[0].GetComponent<AI>().QTEAction(UIBarController.Instance.currentQTENotes[0].SuccessSkill);
                if (inputtype==Note.NoteType.QTEHihat)
                {
                    SoundController.Instance.PlayAudioEffect("Hihat");

                }
                if (inputtype == Note.NoteType.QTESnare)
                {
                    SoundController.Instance.PlayAudioEffect("Snare");

                }
                if (inputtype == Note.NoteType.QTEBassdrum)
                {
                    SoundController.Instance.PlayAudioEffect("Bassdrum");

                }
            }
            //else
            //{
            //    Debug.Log(UIBarController.Instance.currentQTENotes[0].BadSkill);
            //    UIBarController.Instance.currentQTENotes[0].note.GetComponent<VFX>().StartCoroutine("NoteInputBad");
            //    SuperController.Instance.ShowInputTip("BAD");
            //    Player.Instance.enemyList[0].GetComponent<AI>().QTEAction(UIBarController.Instance.currentQTENotes[0].BadSkill);

            //}
            UIBarController.Instance.currentQTENotes.RemoveAt(0);
        }
        else
        {
            RhythmController.Instance.LockPin();
        }
    }
    #endregion

    #region 必杀时的按键判定
    public void UltiInput(Note.NoteType inputtype)
    {
        //锁定状态直接RETURN
        if (RhythmController.Instance.badTime >= 0)
        {
            return;
        }

        if (RhythmController.InputComment(UIBarController.Instance.currentQTENotes) < 2)
        {
            if (inputtype == UIBarController.Instance.currentQTENotes[0].type)
            {
                Debug.Log(UIBarController.Instance.currentQTENotes[0].SuccessSkill);
                UIBarController.Instance.currentQTENotes[0].note.GetComponent<VFX>().StartCoroutine("FadeOutLarger");

                Player.Instance.UltiAction(UIBarController.Instance.currentQTENotes[0].SuccessSkill);

            }
            else
            {
                Debug.Log(UIBarController.Instance.currentQTENotes[0].SuccessSkill);
                UIBarController.Instance.currentQTENotes[0].note.GetComponent<VFX>().StartCoroutine("NoteInputBad");

                Player.Instance.UltiAction(UIBarController.Instance.currentQTENotes[0].BadSkill);

            }
            UIBarController.Instance.currentQTENotes.RemoveAt(0);
        }
        else
        {
         //  RhythmController.Instance.LockPin();
        }
    }
    #endregion
}
