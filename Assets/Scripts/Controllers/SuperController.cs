﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;

public enum GameState
{
    Wait,
    Loot,
    Start,
    QTE,
    End,
    Pause,
    Ulti,
    MainMenu,
    StoryCut,
    Map,
    Menu
}
//统一控制
public class SuperController : MonoBehaviour
{
    //流程
    public string StoryStep = "fresh new";
    public List<string> FinishedStorySteps;

    public GameObject m_Canvas;
    [HideInInspector]
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


    public GameState state;
    //主菜单UI
    public Transform mainMenu;

    //临时选技能菜单
    public Transform skillSelectUI;
    public Transform skillDragSlotUI;
    public Transform equipSelectUI;
    public Transform EquipDragSlotUI;
    //战斗ui
    public Transform playerBattleUIPos;
    public Transform enemyBattleUIPos;
    public UIBattleInfo playerBattleInfo;
    public UIBattleInfo enemyBattleInfo;

    public Transform InputTipPos;

    // public SkillData[] skillList;
    public NovelScript AfterStory;

    public Transform StoryCanvas;

    public Transform bgmask;

    #region 单例
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
    #endregion

    // Use this for initialization
    void Start()
    {
        Random.InitState(System.DateTime.Now.Second);

        commentController = GameObject.Find("Comment").GetComponent<CommentController>();
        skillTipBarController = GameObject.Find("SkillTipArea").GetComponent<UISkillTipBarController>();
        uiBarController = GameObject.Find("BarArea").GetComponent<UIBarController>();

        playerBattleInfo = playerBattleUIPos.GetComponentInChildren<UIBattleInfo>();
        enemyBattleInfo = enemyBattleUIPos.GetComponentInChildren<UIBattleInfo>();

        FinishedStorySteps = new List<string>();

        InputSequenceController.Instance.ResetAvailable();



        UIWindowController.Instance.mainMenu.Open();




        SoundController.Instance.FMODmusic.stop(FMOD.Studio.STOP_MODE.IMMEDIATE);

        //此处有个坑，FMODInstance必须创建完成才能获取channelgroup

        state = GameState.Wait;
    }

    // Update is called once per frame
    void Update()
    {
        UpdateInput();
    }


    #region 输入控制
    protected void UpdateInput()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            //Pause(GameState.Wait);
            //UIWindowController.Instance.prepareWindow.Open();

            MapController.Instance.ShowMap();

