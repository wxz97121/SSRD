using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public string m_name;
    //持续节拍数,-1为永久
    public int remainBeats;
    //宿主
    public Character character;

    public float activateTime;

    public virtual void BuffAdded()
    {
    }

    public virtual void BuffBeat(int beatNum)
    {


    }

    public virtual void BuffDecay()
    {
        if (remainBeats > 0 && (RhythmController.Instance.songPosInBeats - activateTime >= 1 - RhythmController.Instance.commentGoodTime))
        {
            remainBeats--;
            //            Debug.Log("BuffBeat,remainBeat="+ remainBeats);
        }
    }

}
