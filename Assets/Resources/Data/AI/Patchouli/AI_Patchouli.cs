using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI_Patchouli : AI
{
    //记录当前阶段
    public int phaseID = 0;
    private int introIndex=0;
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

        Debug.Log("loopIndex=" + loopIndex);
    }

    public override void Action()
    {
        base.Action();


        switch (phaseID)
        {
            case 0:
                _skillDictionary[skillSequence[actionID]].EffectFunction(this);
                actionID++;
                if (actionID >= loopIndex)
                {
                    phaseID = 1;
                    actionID = loopIndex +1;
                }
                break;
            case 1:
                _skillDictionary[skillSequence[actionID]].EffectFunction(this);
                actionID++;
                if (actionID >= skillSequence.Count)
                    actionID = loopIndex + 1;
                break;
        }


    }
}
