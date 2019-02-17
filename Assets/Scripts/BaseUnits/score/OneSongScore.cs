﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneSongScore
{
    public List<OneBarScore> prelude;
    public List<OneBarScore> mainlude;

    //QTE专用
    public List<OneBarScore> QTEscore;

    #region 写入score ReadScoreData(scorename)
    public static OneSongScore ReadScoreData(ScoreData data)
    {
        if (!data)
        {
            Debug.Log("没找到SCORE DATA");
            Debug.Break();
        }
        Debug.Log("start reading score");

        OneSongScore _score = new OneSongScore
        {
            mainlude = new List<OneBarScore>(),
            prelude = new List<OneBarScore>()
        };



        //_score.mainlude = data.mainlude;
        for (int i = 0; i < data.mainlude.Count; i++)
        {
            OneBarScore _onebarscore = new OneBarScore
            {
                beatsThisBar = data.mainlude[i].beatsThisBar,
                notes = new List<Note>()
            };
            for (int j = 0; j < data.mainlude[i].notes.Count; j++)
            {
                _onebarscore.notes.Add(new Note
                {
                    type = data.mainlude[i].notes[j].type,
                    beatInBar = data.mainlude[i].notes[j].beatInBar
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
                    beatInBar = data.prelude[i].notes[j].beatInBar
                });
            }
            _score.prelude.Add(_onebarscore);

        }


        return _score;
    }
    #endregion



    #region 写入QTE SCORE ReadQTEScoreData(scorename)
    public static OneSongScore ReadQTEScoreData(ScoreData data)
    {
        if (!data)
        {
            Debug.Log("没找到QTE SCORE DATA");
            Debug.Break();
        }
        Debug.Log("start reading QTE score");

        OneSongScore _score = new OneSongScore
        {
            QTEscore = new List<OneBarScore>()
        };



        //_score.mainlude = data.mainlude;
        for (int i = 0; i < data.mainlude.Count; i++)
        {
            OneBarScore _onebarscore = new OneBarScore
            {
                beatsThisBar = data.mainlude[i].beatsThisBar,
                notes = new List<Note>()
            };
            for (int j = 0; j < data.mainlude[i].notes.Count; j++)
            {
                _onebarscore.notes.Add(new Note
                {
                    type = data.mainlude[i].notes[j].type,
                    beatInBar = data.mainlude[i].notes[j].beatInBar
                });
                //                Debug.Log("added mainlude note ");
            }
            _score.mainlude.Add(_onebarscore);

        }



        return _score;
    }
    #endregion
}
