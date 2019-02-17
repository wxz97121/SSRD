using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_QTEtest : AI
{
    //记录当前阶段
    public int phaseID = 0;
    private int InnerActionID = 0;
    private int QTEIndex;

    public QTEScoreData qTEScoreData;
    public OneSongScore qtescore;

    override protected void Update()
    {
        base.Update();
    }

    override protected void Start()
    {
        base.Start();
    }

    public override void Init()
    {
        base.Init();

        QTEIndex = skillSequence.FindIndex(a => a == "QTE");
        qtescore = OneSongScore.ReadQTEScoreData(qTEScoreData);

    }

    public override void Action()
    {
        base.Action();


        switch (phaseID)
        {
            case 0:
                _skillDictionary[skillSequence[actionID]].EffectFunction(this);
                SoundController.Instance.FMODSetParameter("boss", 0);
                SoundController.Instance.FMODSetParameter("chorus", 0);
                SoundController.Instance.FMODSetParameter("verse", 0);
                SoundController.Instance.FMODSetParameter("breakdown", 1);
                SoundController.Instance.FMODSetParameter("outro", 0);
                actionID++;
                if (actionID >= QTEIndex)
                {
                    //Debug.Log(SoundController.Instance.GetLastMarker());
                    if (SoundController.Instance.GetLastMarker()=="minibridge"||SoundController.Instance.GetLastMarker()=="breakdown")
                    {
                        UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.playingBar.GetComponent<UIBar>(), qtescore.QTEscore[0].notes);
                        UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.preBar.GetComponent<UIBar>(), qtescore.QTEscore[1].notes);
                        SuperController.Instance.state = GameState.QTE;
                        UIBarController.Instance.QTEscore = qtescore;
                        UIBarController.Instance.QTEbarIndex = 1;
                        actionID = 0;
                        phaseID = 1;
                        //    Debug.Log("change qte mode complete");

                        SoundController.Instance.FMODSetParameter("boss", 0);
                        SoundController.Instance.FMODSetParameter("chorus", 0);
                        SoundController.Instance.FMODSetParameter("verse",1);
                        SoundController.Instance.FMODSetParameter("breakdown", 0);
                        SoundController.Instance.FMODSetParameter("outro", 0);
                    }
                    else
                    {
                        actionID = 3;

                    }

                }
                break;
            case 1:
             
                break;
        }



    }
}
