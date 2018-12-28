using System.Collections;
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

    public void CalcSkillInput(Note.NoteType inputType)
    {
//        Debug.Log("UIBarController.Instance.playingBarPosInBeats!" + UIBarController.Instance.playingBarPosInBeats);
//        Debug.Log("RhythmController.Instance.commentGoodTime!" + RhythmController.Instance.commentGoodTime);





        //第三拍之后,要记入下一小节
        if ((UIBarController.Instance.playingBarPosInBeats > RhythmController.Instance.commentGoodTime + 2) )
        {

            judgeBeat = UIBarController.Instance.preBarPosInBeats;
//           Debug.Log("early beat!!!");
            InsertInputNote(inputType, judgeBeat, UIBarController.Instance.preBar);
        }

        //第三拍之前
        if ((UIBarController.Instance.playingBarPosInBeats < 2-RhythmController.Instance.commentGoodTime))
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

            //todo 结算本小节的技能
        }
    }

    #region Insert Input Note 根据输入把新的NOTE增加进招式序列
    public void InsertInputNote(Note.NoteType inputType,float beat,GameObject bar)
    {
//        Debug.Log("-----------------------");
  //      Debug.Log("inputtype:"+inputType);
    //    Debug.Log("beat:" + beat);
      //  Debug.Log("availableSkills.Count" + availableSkills.Count);

        //Debug.Log("CurInputSequence.Count"+ CurInputSequence.Count);



        if (RhythmController.Instance.isCurBarCleaned == true)
        {
            Debug.Log("bad");
            return;
        }
        List<Skill> tempskills = new List<Skill>();
        bool inputsucces = false;
        foreach (Skill skill in availableSkills)
        {
            if (CurInputSequence.Count < skill.inputSequence.Count)
            {

    //            Debug.Log("skill.inputSequence.Count" + skill.inputSequence.Count);

                if ((beat >= skill.inputSequence[CurInputSequence.Count].beatInBar - RhythmController.Instance.commentGoodTime) && (judgeBeat <= skill.inputSequence[CurInputSequence.Count].beatInBar + RhythmController.Instance.commentGoodTime)&& inputType == skill.inputSequence[CurInputSequence.Count].type)
                {
                    inputsucces = true;
//                    Debug.Log("判定成功！！");
                    tempskills.Add(skill);
                    

                }
            }
        }

        if (inputsucces)
        {
            //创建音符
            Note note = new Note
            {
                type = inputType,
                beatInBar = beat,
            };
            if (inputType == Note.NoteType.inputBassdrum)
            {
                note.note = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note_Bassdrum", typeof(GameObject)), bar.transform);
                SoundController.Instance.PlayAudioEffect("KICK");

            }
            else
            {
                note.note = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note_Snare", typeof(GameObject)), bar.transform);
                SoundController.Instance.PlayAudioEffect("SNARE");
            }
            note.note.transform.localPosition = bar.GetComponent<UIBar>().startPos + (bar.GetComponent<UIBar>().oneBeatSpace * beat) + new Vector3(0, -10, 0);
            CurInputSequence.Add(note);
            bar.GetComponent<UIBar>().noteList_main.Add(note);
            //            Debug.Log("add note");
        }
        else
        {
            CleanInputSequence();
            Debug.Log("bad");
        }


        availableSkills = tempskills;

        //如果完全输入，则发动招式
        foreach (Skill skill in availableSkills)
            if (skill.inputSequence.Count == CurInputSequence.Count)
            {
                Debug.Log("cast:" + skill.name);
                ClnInpSeqWhenCastSkill();
                RhythmController.Instance.isCurBarCleaned = true;

            }
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
                Instance.CurInputSequence[0].note.GetComponent<VFX>().StartCoroutine("UINoteFadeOut");
                Instance.CurInputSequence.RemoveAt(0);
            }
            int tempcountinbar = UIBarController.Instance.playingBar.GetComponent<UIBar>().noteList_main.Count;
            for (int i = 0; i < tempcountinbar; i++)
            {
                UIBarController.Instance.playingBar.GetComponent<UIBar>().noteList_main.RemoveAt(0);
            }
        }
        availableSkills = skills;
        RhythmController.Instance.isCurBarCleaned = true;
    }
    #endregion
}
