using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//防护罩
public class Buff_playerBarrier : Buff
{
    public override void BuffAdded(Character p_chara,string str="")
    {
        //角色身上已经有本BUFF的情况
        if (p_chara.hasBuff<Buff_playerBarrier>())
        {
            return;
        }
        base.BuffAdded(p_chara,str);

        multicount = -1;
        remainBeats = -1;
        //TODO :添加防护罩动画

    }

    public override void BuffBeat(int beatNum)
    {
        base.BuffBeat(beatNum);
        //

    }



    public override void BeforeAttacked(Character p_chara)
    {
        base.BeforeAttacked(p_chara);
        //TODO:被攻击则消失，抵抗攻击
    }

}
