using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Buff
{
    public string m_name;
    //持续节拍数,-1为永久
    public int remainBeats;
    //buff层数，-1为不叠加
    public int multicount = -1;
    //宿主
    public Character character;

    public float activateTime;

    //BUFF提供的伤害加成
    public int damageAdd = 0;

    //BUFF提供的伤害加倍
    public float damageMulti = 1;

    public virtual void BuffAdded(Character p_chara)
    {
        p_chara.buffs.Add(this);
        character = p_chara;
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

    //左手输入错误时
    public virtual void LeftHandBad(Character p_chara)
    {

    }

    //右手输入错误时
    public virtual void RightHandBad(Character p_chara)
    {

    }

    //攻击之后
    public virtual void AfterAttack(Character p_chara)
    {

    }

    //被攻击之后
    public virtual void AfterAttacked(Character p_chara)
    {

    }

    public virtual void PlayerUpdate()
    {

    }

}
