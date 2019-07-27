using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_defend : Buff
{

    public string EffStr;
    public override void BuffAdded(Character p_chara,string str)
    {
        m_name = "defend";

        base.BuffAdded(p_chara,str);

        EffStr = str.Replace('$','_');



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
            //Player.Instance.animator.Play("player-idle", 0);
        }
    }



}
