using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//统一控制局内
public class SuperController : MonoBehaviour {
    public bool mEndLock_energy = false;
    //评价控制(评价控制还没改成全局控制)
    public CommentController commentController = null;
    public UISkillTipBarController skillTipBarController = null;


    static SuperController _instance;
    public static SuperController Instance
    {
        get
        {
            return _instance;
        }
    }
    private void Awake()
    {
        _instance = this;




    }


    // Use this for initialization
    void Start () {
        commentController = GameObject.Find("Comment").GetComponent<CommentController>();
        skillTipBarController = GameObject.Find("SkillTipArea").GetComponent<UISkillTipBarController>();


        LevelData.Instance.ReadScoreDatas();

        LevelData.Instance.ReadSkillDatas();

        RhythmController.Instance.Reset();

        skillTipBarController.InitSkillTipBarArea();

    }

    // Update is called once per frame
    void Update () {
        UpdateInput();
	}

    protected void UpdateInput()
    {

        if (Input.GetKeyDown(KeyCode.M))
        {
            DuelController.Instance.ShowAction(actionType.Collect);
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
//            Debug.Log("INPUT Z");
            InputSequenceController.Instance.CalcSkillInput(Note.NoteType.inputBassdrum);
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
 //           Debug.Log("INPUT X");

            InputSequenceController.Instance.CalcSkillInput(Note.NoteType.inputSnare);
        }



    }
}
