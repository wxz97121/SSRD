using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameState
{
    Wait,
    Start,
    End,
    Pause
}
//统一控制局内
public class SuperController : MonoBehaviour {

    public LevelData levelData;
    //评价控制(评价控制还没改成全局控制)
    public CommentController commentController = null;
    public UISkillTipBarController skillTipBarController = null;
    public UIBarController uiBarController = null;
    //谱子
    public OneSongScore score;

    //TODO:大流程控制！
    public GameState state;
    //主菜单UI
    public Transform mainMenu;


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
        uiBarController = GameObject.Find("BarArea").GetComponent<UIBarController>();


        ReadLevelDatas();
        ReadSkillDatas();

        state = GameState.Wait;
    }

    // Update is called once per frame
    void Update () {
        UpdateInput();
	}

    protected void UpdateInput()
    {
        if (SuperController.Instance.state != GameState.Start)
        {
            return;
        }

        //测试专用键



        if (Input.GetKeyDown(KeyCode.M))
        {
//            Debug.Log("INPUT M");

            InputSequenceController.Instance.CollectEnergy();
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

    public void NewGame()
    {
        mainMenu.gameObject.SetActive(false);
        skillTipBarController.InitSkillTipBarArea();
        RhythmController.Instance.StartCoroutine("Reset");
       uiBarController.InitController();

        state = GameState.Start;

        Player.Instance.Reset();
    }

    public void GameOver()
    {
        //Debug.Log("Game Over");
        state = GameState.End;
        DuelController.Instance.ClearEnemy();
        uiBarController.ClearBarArea();
        skillTipBarController.ClearSkillTipArea();
        StartCoroutine("GameOverUI");
        
    }

    IEnumerator GameOverUI()
    {
        yield return new WaitForSeconds(2.0f);

        mainMenu.gameObject.SetActive(true);
        mainMenu.Find("Title").GetComponent<Text>().text = "你挂了";
        mainMenu.Find("Button").Find("Text").GetComponent<Text>().text = "再来!";
    }

    public void ReadSkillDatas()
    {
        List<Skill> playerSkills = new List<Skill>
        {
            new Skill("testSkill_00X_ATTACK"),
            new Skill("testSkill_0ZX_DEFEND"),
            new Skill("testSkill_ZZX_SUPERATTACK"),
            new Skill("testSkill_0Z0ZX_HEAL"),
            new Skill("testSkill_0Z0ZZX_ULTI")
        };
        Player.Instance.skills = new List<Skill>();

        Player.Instance.skills = playerSkills;
        InputSequenceController.Instance.skills = Player.Instance.skills;
        InputSequenceController.Instance.availableSkills = InputSequenceController.Instance.skills;

    }

    public void ReadLevelDatas()
    {
        levelData = Resources.Load("Data/Level/testLevel") as LevelData;

        //Debug.Log("leveldata" + levelData.name);
        //Debug.Log("scoredata" + levelData.scoreData.name);
//        RhythmController.Instance.BGM = levelData.BGM;
        score = OneSongScore.ReadScoreData(levelData.scoreData);
        DuelController.Instance.enemyList = levelData.enemyList;
    }

}
