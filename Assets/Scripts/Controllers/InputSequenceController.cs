using System.Collections.Generic;
using UnityEngine;

public class InputSequenceController : MonoBehaviour
{
    //存技能列表
    public List<Skill> skills;
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
                SoundController.Instance.PlayAudioEffect("HIHAT");

                UIBarController.Instance.currentEnergyNotes[0].note.GetComponent<VFX>().StartCoroutine("FadeOutLarger");
                UIBarController.Instance.currentEnergyNotes.RemoveAt(0);

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
            SoundController.Instance.PlayAudioEffect("HIHAT");

            UIBarController.Instance.currentEnergyNotes[0].note.GetComponent<VFX>().StartCoroutine("FadeOutLarger");
            UIBarController.Instance.currentEnergyNotes.RemoveAt(0);

            //      Debug.Log("2");
        }
        else
        {
            RhythmController.Instance.LockPin();
        }
        //


    }
    #endregion


    public void CalcSkillInput(Note.NoteType inputType)
    {
        //        Debug.Log("UIBarController.Instance.playingBarPosInBeats!" + UIBarController.Instance.playingBarPosInBeats);
        //        Debug.Log("RhythmController.Instance.commentGoodTime!" + RhythmController.Instance.commentGoodTime);

        //锁定状态直接RETURN
        if (RhythmController.Instance.badTime >= 0)
        {
            return;
        }



        //第三拍之后,要记入下一小节
        if ((UIBarController.Instance.playingBarPosInBeats > RhythmController.Instance.commentGoodTime + 2))
        {

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



    #region Insert Input Note 根据输入把新的NOTE增加进招式序列
    public void InsertInputNote(Note.NoteType inputType, float beat, GameObject bar)
    {

        //  Debug.Log("availableSkills.Count" + availableSkills.Count);

        //Debug.Log("CurInputSequence.Count"+ CurInputSequence.Count);



        if (RhythmController.Instance.isCurBarCleaned == true)
        {
            Debug.Log("已经BAD(当前不可输入）");
            return;
        }
        List<Skill> tempskills = new List<Skill>();
        bool inputsuccess = false;
        foreach (Skill skill in availableSkills)
        {
            if (CurInputSequence.Count < skill.inputSequence.Count)
            {

                //Debug.Log("skill.inputSequence.Count" + skill.inputSequence.Count);

                if ((beat >= skill.inputSequence[CurInputSequence.Count].beatInBar - RhythmController.Instance.commentGoodTime) && (judgeBeat <= skill.inputSequence[CurInputSequence.Count].beatInBar + RhythmController.Instance.commentGoodTime) && inputType == skill.inputSequence[CurInputSequence.Count].type)
                {
                    inputsuccess = true;
                    //Debug.Log("判定成功！！");
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

            Bad();
            return;
        }
  




        availableSkills = tempskills;

        //如果完全输入，则发动招式
        foreach (Skill skill in availableSkills)
        {

        if (skill.inputSequence.Count == CurInputSequence.Count)
            {
                //搓招正确但是能量不足
                if (Player.Instance.Mp < skill.cost)
                {
                    Debug.Log("能量不足");
                    inputsuccess = false;
                    Bad();
                }
                else
                {
                    Player.Instance.Mp -= skill.cost;
//                    Debug.Log("cast:" + skill.m_name);
                    skill.EffectFunction(Player.Instance);
                    ClnInpSeqWhenCastSkill();
                    RhythmController.Instance.isCurBarCleaned = true;
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
    public void Bad()
    {
        SoundController.Instance.PlayAudioEffect("ROUND");
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
        availableSkills = skills;
        SuperController.Instance.skillTipBarController.RemoveAllRightO();
        RhythmController.Instance.isCurBarCleaned = true;
    }
    #endregion


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
        availableSkills = skills;
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
        availableSkills = skills;
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

                Player.Instance.enemyList[0].GetComponent<AI>().QTEAction(UIBarController.Instance.currentQTENotes[0].SuccessSkill);

            }
            else
            {
                Debug.Log(UIBarController.Instance.currentQTENotes[0].SuccessSkill);
                UIBarController.Instance.currentQTENotes[0].note.GetComponent<VFX>().StartCoroutine("NoteInputBad");

                Player.Instance.enemyList[0].GetComponent<AI>().QTEAction(UIBarController.Instance.currentQTENotes[0].BadSkill);

            }
            UIBarController.Instance.currentQTENotes.RemoveAt(0);
        }
        else
        {
            RhythmController.Instance.LockPin();
        }
    }
    #endregion
}
