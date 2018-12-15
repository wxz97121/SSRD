using System.Collections;
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
    public float barPosInBeats;

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


    // Use this for initialization
    void Start() {
        Debug.Log("uibar controller start!");
        currentBarList = new List<GameObject>();
        score = LevelData.Instance.score;

        InitBarArea();

    }

    // Update is called once per frame
    void Update() {
        barPosInBeats = RhythmController.Instance.songPosInBeats - playingBar.GetComponent<UIBar>().startBeat;
        BarMoving();
        BarSwitch();
    }

    //初始化
    public void InitBarArea()
    {
        pieceIndex = 0;
        barIndex = 0;
        occupiedBeats = 0;
        finishedBeats = 0;
        barPosInBeats =0;
        //SoundController.Instance.PlayBgMusic(score.bgmusic);

        //获取位置信息
        barPos0 = barPos0GO.transform.localPosition;
        barPos1 = barPos1GO.transform.localPosition;
        barPos2 = barPos2GO.transform.localPosition;
        barPos3 = barPos3GO.transform.localPosition;

        //连加三个新小节
        currentBarList.Add(CreateBarByScore(pieceIndex, barIndex));
        currentBarList[0].transform.localPosition = barPos1;
        //currentBarList[0].GetComponent<UIBar>().SetAlpha(0f);
        postBar = currentBarList[0];
        occupiedBeats = 0;
        Debug.Log("uibar 0 complete!");

        currentBarList.Add(CreateBarByScore(pieceIndex, barIndex));
        currentBarList[1].transform.localPosition = barPos2;
        playingBar = currentBarList[1];
        NextBar();

        Debug.Log("uibar 1 complete!");

        currentBarList.Add(CreateBarByScore(pieceIndex, barIndex));
        currentBarList[2].transform.localPosition = barPos3;
        currentBarList[2].GetComponent<UIBar>().SetAlpha(0f);
        preBar = currentBarList[2];

        NextBar();
        Debug.Log("uibar 2 complete!");


    }

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
        instBar.GetComponent<UIBar>().startBeat = occupiedBeats;
        Debug.Log("startbeat=" + instBar.GetComponent<UIBar>().startBeat);
        occupiedBeats += instBar.GetComponent<UIBar>().beatsThisBar;
        Debug.Log("occupiedBeats=" + occupiedBeats);

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
        Debug.Log("startbeat=" + uiBar.startBeat);
        occupiedBeats += uiBar.beatsThisBar;
        Debug.Log("occupiedBeats=" + occupiedBeats);
        uiBar.Init();
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
        barPosInBeats / playingBar.GetComponent<UIBar>().beatsThisBar
        );
        postBar.GetComponent<UIBar>().SetAlpha(a1);

        postBar.transform.localPosition = Vector2.Lerp(
        barPos1,
        barPos0,
        barPosInBeats / playingBar.GetComponent<UIBar>().beatsThisBar
        );

        //playing

        playingBar.transform.localPosition = Vector2.Lerp(
        barPos2,
        barPos1,
        barPosInBeats / playingBar.GetComponent<UIBar>().beatsThisBar
        );

        //pre
        float a2 = Mathf.Lerp(
        0,
        1,
        barPosInBeats / playingBar.GetComponent<UIBar>().beatsThisBar
        );
        preBar.GetComponent<UIBar>().SetAlpha(a2);

        preBar.transform.localPosition = Vector2.Lerp(
        barPos3,
        barPos2,
        barPosInBeats / playingBar.GetComponent<UIBar>().beatsThisBar
        );
    }
    #endregion


    //小节轮转换位
    public void BarSwitch()
    {
        if (playingBar.GetComponent<UIBar>().beatsThisBar< barPosInBeats)
        {
            Debug.Log("switch!!!");
            Debug.Log("beatsthisbar="+ playingBar.GetComponent<UIBar>().beatsThisBar);
            Debug.Log("barPosInBeats" + barPosInBeats);
            GameObject temp = postBar;
            //轮转换位
            postBar = playingBar;
            playingBar = preBar;
            preBar = temp;

            //读取下一小节
            preBar.GetComponent<UIBar>().Empty();
            InitBarByScore(pieceIndex, barIndex, preBar.GetComponent<UIBar>());
            preBar.transform.localPosition = barPos3;
            preBar.GetComponent<UIBar>().SetAlpha(0);
        }

    }

    //指针移动
    public void PinMoving()
    {

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
            else
            {
                barIndex++;
            }
        }
        //已经进行至main
        else
        {
            if (barIndex >= score.prelude.Count)
            {
                barIndex = 0;
            }
            else
            {
                barIndex++;
            }
        }
    }
    #endregion
}
