using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Wizard : AI
{
    //记录当前阶段
    public int phaseID = 0;
    private int InnerActionID = 0;
    private int firstloopIndex;
    private int secondloopIndex;


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
        firstloopIndex = skillSequence.FindIndex(a => a == "firstloop");

        secondloopIndex = skillSequence.FindIndex(a => a == "secondloop");
    }

    public override void Action(int beatnum)
    {
        base.Action(beatnum);


        switch (phaseID)
        {
            case 0:


                if(beatnum!=3) _skillDictionary[skillSequence[actionID]].EffectFunction(this);
                actionID++;
                if (actionID >= firstloopIndex)
                {
                    phaseID = 1;
                    actionID = firstloopIndex +1;
                }
                InnerActionID = actionID;
                break;
            case 1:
                if (Hp <= 15)
                {
                    if (beatnum != 3) _skillDictionary[skillSequence[0]].EffectFunction(this);
                    actionID++;
                    if ((actionID - firstloopIndex) % 4 == 1)
                    {
                        phaseID = 2;
                        actionID = secondloopIndex + 1;
                    }
                    break;
                }



                if (beatnum != 3) _skillDictionary[skillSequence[actionID]].EffectFunction(this);
                actionID++;
                if (actionID >= secondloopIndex)
                    actionID = firstloopIndex + 1;

                InnerActionID = actionID;

                break;
            case 2:
                Debug.Log("action ID:" + actionID);
                Debug.Log("time:" + RhythmController.Instance.songPosInBeats);
                if (beatnum != 3) _skillDictionary[skillSequence[actionID]].EffectFunction(this);
                actionID++;
                if (actionID >= skillSequence.Count)
                    actionID = secondloopIndex + 1;

                InnerActionID = actionID;

                break;
        }



    }
}
