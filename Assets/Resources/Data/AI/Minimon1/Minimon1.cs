using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimon1 : AI
{
    //记录循环的函数
    private int circleID = 0;
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
        SGSAdd("idle");
    }

    public override void Action(int beatnum)
    {
        base.Action(beatnum);
        //_skillDictionary[skillSequence[actionID]].EffectFunction(this);
        //actionID++;
        //if (actionID >= skillSequence.Count)
        //actionID = 0;
        if (beatnum == 3)
        {
            if (circleID >= 3)
            {
                SGSAdd("attack");
                circleID = 0;
            }
            else
            {
                SGSAdd("idle");
                circleID++;
            }
            actionID++;

        }
        else
        {
//            Debug.Log("actionID = " + actionID);
//            Debug.Log("seq count " + skillGroupSeq.Count);

//            Debug.Log("GetNextSkill(beatnum)"+GetNextSkill(beatnum));
            CastSkill(GetNextSkill(beatnum));


        }
    }
}
