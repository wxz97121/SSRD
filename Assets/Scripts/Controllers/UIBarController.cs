﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarController : MonoBehaviour {

    //小节列表
    public List<GameObject> currentBarList;
    //谱子
    public OneSongScore score;
    //当前播放段落序号
    public int pieceIndex;
    //区域内当前小节序号
    public int barIndex;

    //已被读取小节占用的拍子数
    public float occupiedBeats;
    //已完成的小节的拍子数
    public float finishedBeats;

    //播放到当前小节的节拍数
    public float postBarPosInBeats;
    public float playingBarPosInBeats;

    public float preBarPosInBeats;

    public GameObject barPos0GO;
    public GameObject barPos1GO;
    public GameObject barPos2GO;
    public GameObject barPos3GO;

    [HideInInspector] public Vector3 barPos0;
    [HideInInspector] public Vector3 barPos1;
    [HideInInspector] public Vector3 barPos2;
    [HideInInspector] public Vector3 barPos3;

    [HideInInspector] public GameObject postBar;
    [HideInInspector] public GameObject playingBar;
    [HideInInspector] public GameObject preBar;

    public List<Note> currentEnergyNotes;


    #region 单例
    static UIBarController _instance;
    private void Awake()
    {
        _instance = this;
    }
    public static UIBarController Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion

    // Use this for initialization
    void Start() {
        Debug.Log("uibar controller start!");

        currentEnergyNotes = new List<Note>();
        currentBarList = new List<GameObject>();
        score = LevelData.Instance.score;

        InitBarArea();

    }

    // Update is called once per frame
    private void FixedUpdate() {
        postBarPosInBeats = RhythmController.Instance.songPosInBeats - postBar.GetComponent<UIBar>().startBeat;

        playingBarPosInBeats = RhythmController.Instance.songPosInBeats - playingBar.GetComponent<UIBar>().startBeat;

        preBarPosInBeats = RhythmController.Instance.songPosInBeats - preBar.GetComponent<UIBar>().startBeat;
        BarSwitch();

        BarPinMoving();

        BarMoving();

    }

    #region 初始化
    public void InitBarArea()
    {
        pieceIndex = 0;
        barIndex = 0;
        occupiedBeats = 0;
        finishedBeats = 0;
        playingBarPosInBeats =0;
        //SoundController.Instance.PlayBgMusic(score.bgmusic);

        //获取位置信息
        barPos0 = barPos0GO.transform.localPosition;
        barPos1 = barPos1GO.transform.localPosition;
        barPos2 = barPos2GO.transform.localPosition;
        barPos3 = barPos3GO.transform.localPosition;

        //连加三个新小节
        currentBarList.Add(CreateBarByScore(pieceIndex, barIndex));
        currentBarList[0].transform.localPosition = barPos1;

        postBar = currentBarList[0];
        postBar.GetComponent<UIBar>().Empty();
        postBar.GetComponent<UIBar>().active = false;
        occupiedBeats = 0;
        Debug.Log("uibar 0 complete!");

        currentBarList.Add(CreateBarByScore(pieceIndex, barIndex));
        currentBarList[1].transform.localPosition = barPos2;

        playingBar = currentBarList[1];
        currentEnergyNotes.AddRange(playingBar.GetComponent<UIBar>().noteList_energy);

        Debug.Log("uibar 1 complete!");

        currentBarList.Add(CreateBarByScore(pieceIndex, barIndex));
        currentBarList[2].transform.localPosition = barPos3;
        currentBarList[2].GetComponent<UIBar>().SetAlpha(0);
        preBar = currentBarList[2];
        currentEnergyNotes.AddRange(preBar.GetComponent<UIBar>().noteList_energy);

        NextBar();
        Debug.Log("uibar 2 complete!");


    }
    #endregion

    #region 新建一个UIBAR CreateBarByScore(int pieceIndex,int barIndex)
    public GameObject CreateBarByScore(int pieceIndex, int barIndex)
    {
        GameObject instBar = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar", typeof(GameObject)), transform);


        //判断当前所在的段落，读取单小节乐谱

        OneBarScore _barScore = pieceIndex > 0 ? score.mainlude[barIndex] : score.prelude[barIndex];


        //将音符收进两个轨道
        instBar.GetComponent<UIBar>().ReadScore(_barScore.notes);
        instBar.GetComponent<UIBar>().beatsThisBar = _barScore.beatsThisBar;
        instBar.GetComponent<UIBar>().Init();
        instBar.GetComponent<UIBar>().SetPinAlpha(0);
        instBar.GetComponent<UIBar>().active=true;

        instBar.GetComponent<UIBar>().startBeat = occupiedBeats;
        occupiedBeats += instBar.GetComponent<UIBar>().beatsThisBar;


        return instBar;
    }
    #endregion

    #region 读取一个新的BAR InitBarByScore(int pieceIndex,int barIndex)
    public void InitBarByScore(int pieceIndex, int barIndex,UIBar uiBar)
    {


        //判断当前所在的段落，读取单小节乐谱

        OneBarScore _barScore = pieceIndex > 0 ? score.mainlude[barIndex] : score.prelude[barIndex];


        uiBar.ReadScore(_barScore.notes);
        uiBar.beatsThisBar = _barScore.beatsThisBar;

        uiBar.startBeat = occupiedBeats;
//        Debug.Log("startbeat=" + uiBar.startBeat);
        occupiedBeats += uiBar.beatsThisBar;
 //       Debug.Log("occupiedBeats=" + occupiedBeats);
        uiBar.Init();
        uiBar.GetComponent<UIBar>().SetPinAlpha(0);
        uiBar.GetComponent<UIBar>().active=true;

        currentEnergyNotes.AddRange(uiBar.GetComponent<UIBar>().noteList_energy);
    }
    #endregion

    #region 小节整体上移
    public void BarMoving()
    {
        //Debug.Log(RhythmController.Instance.songPosInBeats);
        //post
        float a1 = Mathf.Lerp(
        1,
        0,
        playingBarPosInBeats / playingBar.GetComponent<UIBar>().beatsThisBar
        );
        postBar.GetComponent<UIBar>().SetAlpha(a1);

        postBar.transform.localPosition = Vector2.Lerp(
        barPos1,
        barPos0,
        playingBarPosInBeats / playingBar.GetComponent<UIBar>().beatsThisBar
        );

        //playing

        playingBar.transform.localPosition = Vector2.Lerp(
        barPos2,
        barPos1,
        playingBarPosInBeats / playingBar.GetComponent<UIBar>().beatsThisBar
        );

        //pre
        float a2 = Mathf.Lerp(
        0,
        1,
        playingBarPosInBeats / playingBar.GetComponent<UIBar>().beatsThisBar
        );
        preBar.GetComponent<UIBar>().SetAlpha(a2);

        preBar.transform.localPosition = Vector2.Lerp(
        barPos3,
        barPos2,
        playingBarPosInBeats / playingBar.GetComponent<UIBar>().beatsThisBar
        );
    }
    #endregion


    #region 小节轮转换位BAR SWITCH
    public void BarSwitch()
    {
        if (playingBar.GetComponent<UIBar>().beatsThisBar< playingBarPosInBeats)
        {
            RhythmController.Instance.NewBarInit();

            finishedBeats += postBar.GetComponent<UIBar>().beatsThisBar;
//            Debug.Log("finished:"+ finishedBeats);
            //轮转换位
            GameObject temp = postBar;
            postBar = playingBar;
            playingBar = preBar;
            preBar = temp;

            NextBar();

            //读取下一小节
            preBar.GetComponent<UIBar>().Empty();
            InitBarByScore(pieceIndex, barIndex, preBar.GetComponent<UIBar>());
            preBar.transform.localPosition = barPos3;
            preBar.GetComponent<UIBar>().SetAlpha(0);

            //处理指针
            //postBar.GetComponent<UIBar>().SetPinAlpha(0);
            //preBar.GetComponent<UIBar>().SetPinAlpha(0);
            //playingBar.GetComponent<UIBar>().PinMoving(0);
            //playingBar.GetComponent<UIBar>().SetPinAlpha(255);


        }

    }
    #endregion


    //指针移动
    public void BarPinMoving()
    {
        postBarPosInBeats = RhythmController.Instance.songPosInBeats - postBar.GetComponent<UIBar>().startBeat;
        postBar.GetComponent<UIBar>().PinMoving(postBarPosInBeats);

        playingBarPosInBeats = RhythmController.Instance.songPosInBeats - playingBar.GetComponent<UIBar>().startBeat;
        playingBar.GetComponent<UIBar>().PinMoving(playingBarPosInBeats);

        preBarPosInBeats = RhythmController.Instance.songPosInBeats - preBar.GetComponent<UIBar>().startBeat;
        preBar.GetComponent<UIBar>().PinMoving(preBarPosInBeats);
    }

    //移除NOTE
    public void RemoveNote()
    {

    }



    #region 计算下一个小节的位置 NextBar（）
    private void NextBar()
    {
        barIndex++;
        //还在PRELUDE中的情况
        if(pieceIndex==0)
        {
            if (barIndex >= score.prelude.Count)
            {
                pieceIndex++;
                barIndex = 0;
            }

        }else if (barIndex >= score.mainlude.Count)
            {
                barIndex = 0;
            }


    }
    #endregion
}