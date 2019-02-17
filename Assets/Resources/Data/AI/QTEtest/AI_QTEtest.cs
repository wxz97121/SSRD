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
                actionID++;
                if (actionID >= QTEIndex)
                {
                    UIBarController.Instance.TurnBarIntoQTE(UIBarController.Instance.preBar.GetComponent<UIBar>(),qtescore.QTEscore[1].notes);
                    actionID =  0;
                    phaseID = 1;
                }
                break;
            case 1:
             
                break;
        }



    }
}
