using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_defend : Buff
{
    public override void BuffAdded(Character p_chara)
    {
        base.BuffAdded(p_chara);

        //TODO :把播动画挪走！
        Player.Instance.animator.Play("player_defense", 0);

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
            Player.Instance.animator.Play("player_idle", 0);
        }
    }



}
