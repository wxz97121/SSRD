using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimon2 : AI
{
    private int circleID = 0;
//    private bool isDisabled = false;
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

        if (beatnum == 3)
        {
            if (circleID >= 3)
            {
                float P = Random.value;
                if (P > 0.5)
                {
                    SGSAdd("attack");
                    circleID = 0;

                }
                else
                {
                    SGSAdd("Dattack");
                    SGSAdd("attack");
                    circleID = -1;

                }
            }
            else
            {
                if(skillGroupSeq.Count==actionID+1)
                    SGSAdd("idle");
                circleID++;
            }
            actionID++;


        }
        else
        {
//            Debug.Log("cast skill="+GetNextSkill(beatnum));
            CastSkill(GetNextSkill(beatnum));
        }
    }

    public override void Hit(int dDamage, bool noAfterattack = false, bool isCounterable = false)
    {
        base.Hit(dDamage, noAfterattack, isCounterable);
        if (isDefenced)
        {
            //.Break();
            SGSDelete(skillGroupSeq.Count - 1);
            SGSAdd("disable");
            //enemySkill.CommonEffect(this, "ANIT_disable");
            //animator.ResetTrigger("attack");
            isDefenced = false;
        }
    }
}
