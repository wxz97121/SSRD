﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

using TMPro;

//角色基类，控制玩家和敌人的全部属性和行动
public class Character : MonoBehaviour
{
    public bool isPlayer;

    public int maxHp = 3;
    public int Hp = 3;

    public int maxLife = 10;
    public int life =10;

    public int maxMp = 5;
    public int Mp = 0;

    //基础攻击力
    public int ATK = 2;

    //基础防御力
    public int DEF = 0;

    public bool isUndead = false;
    //被精防
    public bool isDefenced = false;
    //被打断
    public bool isBroken = false;
    //当前可以被打断
    public bool isBreakable = false;

    //魂点数
    public int soulPoint = 0;
    //魂到达多少点
    public int soulMaxPoint = 10;

    //当前BUFF列表
    public List<Buff> buffs = new List<Buff>();


    //当前攻击的目标
    public GameObject mTarget;


    //这一拍有没有被击中
    //public bool getHit = false;

    //蓄力动画
    //public GameObject chargeVfx;
    // Use this for initialization
    virtual protected void Start()
    {

    }

    // Update is called once per frame
    virtual protected void Update()
    {
       // UIHpNum.SetText(Hp.ToString());
       //UIMpNum.SetText(Mp.ToString());
        //UpdateInput();
        UpdateBuff();
    }

    //virtual protected void UpdateInput()
    //{

    //}
    //每次行动之前的初始化:判断蓄力是否断，清空护盾和被击
    //virtual public void Initialize()
    //{


    //    getHit = false;



    //}
    //战斗开始
    public virtual void BattleStart()
    {

    }

    //加灵力
    virtual public void AddMp(int dMp)
    {
        if (Mp + dMp >= maxMp)
        {
            Mp = maxMp;
        }
        else
        {
            Mp += dMp;
        }
    }

    virtual public void Hit(int dDamage, bool noAfterattack=false,bool isDefenceToDisable= false,bool isDefencePenetrate = false,string sfxstr="SLASH")
    {
        //普通攻击目标
        if (mTarget != null)
        {
            Character cTarget = mTarget.GetComponent<Character>();
            //判断对方护盾
            if (cTarget.hasBuff<Buff_defend>())
            {
                //破防
                if (isDefencePenetrate)
                {
                    //todo:通用的破防挨打动画，破防音效
                    SoundController.Instance.PlayAudioEffect(sfxstr);

                    cTarget.Damage(CalcDmg(getCurrentATK(), cTarget.getCurrentDEF(), dDamage), this);
                    if (cTarget.isPlayer)
                    {
                        Camera.main.gameObject.transform.DOShakePosition(1, 0.5f);

                    }
                }
                else
                {
                    GameObject fxClone = Instantiate(Resources.Load("VFX/Shield"), cTarget.transform.Find("pos_defendfx").transform.position, Quaternion.identity) as GameObject;
                    fxClone.transform.localScale = cTarget.transform.Find("pos_defendfx").transform.localScale;

                    //Debug.Log("ISCOUNTERABLE "+isCounterable.ToString());
                    if (isDefenceToDisable)
                    {
                        //Debug.Break();
                        SoundController.Instance.PlayAudioEffect("PDEFEND");
                        isDefenced = true;
                    }
                    else
                    {
                        SoundController.Instance.PlayAudioEffect("DEFEND");

                    }

                    //释放防御成功带的技能
                    cTarget.CastSkill((cTarget.buffs.Find((Buff obj) => obj.m_name == "defend") as Buff_defend).EffStr);

                }


            }
            else
            {
                //todo:通用的挨打动画
                cTarget.GetComponent<VFX>().StartCoroutine("GetHit");
                SoundController.Instance.PlayAudioEffect(sfxstr);
                
                cTarget.Damage(CalcDmg(getCurrentATK(), cTarget.getCurrentDEF(), dDamage), this);
                if (cTarget.isPlayer)
                {
                    Camera.main.gameObject.transform.DOShakePosition(1,0.5f);

                }



            }
            //执行BUFF中的攻击结束效果
            if (!noAfterattack)
            {
                List<Buff> tempbuffs = new List<Buff>();
                tempbuffs.AddRange(buffs);
                foreach (Buff b in tempbuffs)
                {

                    b.AfterAttack(this);
                }
            }
        }

        //        lastAction = actionType.Hit;
    }
    /*
    virtual public bool Hit(int dDamage,bool nothaveAfterattack)
    {
        //普通攻击目标
        if (mTarget != null)
        {
            Character cTarget = mTarget.GetComponent<Character>();
            //判断对方护盾
            if (cTarget.hasBuff<Buff_oneTimeShield>())
            {
                cTarget.transform.Find("oneTimeShield").GetComponent<VFX>().StartCoroutine("FadeOutLarger");
                cTarget.RemoveBuff<Buff_oneTimeShield>();
                return false;
            }
            else if (cTarget.hasBuff<Buff_defend>())
            {
                Instantiate(Resources.Load("VFX/Shield"), cTarget.transform.position, Quaternion.identity);
                return false;
            }
            else
            {
   

                Instantiate(Resources.Load("VFX/Slash"), cTarget.transform.position, Quaternion.identity);
                cTarget.Damage(CalcDmg(getCurrentATK(), cTarget.getCurrentDEF(), dDamage), this);

                return true;
            }
        }

        //        lastAction = actionType.Hit;
        return true;
    }
    */




