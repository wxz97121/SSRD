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
        _score.mainlude = new List<OneBarScore>();
        _score.prelude = new List<OneBarScore>();

        ScoreData data = Resources.Load("Data/Score/" + scorename) as ScoreData;


        //_score.mainlude = data.mainlude;
        for(int i = 0; i < data.mainlude.Count; i++)
        {
            OneBarScore _onebarscore = new OneBarScore
            {
                beatsThisBar = data.mainlude[i].beatsThisBar,
                notes=new List<Note>()
            };
            for (int j = 0; j < data.mainlude[i].notes.Count;j++)
            {
                _onebarscore.notes.Add(new Note
                {
                    type = data.mainlude[i].notes[j].type,
                    //_note.note = new GameObject();
                    beat = data.mainlude[i].notes[j].beat
                });
//                Debug.Log("added mainlude note ");
            }
            _score.mainlude.Add(_onebarscore);

        }

        //_score.prelude = data.prelude;

        for (int i = 0; i < data.prelude.Count; i++)
        {
            OneBarScore _onebarscore = new OneBarScore
            {
                beatsThisBar = data.prelude[i].beatsThisBar,
                notes = new List<Note>()
            };
            for (int j = 0; j < data.prelude[i].notes.Count; j++)
            {
                _onebarscore.notes.Add(new Note
                {
                    type = data.prelude[i].notes[j].type,
                    //_note.note = data.mainlude[j].notes[j].note;
                    beat = data.prelude[i].notes[j].beat
                });
            }
            _score.prelude.Add(_onebarscore);

        }

        _score.bgmusic = data.bgmusic;

        return _score;
    }
    #endregion
}
