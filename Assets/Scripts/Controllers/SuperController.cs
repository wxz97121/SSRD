using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public enum GameState
{
    Wait,
    Loot,
    Start,
    QTE,
    End,
    Pause,
    Ulti
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

    //在暂停时储存暂停前的状态
    public GameState tempstate;
    public bool pausing;
    //SRDTap
    public SrdTap SRDTap;

    public GameState state;
    //主菜单UI
    public Transform mainMenu;

    //临时选技能菜单
    public Transform skillSelectUI;
    public Transform skillDragSlotUI;
    //战斗ui
    public Transform playerBattleUIPos;
    public Transform enemyBattleUIPos;
    public UIBattleInfo playerBattleInfo;
    public UIBattleInfo enemyBattleInfo;

    public Transform InputTipPos;

    public SkillData[] skillList;

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
        Random.InitState(System.DateTime.Now.Second);
        mainMenu.gameObject.SetActive(true);
        commentController = GameObject.Find("Comment").GetComponent<CommentController>();
        skillTipBarController = GameObject.Find("SkillTipArea").GetComponent<UISkillTipBarController>();
        uiBarController = GameObject.Find("BarArea").GetComponent<UIBarController>();
        SRDTap = GameObject.Find("SrdTap").GetComponent<SrdTap>();

        playerBattleInfo = playerBattleUIPos.GetComponentInChildren<UIBattleInfo>();
        enemyBattleInfo =enemyBattleUIPos.GetComponentInChildren<UIBattleInfo>();

        ReadLevelDatas();
        InputSequenceController.Instance.ResetAvailable();
        //        ReadSkillDatas();
        SoundController.Instance.FMODmusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        //此处有个坑，FMODInstance必须创建完成才能获取channelgroup
        SoundController.Instance.FMODMusicChange(SuperController.Instance.levelData.BGMPath);
        state = GameState.Wait;
    }

    // Update is called once per frame
    void Update () {
        UpdateInput();
	}

    protected void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausing)
            {
                Resume();
                pausing = false;
                return;
            }
            Pause(GameState.Wait);
            pausing = true;
        }

        if (SuperController.Instance.state != GameState.Start && SuperController.Instance.state != GameState.QTE && SuperController.Instance.state != GameState.Ulti)
        {
            return;
        }

        //测试专用键
        if (Input.GetKeyDown(KeyCode.A))
        {
            Bubble.AddBubble(BubbleSprType.hp, "-6", Player.Instance);
                    }
        if (Input.GetKeyDown(KeyCode.S))
        {
            SoundController.Instance.FMODSetParameter("boss", 0);
            SoundController.Instance.FMODSetParameter("chorus", 1);
            SoundController.Instance.FMODSetParameter("verse", 0);
            SoundController.Instance.FMODSetParameter("breakdown", 0);
            SoundController.Instance.FMODSetParameter("outro", 0);
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            SoundController.Instance.FMODSetParameter("boss", 0);
            SoundController.Instance.FMODSetParameter("chorus", 0);
            SoundController.Instance.FMODSetParameter("verse", 1);
            SoundController.Instance.FMODSetParameter("breakdown", 0);
            SoundController.Instance.FMODSetParameter("outro", 0);
        }
        if (Input.GetKeyDown(KeyCode.F))
        {
            SoundController.Instance.FMODSetParameter("boss", 0);
            SoundController.Instance.FMODSetParameter("chorus", 0);
            SoundController.Instance.FMODSetParameter("verse", 0);
            SoundController.Instance.FMODSetParameter("breakdown", 1);
            SoundController.Instance.FMODSetParameter("outro", 0);
        }
        if (Input.GetKeyDown(KeyCode.G))
        {
            SoundController.Instance.FMODSetParameter("boss", 0);
            SoundController.Instance.FMODSetParameter("chorus", 0);
            SoundController.Instance.FMODSetParameter("verse", 0);
            SoundController.Instance.FMODSetParameter("breakdown", 0);
            SoundController.Instance.FMODSetParameter("outro", 1);
        }

        if (Input.GetKeyDown(KeyCode.Q))
        {
              SoundController.Instance.FMODPlayOneShot("event:/instruments/snare");
              SoundController.Instance.PlayAudioEffect("SNARE");
           // Debug.Log(SoundController.Instance.GetSpectrum().Length);
        }

        //战斗输入按键
        if (Input.GetKeyDown(KeyCode.M))
        {
            //            Debug.Log("INPUT M");
            switch (state)
            {
                case GameState.Start:
                    InputSequenceController.Instance.CollectEnergy();
                    break;
                case GameState.QTE:
                    InputSequenceController.Instance.QTEInput(Note.NoteType.QTEHihat);
                    break;
                case GameState.Ulti:
                    InputSequenceController.Instance.UltiInput(Note.NoteType.QTEHihat);

                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.Z))
        {
            //            Debug.Log("INPUT Z");
            switch (state)
            {
                case GameState.Start:
                    InputSequenceController.Instance.CalcSkillInput(Note.NoteType.inputBassdrum);
                    break;
                case GameState.QTE:
                    InputSequenceController.Instance.QTEInput(Note.NoteType.QTEBassdrum);
                    break;
                case GameState.Ulti:
                    InputSequenceController.Instance.UltiInput(Note.NoteType.QTEBassdrum);

                    break;
            }
        }
        if (Input.GetKeyDown(KeyCode.X))
        {
            //           Debug.Log("INPUT X");
            switch (state)
            {
                case GameState.Start:
                    InputSequenceController.Instance.CalcSkillInput(Note.NoteType.inputSnare);
                    break;
                case GameState.QTE:
                    InputSequenceController.Instance.QTEInput(Note.NoteType.QTESnare);

                    break;
                case GameState.Ulti:
                    InputSequenceController.Instance.UltiInput(Note.NoteType.QTESnare);

                    break;
            }
        }

        if (Input.GetKeyDown(KeyCode.LeftShift))
        {
            //           Debug.Log("INPUT X");
            switch (state)
            {
                case GameState.Start:
                    InputSequenceController.Instance.SuperCancel();
                    break;

            }
        }

    }

    public void NewGame()
    {
        DuelController.Instance.ClearEnemy();
        Player.Instance.currentArmor = null;
        Player.Instance.currentScroll = null;
        Player.Instance.currentWeapon = null;
        Player.Instance.equipmentList.Clear();
        Player.Instance.buffs.Clear();
        SkillSelectUI();


        mainMenu.gameObject.SetActive(false);


    }

    public void GameOver()
    {
        //Debug.Log("Game Over");

        state = GameState.End;
        uiBarController.ClearBarArea();
        skillTipBarController.ClearSkillTipArea();
        //SoundController.Instance.SetPlayedTime();
        SoundController.Instance.FMODmusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        StartCoroutine("GameOverUI");
        
    }

    IEnumerator GameOverUI()
    {
        yield return new WaitForSeconds(2.0f);

        mainMenu.gameObject.SetActive(true);
        mainMenu.Find("Title").GetComponent<Text>().text = "死";
        mainMenu.Find("Button").Find("Text").GetComponent<Text>().text = "再来!";
    }
    public void Win()
    {
        //Debug.Log("Game Over");

        state = GameState.End;
        uiBarController.ClearBarArea();
        skillTipBarController.ClearSkillTipArea();
        //SoundController.Instance.SetPlayedTime();
        SoundController.Instance.FMODmusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);
        StartCoroutine("WinUI");

    }
    IEnumerator WinUI()
    {
        yield return new WaitForSeconds(2.0f);

        mainMenu.gameObject.SetActive(true);
        mainMenu.Find("Title").GetComponent<Text>().text = "牛逼！";
        mainMenu.Find("Button").Find("Text").GetComponent<Text>().text = "再来!";
    }

    void SkillSelectUI()
    {
        skillSelectUI.gameObject.SetActive(true);
        foreach (var skill in skillList)
        {
            var inst = Instantiate(Resources.Load<GameObject>("Prefab/SkillDrag"), skillDragSlotUI);
            Debug.Log(skill.name);
            inst.GetComponent<SkillDrag>().InitSkill(skill.name);
        }
    }

    //真正的开始游戏 TODO：这里的一些初始化代码需要封装
    public void SkillSelectOK()
    {
        state = GameState.Start;

        Pause(GameState.Loot);
        LootController.Instance.NewLoot();

        skillSelectUI.gameObject.SetActive(false);

        skillTipBarController.InitSkillTipBarArea();
        RhythmController.Instance.Reset();

        Player.Instance.Reset();
        Player.Instance.BattleStart();

        //战斗UI


        playerBattleInfo.init(Player.Instance);
        //enemyBattleInfo.init(Player.Instance.mTarget.GetComponent<Character>());
        InputSequenceController.Instance.ResetAvailable();

    }

    //public void ReadSkillDatas()
    //{
    //List<Skill> playerSkills = new List<Skill>
    //{
    //    new Skill("testSkill_00X_ATTACK"),
    //    new Skill("testSkill_0ZX_DEFEND"),
    //    new Skill("testSkill_Z0X_SUPERATTACK"),
    //    new Skill("testSkill_ZZX_TRIPLEDMG"),
    //    new Skill("testSkill_0Z0ZX_HEAL"),
    //    new Skill("testSkill_ZZZX_ALLMPATK"),

    //    new Skill("testSkill_ZXZZX_ULTI")
    //};
    //Player.Instance.skills = new List<Skill>();

    //Player.Instance.skills = playerSkills;
    //InputSequenceController.Instance.skills = Player.Instance.skills;
    //InputSequenceController.Instance.availableSkills = InputSequenceController.Instance.skills;

    //}

    public void ReadLevelDatas()
    {
        levelData = Resources.Load("Data/Level/testLevel") as LevelData;
        //Debug.Log("leveldata" + levelData.name);
        //Debug.Log("scoredata" + levelData.scoreData.name);
        //RhythmController.Instance.BGM = levelData.BGM;
        score = OneSongScore.ReadScoreData(levelData.scoreData);
        DuelController.Instance.enemyList = levelData.enemyList;
    }

    public void Pause(GameState _state)
    {
        tempstate = state;
        Debug.Log("tempstate"+ tempstate);
        state = _state;
        Debug.Log("state" + state);

        SoundController.Instance.FMODmusic.setPaused(true);
    }

    public void Resume()
    {
        state = tempstate;
        Debug.Log("state" + state);

        SoundController.Instance.FMODmusic.setPaused(false);
    }

    //一些UI的按节奏闪动
    public void Blink()
    {
        playerBattleInfo.Blink();
        enemyBattleInfo.Blink();
    }

    //
    public void ShowInputTip(string text,int type=0)
    {
        GameObject tip = Instantiate(Resources.Load("Prefab/InputTip/InputTip"), InputTipPos.transform) as GameObject;
        tip.GetComponent<InputTip>().Init(text,type);
    }
}
