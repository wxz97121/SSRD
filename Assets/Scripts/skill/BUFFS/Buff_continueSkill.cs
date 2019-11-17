using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_continueSkill : Buff
{
    string skillName = "";

    public override void BuffAdded(Character p_chara,string str="")
    {


        m_name = "continueSkill";
        remainBeats = 5;


        //角色身上已经有本BUFF的情况,叠层

        Buff_continueSkill oldbuff = p_chara.buffs.FindLast(b => b.m_name== "continueSkill") as Buff_continueSkill;
        if (oldbuff != null)
        {
            if (oldbuff.skillName == str)
            {
                oldbuff.multicount++;
                oldbuff.remainBeats = 5;

            }
        }
        else
        {
            skillName = str;
            base.BuffAdded(p_chara, str);
            multicount = 1;
        }

    }

}
