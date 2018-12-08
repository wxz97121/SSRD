using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIBarController : MonoBehaviour {

    //小节列表
    public List<UIBar> currentBarList;
    //谱子
    public OneSongScore score;
    //当前播放段落序号
    public int pieceIndex;
    //区域内当前小节序号
    public int barIndex;


    // Use this for initialization
    void Start () {
        Debug.Log("uibar controller start!");
        currentBarList = new List<UIBar>();
        score =ReadScoreData("score_1_test");

        InitBarArea();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    //初始化
    public void InitBarArea()
    {
        pieceIndex = 0;
        barIndex = 0;
        SoundController.Instance.PlayBgMusic(score.bgmusic);

        //连加三个新小节
        currentBarList.Add(InitBarByScore(pieceIndex, barIndex));
        NextBar();
        currentBarList.Add(InitBarByScore(pieceIndex, barIndex));
        NextBar();
        currentBarList.Add(InitBarByScore(pieceIndex, barIndex));
        NextBar();


    }

    #region 读取一个新的BAR InitBarByScore(int pieceIndex,int barIndex)
    public UIBar InitBarByScore(int pieceIndex,int barIndex)
    {
        UIBar _uIBar=new UIBar();

        //判断当前所在的段落，读取单小节乐谱

        OneBarScore _barScore = pieceIndex > 0 ? score.mainlude[barIndex] : score.prelude[barIndex];


        //将音符收进两个轨道
        foreach (Note note in _barScore.notes)
        {
            if ((int)note.type<=8)
            {
                _uIBar.noteList_energy.Add(note);
            }
            else
            {
                _uIBar.noteList_main.Add(note);

            }
        }
        _uIBar.length = _barScore.beatsThisBar;
        return _uIBar;
    }
    #endregion

    //小节整体上移
    public void BarMoving()
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

    # region 写入score ReadScoreData(string scorename)
    public OneSongScore ReadScoreData(string scorename)
    {
        Debug.Log("start reading score");

        OneSongScore _score = new OneSongScore();
        ScoreData data = Resources.Load("Data/Score/" + scorename) as ScoreData;

        _score.mainlude = data.mainlude;
        _score.prelude = data.prelude;
        _score.bgmusic = data.bgmusic;

        return _score;
    }
    #endregion

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
