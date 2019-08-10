using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_regenEnergy : Buff
{

    public override void BuffAdded(Character p_chara,string str="")
    {
        m_name = "regenEnergy";
        remainBeats = -1;

        //角色身上已经有本BUFF的情况
        if (p_chara.hasBuff<Buff_autoenergy>())
        {
            return;
        }

        base.BuffAdded(p_chara,str);

        multicount = -1;
    }


    public override void BuffBeat(int beatNum)
    {
        base.BuffBeat(beatNum);
        if (beatNum == 0)
        {
            character.AddMp(4);

        }
    }
}