            // Bubble.AddBubble(BubbleSprType.hp, "-6", Player.Instance);
        }

        if (Input.GetKeyDown(KeyCode.W))
        {
            Debug.Log(EventSystem.current.currentSelectedGameObject);

            // Bubble.AddBubble(BubbleSprType.hp, "-6", Player.Instance);
        }


        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (pausing)
            {
                Resume();
                pausing = false;
                return;
            }
            if(state == GameState.Start || state == GameState.QTE || state == GameState.Ulti)
            {
                Pause(GameState.Wait);
                pausing = true;
            }

        }

        if (state != GameState.Start && state != GameState.QTE && state != GameState.Ulti)
        {
            return;
        }

        //测试专用键



        if (Input.GetKeyDown(KeyCode.A))
        {
            Instantiate(Resources.Load("VFX/HealStream"), DuelController.Instance.GetCurAI().transform.position + new Vector3(0, 0, -1), Quaternion.identity);

            // Bubble.AddBubble(BubbleSprType.hp, "-6", Player.Instance);
        }
        if (Input.GetKeyDown(KeyCode.S))
        {
            Instantiate(Resources.Load("VFX/HealBig"), DuelController.Instance.GetCurAI().transform.position+new Vector3(0,0,-1), Quaternion.identity);

        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            Instantiate(Resources.Load("VFX/HealOnce"), DuelController.Instance.GetCurAI().transform.position + new Vector3(0, 0, -1), Quaternion.identity);

            //SoundController.Instance.FMODSetParameter("boss", 0);
            //SoundController.Instance.FMODSetParameter("chorus", 0);
            //SoundController.Instance.FMODSetParameter("verse", 1);
            //SoundController.Instance.FMODSetParameter("breakdown", 0);
            //SoundController.Instance.FMODSetParameter("outro", 0);
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

        if (Input.GetKeyDown(KeyCode.R))
        {
            SoundController.Instance.FMODPlayOneShot("event:/instruments/snare");
            SoundController.Instance.PlayAudioEffect("Snare");
            // Debug.Log(SoundController.Instance.GetSpectrum().Length);
        }
        if (Input.GetKeyDown(KeyCode.W))
        {
            //SkillUpgrade.UpgradeSkill("attack", 1, 1, Player.Instance.skillSlots[0].skill);
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
    #endregion

    #region 主线剧情关键点时调用
    public void NextStep(string p_storystep)
    {
        StoryStep = p_storystep;
        Debug.Log("Next Step : " + p_storystep);

        switch (p_storystep)
        {
            case "start game":
                //Story.PlayStoryAnim("Story1");
                SuperController.Instance.NextStep("teaching");
                FinishedStorySteps.Add("start game");
                break;
            case "teaching":
                MapController.Instance.ShowMap();
                FinishedStorySteps.Add("teaching");

                break;
            case "lesson1finished":
                if (MapController.Instance.mapAreas.Find(a => a.AreaName == "安贞医院").m_VisitType == MapState.Locked)
                {
                    MapController.Instance.mapAreas.Find(a => a.AreaName == "安贞医院").m_VisitType = MapState.Unlocked;
                }
                FinishedStorySteps.Add("lesson1finished");

                break;

            case "anzhenyiyuan_finished":
                if (MapController.Instance.mapAreas.Find(a => a.AreaName == "北土城").m_VisitType == MapState.Locked)
                {
                    MapController.Instance.mapAreas.Find(a => a.AreaName == "北土城").m_VisitType = MapState.Unlocked;
                }
                if (MapController.Instance.mapAreas.Find(a => a.AreaName == "环宇会").m_VisitType == MapState.Locked)
                {
                    MapController.Instance.mapAreas.Find(a => a.AreaName == "环宇会").m_VisitType = MapState.Unlocked;
                }
                FinishedStorySteps.Add("anzhenyiyuan_finished");

                break;

            case "beitucheng_finished":
                if (MapController.Instance.mapAreas.Find(a => a.AreaName == "奥体中心").m_VisitType == MapState.Locked)
                {
                    MapController.Instance.mapAreas.Find(a => a.AreaName == "奥体中心").m_VisitType = MapState.Unlocked;
                }

                FinishedStorySteps.Add("beitucheng_finished");

                break;

            case "huanyuhui_finished":
                if (MapController.Instance.mapAreas.Find(a => a.AreaName == "小关").m_VisitType == MapState.Locked)
                {
                    MapController.Instance.mapAreas.Find(a => a.AreaName == "小关").m_VisitType = MapState.Unlocked;
                }

                FinishedStorySteps.Add("huanyuhui_finished");

                break;
            case "aotizhongxin_finished":



                FinishedStorySteps.Add("aotizhongxin_finished");

                break;
            default:
                Debug.LogError("can not find step : " + p_storystep);
                break;
        }
    }
    #endregion

    #region 非主线剧情关键点时调用
    public void SideStoryHappen(string p_storystep)
    {
        Debug.Log("SideStoryHappen : " + p_storystep);
        //todo:支线流程在这控制吧
        switch (p_storystep)
        {


            default:
                break;
        }
    }
    #endregion







    public void NewGame()
    {
        DuelController.Instance.ClearEnemy();
        Player.Instance.currentCloth = null;
        Player.Instance.currentAmulet = null;
        Player.Instance.currentWeapon = null;
        //Player.Instance.equipmentList.Clear();
        for (int index = 0; index < Player.Instance.skillSlots.Length; index++)
            Player.Instance.skillSlots[index].skill = null;
        //Player.Instance.skillListInBag = new List<SkillData>();
        //Player.Instance.AddSkill(Resources.Load<SkillData>("Data/Skill/testSkill_00X_ATTACK"));
        Player.Instance.buffs.Clear();
        //SkillSelectUI();
//        MapController.Instance.CreateChapterMap();
        MapController.Instance.ShowMap();
        UIWindowController.Instance.mainMenu.Close();

    }


    public void ContinueAfterWin()
    {
        DuelController.Instance.ClearEnemy();
        Player.Instance.buffs.Clear();
        //CleanSelectUI();
        //Player.Instance.currentArmor = null;
        //Player.Instance.currentScroll = null;
        //Player.Instance.currentWeapon = null;
        //for (int index = 0; index < Player.Instance.skillSlots.Length; index++)
            //Player.Instance.skillSlots[index].skill = null;

        Player.Instance.Reset();

        //SkillSelectUI();
        //MapController.Instance.CreateChapterMap();
        UIWindowController.Instance.winWindow.Close();

        if (AfterStory != null)
        {
            VisualNovelController.Instance.InitScript(AfterStory);
        }
        else
        {
            MapController.Instance.ShowMap();
        }

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
        //mainMenu.Find("Button").GetComponent<Button>().onClick.AddListener(NewGame);
        mainMenu.Find("NewGameButton").Find("Text").GetComponent<Text>().text = "再来!";
        mainMenu.Find("NewGameButton").transform.localScale = Vector3.one;
        mainMenu.Find("NextButton").transform.localScale = Vector3.zero;
    }
    public void Win()
    {
        //Debug.Log("Game Over");
        Player.Instance.money += levelData.AwardMoney;

        state = GameState.End;
        uiBarController.ClearBarArea();
        skillTipBarController.ClearSkillTipArea();
        //SoundController.Instance.SetPlayedTime();
        //SoundController.Instance.FMODmusic.stop(FMOD.Studio.STOP_MODE.ALLOWFADEOUT);
        StartCoroutine(SoundController.Instance.SetFadeOut(5f));

        if (!MapController.Instance.currentMapArea.isVisited)
        {
            MapController.Instance.currentMapArea.isVisited = true;
            SuperController.Instance.NextStep(MapController.Instance.currentMapArea.levelData.NextStoryStep);
        }
        StartCoroutine("WinUI");
    }
    IEnumerator WinUI()
    {
        yield return new WaitForSeconds(2.0f);
        string award="";

        UIWindowController.Instance.winWindow.Open();

        if (levelData.AwardSkill)
        {
            Player.Instance.AddSkill(levelData.AwardSkill);
            award += levelData.AwardSkill._name+" ";
        }
        if (levelData.AwardEquip)
        {
            Player.Instance.equipmentList.Add(levelData.AwardEquip);
            award += levelData.AwardEquip.name + " ";

        }

        //UIWindowController.Instance.winWindow.desc.text = "获得 ";p
        string s_skill="";
        string s_equip="";
        string s_money = "";

        if (levelData.AwardSkill != null)
        {
            s_skill = " skill:"+levelData.AwardSkill._name+" ";
        }
        if (levelData.AwardEquip != null)
        {
            s_equip = " equipment:" + levelData.AwardSkill._name+" ";
        }
        if (levelData.AwardMoney >0)
        {
            s_money = " money:" + levelData.AwardMoney.ToString() + " ";
        }
        UIWindowController.Instance.winWindow.desc.text = s_skill+s_equip+ s_money;

        UIWindowController.Instance.winWindow.title.text = "牛逼！"+ levelData.AreaName + " Clear !";


    }









    public void BattleStart()
    {
        state = GameState.Start;
        Pause(GameState.Wait);

        skillTipBarController.InitSkillTipBarArea();

        Player.Instance.BattleStart();
        playerBattleInfo.init(Player.Instance);
        InputSequenceController.Instance.ResetAvailable();
        StartCoroutine(StateDelay());

    }






    public IEnumerator StateDelay()
    {
        Debug.Log("startdeley");
        yield return new WaitForSeconds(1f);
        RhythmController.Instance.Reset();

        Resume();
        //SuperController.Instance.state = GameState.Start;
    }






    public void ReadLevelDatas(LevelData newLevelData)
    {
        levelData = newLevelData;
        score = OneSongScore.ReadScoreData(levelData.scoreData);
        DuelController.Instance.enemyList = levelData.enemyList;
        SoundController.Instance.FMODMusicChange(SuperController.Instance.levelData.BGMPath);
    }

    public void Pause(GameState _state)
    {
        tempstate = state;
        Debug.Log("tempstate" + tempstate);
        state = _state;
        Debug.Log("state" + state);

        SoundController.Instance.FMODmusic.setPaused(true);
        SoundController.Instance.CalcDSPtime();
    }

    public void Resume()
    {
        state = tempstate;
        Debug.Log("state" + state);

        SoundController.Instance.FMODmusic.setPaused(false);
        SoundController.Instance.CalcDSPtime();

    }

    //一些UI的按节奏闪动
    public void Blink()
    {
        playerBattleInfo.Blink();
        enemyBattleInfo.Blink();
    }

    //
    public void ShowInputTip(string text, int type = 0)
    {
        GameObject tip = Instantiate(Resources.Load("Prefab/InputTip/InputTip"), InputTipPos.transform) as GameObject;
        tip.GetComponent<InputTip>().Init(text, type);
    }

    public void BgMaskTransition()
    {
        StartCoroutine(bgmaskeff());
    }

    IEnumerator bgmaskeff()
    {
        bgmask.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, .8f), .5f);
        yield return new WaitForSeconds(.5f);
        bgmask.GetComponent<SpriteRenderer>().DOColor(new Color(1, 1, 1, 0), .5f);
    }
}