    virtual public void Heal(int dHeal)
    {
        Hp = Hp + dHeal;
        if (Hp>maxHp)
        {
            Hp = maxHp;
        }
        if(dHeal>0)Bubble.AddBubble(BubbleSprType.hp, "+" + dHeal.ToString(), this, true);
    }

    virtual public void HealLife(int dHeal)
    {
        life = life + dHeal;
        if (life > maxLife)
        {
            life = maxLife;
        }
        if (dHeal > 0) Bubble.AddBubble(BubbleSprType.life, "+" + dHeal.ToString(), this, true);

    }

    //死亡
    virtual public void Die()
    {
    }
    //受到伤害(以后名字要改下)
    virtual public void Damage(int dDamage, Character source)
    {
        //反甲Buff_reflectdmg判定
        if (hasBuff<Buff_reflectdmg>())
        {
            if (!source.hasBuff<Buff_reflectdmg>())
            {
                source.Damage(dDamage, this);
                //break;
            }
        }

        //触发被打断效果
        if(isBreakable)Broken();


        //getHit = true;
        if (Hp >= dDamage)
        {
            Hp -= dDamage;
            Bubble.AddBubble(BubbleSprType.hp, "-" + dDamage.ToString(), this);

        }
        else
        {
            if(Hp>0)Bubble.AddBubble(BubbleSprType.hp, "-" + Hp.ToString(), this);

            int lifeDamage = dDamage - Hp;
            if (lifeDamage > 0) Bubble.AddBubble(BubbleSprType.life, "-" + lifeDamage.ToString(), this);

            Hp = 0;

            life -= lifeDamage;
            if (life <= 0)
            {
                life = 0;
            }
            // Die();
        }

        List<Buff> tempbuffs = new List<Buff>();
        tempbuffs.AddRange(buffs);
        foreach (Buff b in tempbuffs)
        {

            b.AfterAttacked(this);
        }
    }

    //被打断
    public virtual void Broken()
    {
        isBroken = true;
        SoundController.Instance.PlayAudioEffect("Break");
        SuperController.Instance.ShowInputTip("BREAK!", 1);
        //TODO:增加打断特效
    }

    //计算当前攻击力
    virtual public int getCurrentATK()
    {
        return ATK;
    }

    //计算当前防御力
    virtual public int getCurrentDEF()
    {
        return DEF;

    }


    //简单的伤害计算
    public int CalcDmg(int dATK, int dDEF, int dDamage)
    {
        //print("NOW ATK " + dATK.ToString() + " " + " NOW DEF " + dDEF.ToString() + " DMG " + dDamage.ToString());
        int DMG;
        dATK += dDamage;
        foreach (Buff B in buffs) 
        {
            dATK += B.damageAdd;
        }
        float tempATK = dATK;
        foreach (Buff B in buffs)
        {
            tempATK *= B.damageMulti;
//            Debug.Log("damageMulti = "+ B.damageMulti+ " , tempATK = "+ tempATK);
        }

        DMG = ((tempATK - dDEF) > 1) ?(int) (tempATK - dDEF) : 1;
        return DMG;
    }

    //触发BUFF衰减
    public void BuffsDecay(int beatNum)
    {
        List<Buff> remainbuffs = new List<Buff>();
        foreach(Buff b in buffs)
        {
            b.BuffDecay();
            if (b.remainBeats != 0)
            {
                remainbuffs.Add(b);
            }
            else
            {
                b.BuffRemove();
            }
        }
        buffs = remainbuffs;
//        Debug.Log("buff count="+buffs.Count);

    }


    //触发BUFF效果
    public void BuffsBeat(int beatNum)
    {
        foreach (Buff b in buffs)
        {
            b.BuffBeat(beatNum);
        }
    }

    //判断是否存在Buff
    public bool hasBuff<T> () where T : Buff
    {
        foreach (var b in buffs)
        {
            if (b.GetType() == typeof(T)) return true;
        }
        return false;
    }

    //判断是否存在Buff
    public bool RemoveBuff<T>() where T : Buff
    {
        int num = -1;
        for (int i=0;i<buffs.Count; i++)
        {
            if (buffs[i].GetType() == typeof(T))
            {
                num = i;
            }
        }
        if (num >= 0)
        {
            buffs[num].BuffRemove();
            buffs.RemoveAt(num);
            return true;
        }
        return false;
    }

    //更新长时间buff，比如自动集气外挂
    public void UpdateBuff()
    {
        foreach (var b in buffs)
        {
            b.PlayerUpdate();
        }
    }

    //增加魂儿
    public void AddSoul(int dSoul)
    {
        soulPoint = Mathf.Clamp(dSoul + soulPoint, 0, soulMaxPoint);
    }

    //魂清空
    public void ClearSoul()
    {
        soulPoint = 0;
    }

    public virtual void CastSkill(string str)
    {

    }
}
