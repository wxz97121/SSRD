using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputSequenceController : MonoBehaviour
{
    public List<Skill> skills;
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
        Debug.Log("UIBarController.Instance.playingBarPosInBeats!" + UIBarController.Instance.playingBarPosInBeats);
//        Debug.Log("RhythmController.Instance.commentGoodTime!" + RhythmController.Instance.commentGoodTime);





        //第三拍之后,要记入下一小节
        if ((UIBarController.Instance.playingBarPosInBeats > RhythmController.Instance.commentGoodTime + 2) )
        {

            judgeBeat = UIBarController.Instance.preBarPosInBeats;
            Debug.Log("early beat!!!");
            InsertInputNote(inputType, judgeBeat, UIBarController.Instance.preBar);
        }

        //第三拍之前
        if ((UIBarController.Instance.playingBarPosInBeats < 2-RhythmController.Instance.commentGoodTime))
        {

            judgeBeat = UIBarController.Instance.playingBarPosInBeats;
            Debug.Log("normal beat!!!");
            InsertInputNote(inputType, judgeBeat, UIBarController.Instance.playingBar);

        }



        //第三拍
        if ((UIBarController.Instance.playingBarPosInBeats < RhythmController.Instance.commentGoodTime + 2) && (UIBarController.Instance.playingBarPosInBeats > 2 - RhythmController.Instance.commentGoodTime))
        {

            judgeBeat = UIBarController.Instance.playingBarPosInBeats;
            Debug.Log("third beat!!!");
            InsertInputNote(inputType, judgeBeat, UIBarController.Instance.playingBar);

            //todo 结算本小节的技能
        }
    }

    #region Insert Input Note 根据输入把新的NOTE增加进招式序列
    public void InsertInputNote(Note.NoteType inputType,float beat,GameObject bar)
    {
        foreach (Skill skill in skills)
        {
            if (CurInputSequence.Count >= skill.inputSequence.Count)
            {
                continue;
            }
//            Debug.Log("CurInputSequence.Count"+ CurInputSequence.Count);
            if((judgeBeat>=skill.inputSequence[CurInputSequence.Count].beatInBar-RhythmController.Instance.commentGoodTime)&&(judgeBeat <= skill.inputSequence[CurInputSequence.Count].beatInBar + RhythmController.Instance.commentGoodTime)){
                if (inputType == skill.inputSequence[CurInputSequence.Count].type)
                {

                
                Note note = new Note
                {
                    type = inputType,
                    beatInBar = beat,
                };
                if (inputType == Note.NoteType.inputBassdrum)
                {
                    note.note = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note_Bassdrum", typeof(GameObject)), bar.transform);

                }else
                {
                    note.note = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar_Note_Snare", typeof(GameObject)), bar.transform);

                }
                note.note.transform.localPosition = bar.GetComponent<UIBar>().startPos + (bar.GetComponent<UIBar>().oneBeatSpace * beat) + new Vector3(0, -10, 0);
                CurInputSequence.Add(note);
                bar.GetComponent<UIBar>().noteList_main.Add(note);
                }
            }

            //如果完全输入，则发动招式
            if (skill.inputSequence.Count==CurInputSequence.Count)
            {
                Debug.Log("cast:"+skill.name);
            }
        }
    }
    #endregion


    #region CleanInputSequence 清除输入记录
    public void CleanInputSequence()
    {
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
        RhythmController.Instance.isCurBarCleaned = true;

    }
    #endregion
}
