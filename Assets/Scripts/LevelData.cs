using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelData : MonoBehaviour {
    //谱子
    public OneSongScore score;

    #region 单例
    static LevelData _instance;
    private void Awake()
    {
        _instance = this;

        ReadDatas();
    }
    public static LevelData Instance
    {
        get
        {
            return _instance;
        }
    }
    #endregion
    // Use this for initialization
    void ReadDatas() {
        score = ReadScoreData("score_1_test");

    }

    // Update is called once per frame
    void Update () {
		
	}

    #region 写入score ReadScoreData(string scorename)
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
}
