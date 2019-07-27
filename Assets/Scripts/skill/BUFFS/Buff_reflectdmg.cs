using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_reflectdmg : Buff
{

    public override void BuffAdded(Character p_chara,string str="")
    {
        m_name = "autoenergy";
        remainBeats = -1;

        //角色身上已经有本BUFF的情况
        if (p_chara.hasBuff<Buff_reflectdmg>())
        {
            return;
        }

        base.BuffAdded(p_chara,str);

        multicount = -1;
    }


}
