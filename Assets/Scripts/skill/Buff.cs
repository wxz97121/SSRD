using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[System.Serializable]
public class Buff
{
    public string m_name;
    //持续节拍数,-1为永久
    public int remainBeats;
    //buff层数，-1为不叠加
    public int multicount = -1;
    //buff是否有显示
    public bool isDisplay = false;
    //宿主
    public Character character;

    public float activateTime;

    //BUFF提供的伤害加成
    public int damageAdd = 0;

    //BUFF提供的伤害加倍
    public float damageMulti = 1;

    public virtual void BuffAdded(Character p_chara,string str="")
    {
        p_chara.buffs.Add(this);
        character = p_chara;
    }

    public virtual void BuffBeat(int beatNum)
    {


    }

    //buff层数衰减
    public virtual void BuffDecay()
    {
        if (remainBeats > 0 && (RhythmController.Instance.songPosInBeats - activateTime >= 1 - RhythmController.Instance.commentGoodTime))
        {
            remainBeats--;
            //具体BUFF消失在Character类的BuffsDecay中实现

        }
    }

    public virtual void BuffRemove()
    {


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

    //被攻击之前
    public virtual void BeforeAttacked(Character p_chara)
    {

    }

    //被攻击之后
    public virtual void AfterAttacked(Character p_chara)
    {

    }

    public virtual void PlayerUpdate()
    {

    }

    public virtual void AfterNoAction()
    {

    }

    public virtual void GainEnergy()
    {

    }

    public virtual void MissEnergy()
    {

    }

    public virtual void WhenDie(Character p_chara)
    {

    }

}
