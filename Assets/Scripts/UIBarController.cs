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

    //已被占用的拍子数
    public float occupiedBeats;

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
        BarMoving();

    }

    //初始化
    public void InitBarArea()
    {
        pieceIndex = 0;
        barIndex = 0;
        occupiedBeats = 0;
        //SoundController.Instance.PlayBgMusic(score.bgmusic);

        //获取位置信息
        barPos0 = barPos0GO.transform.localPosition;
        barPos1 = barPos1GO.transform.localPosition;
        barPos2 = barPos2GO.transform.localPosition;
        barPos3 = barPos3GO.transform.localPosition;

        //连加三个新小节
        currentBarList.Add(InitBarByScore(pieceIndex, barIndex));
        currentBarList[0].transform.localPosition = barPos1;
        //currentBarList[0].GetComponent<UIBar>().SetAlpha(0f);
        postBar = currentBarList[0];
        Debug.Log("uibar 0 complete!");

        currentBarList.Add(InitBarByScore(pieceIndex, barIndex));
        currentBarList[1].transform.localPosition = barPos2;
        playingBar = currentBarList[1];
        NextBar();
        playingBar.GetComponent<UIBar>().startBeat = occupiedBeats;
        occupiedBeats += playingBar.GetComponent<UIBar>().beatsThisBar;
        Debug.Log("uibar 1 complete!");

        currentBarList.Add(InitBarByScore(pieceIndex, barIndex));
        currentBarList[2].transform.localPosition = barPos3;
        currentBarList[2].GetComponent<UIBar>().SetAlpha(0f);
        preBar = currentBarList[2];
        preBar.GetComponent<UIBar>().startBeat = occupiedBeats;

        occupiedBeats += preBar.GetComponent<UIBar>().beatsThisBar;

        NextBar();
        Debug.Log("uibar 2 complete!");


    }

    #region 读取一个新的BAR InitBarByScore(int pieceIndex,int barIndex)
    public GameObject InitBarByScore(int pieceIndex, int barIndex)
    {
        GameObject instBar = Instantiate((GameObject)Resources.Load("Prefab/UI/Bar/UI_Bar", typeof(GameObject)), transform);


        //判断当前所在的段落，读取单小节乐谱

        OneBarScore _barScore = pieceIndex > 0 ? score.mainlude[barIndex] : score.prelude[barIndex];


        //将音符收进两个轨道
        foreach (Note note in _barScore.notes)
        {
            if ((int)note.type <= 8)
            {

                instBar.GetComponent<UIBar>().noteList_energy.Add(new Note
                {
                    type = note.type,
                    beat = note.beat
                }
                );
            }

        }
        instBar.GetComponent<UIBar>().beatsThisBar = _barScore.beatsThisBar;
        instBar.GetComponent<UIBar>().Init();
        return instBar;
    }
    #endregion

    //小节整体上移
    public void BarMoving()
    {
        //Debug.Log(RhythmController.Instance.songPosInBeats);
        //post
        float a1 = Mathf.Lerp(
        1,
        0,
        (RhythmController.Instance.songPosInBeats - playingBar.GetComponent<UIBar>().startBeat) / playingBar.GetComponent<UIBar>().beatsThisBar
        );
        postBar.GetComponent<UIBar>().SetAlpha(a1);

        postBar.transform.localPosition = Vector2.Lerp(
        barPos1,
        barPos0,
        (RhythmController.Instance.songPosInBeats - playingBar.GetComponent<UIBar>().startBeat) / playingBar.GetComponent<UIBar>().beatsThisBar
        );

        //playing

        playingBar.transform.localPosition = Vector2.Lerp(
        barPos2,
        barPos1,
        (RhythmController.Instance.songPosInBeats - playingBar.GetComponent<UIBar>().startBeat) / playingBar.GetComponent<UIBar>().beatsThisBar
        );

        //pre
        float a2 = Mathf.Lerp(
        0,
        1,
        (RhythmController.Instance.songPosInBeats - playingBar.GetComponent<UIBar>().startBeat) / playingBar.GetComponent<UIBar>().beatsThisBar
        );
        preBar.GetComponent<UIBar>().SetAlpha(a2);

        preBar.transform.localPosition = Vector2.Lerp(
        barPos3,
        barPos2,
        (RhythmController.Instance.songPosInBeats - playingBar.GetComponent<UIBar>().startBeat) / playingBar.GetComponent<UIBar>().beatsThisBar
        );
    }


    //小节轮转换位
    public void BarSwitch()
    {

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
