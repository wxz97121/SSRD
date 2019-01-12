﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff_defend : Buff
{
    public override void BuffAdded()
    {
        base.BuffAdded();

        //TODO :把播动画挪走！
        Player.Instance.animator.Play("player_defense", 0);

    }

    public override void BuffBeat(int beatNum)
    {
        base.BuffBeat(beatNum);
        if (remainBeats == 0)
        {
            Player.Instance.animator.Play("player_idle", 0);
        }

    }

}
