using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_yiWenZiZhan : Buff
{

    public string EffStr;
    public override void BuffAdded(Character p_chara,string str)
    {
        m_name = "yiwenzizhan";

        base.BuffAdded(p_chara,str);




    }

    public override void BuffBeat(int beatNum)
    {
        base.BuffBeat(beatNum);


    }

    public override void BuffDecay()
    {
        base.BuffDecay();
        if (remainBeats == 0)
        {
            //Player.Instance.CastSkill(EffStr);
            //Player.Instance.animator.Play("player-idle", 0);
        }
    }



}
