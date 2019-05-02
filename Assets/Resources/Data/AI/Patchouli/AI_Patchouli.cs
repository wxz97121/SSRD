using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patchouli : AI
{
    //记录当前阶段
    public int phaseID = 0;
    private int loopIndex;


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
        loopIndex=skillSequence.FindIndex(a => a == "---");
    }

    public override void Action(int beatnum)
    {
        base.Action(beatnum);

        Debug.Log("action id =" + actionID);
        switch (phaseID)
        {
            case 0:
                if (beatnum == 3)
                {
                    actionID++;
                    if (actionID >= loopIndex)
                    {
                        phaseID = 1;
                        actionID = loopIndex + 1;
                    }
                    break;
                }
                _skillDictionary[skillSequence[actionID]].EffectFunction(this);
                actionID++;
                if (actionID >= loopIndex)
                {
                    phaseID = 1;
                    actionID = loopIndex +1;
                }
                break;
            case 1:
                if(beatnum==3)
                {
                    actionID++;
                    if (actionID >= skillSequence.Count)
                        actionID = loopIndex + 1;
                    break;
                }
                _skillDictionary[skillSequence[actionID]].EffectFunction(this);
                actionID++;
                if (actionID >= skillSequence.Count)
                    actionID = loopIndex + 1;
                break;
        }


    }
}
