﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Minimon3 : AI
{
//    private int circleID = 0;
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
            if (skillGroupSeq.Count == 0)
            {
                float P = Random.value;
                if (P > 0.9)
                {
                    SGSAdd("attack");

                }
                else if (P > 0.7)
                {
                    SGSAdd("Dattack");
                    SGSAdd("attack");

                }
                else if (P > 0.3)
                {
                    SGSAdd("defend");

                }
                else
                {
                    SGSAdd("idle");
                }
            }


        }
        else
        {
            //            Debug.Log("cast skill="+GetNextSkill(beatnum));
            CastSkill(GetNextSkill(beatnum));
        }

    }


    public override void Hit(int dDamage, bool noAfterattack = false, bool isDefenceToDisable = false, bool isDefencePenetrate = false, string sfxstr = "SLASH")
    {
        base.Hit(dDamage, noAfterattack, isDefenceToDisable, isDefencePenetrate, sfxstr);
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
