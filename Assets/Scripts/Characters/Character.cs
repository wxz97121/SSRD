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

    virtual public void Hit(int dDamage, bool noAfterattack=false,bool isDefenceToDisable= false,string penetrateEffstr="",bool isDefencePenetrate = false,string sfxstr="SLASH",string fxstr="NORMAL")
    {
        //普通攻击目标
        if (mTarget != null)
        {
            Character cTarget = mTarget.GetComponent<Character>();
            bool ishit = true;
            //判断对方护盾
            if (cTarget.HasBuff("defend"))
            {
                //破防
                if (isDefencePenetrate)
                {
                    if (penetrateEffstr == "1")
                    {
                        dDamage += 5;
                    }

                    VFX.ShowVFX("PierceHit", new Vector3(4f,0f,-1f) +transform.localPosition) ;

                    //todo:通用的破防音效

                }
                else
                {


                    //被防到硬直;
                    if (isDefenceToDisable)
                    {
                        GameObject fxClone = Instantiate(Resources.Load("VFX/DefendBig"), cTarget.transform.position, Quaternion.identity) as GameObject;
                        fxClone.transform.localScale = new Vector3(fxClone.transform.localScale.x * cTarget.transform.Find("pos_defendfx").transform.localScale.x, fxClone.transform.localScale.y, fxClone.transform.localScale.z);
                        SoundController.Instance.PlayAudioEffect("PDEFEND");
                        isDefenced = true;
                    }
                    else
                    {
                        //被防住
                        GameObject fxClone = Instantiate(Resources.Load("VFX/Defend"), cTarget.transform.Find("pos_defendfx").transform.position, Quaternion.identity) as GameObject;
                        fxClone.transform.localScale = new Vector3(3f * cTarget.transform.Find("pos_defendfx").transform.localScale.x, 3f, 3f);

                        SoundController.Instance.PlayAudioEffect("DEFEND");

                    }

                    //释放防御成功带的技能
                    string defskillstr = (cTarget.buffs.Find((Buff obj) => obj.m_name == "defend") as Buff_defend).EffStr;
                    cTarget.CastSkill(defskillstr);
                    //Debug.Log("防御成功附带技能：" + defskillstr);
                    ishit = false;
                }


            }

            //反击
            foreach (Buff b in cTarget.buffs)
            {
                bool iscountered = false;
                if (b.m_name == "counter")
                {
                    ishit = false;
                    (b as Buff_counter).CounterAction();
                    iscountered = true;
                }
                if (iscountered)
                {
                    GameObject fxClone = Instantiate(Resources.Load("VFX/CounterHit"), cTarget.transform.Find("pos_defendfx").transform.position, Quaternion.identity) as GameObject;
                    fxClone.transform.localScale = new Vector3(3f * cTarget.transform.Find("pos_defendfx").transform.localScale.x, 3f, 3f);
                    SoundController.Instance.PlayAudioEffect("PDEFEND");
                    SuperController.Instance.ShowInputTip("Counter!");

                }
            }


            //确定被击中时
            if (ishit)
            {
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

        //如过被防住了 触发效果
        if (isDefenced)
        {

            IsDefended();
        }
        //        lastAction = actionType.Hit;
    }



    virtual public void IsDefended()
    {
        isDefenced = false;

    }


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
        List<Buff> tempbuffs = new List<Buff>();
        tempbuffs.AddRange(buffs);
        foreach (Buff b in tempbuffs)
        {
            Debug.Log("When Die");

            b.WhenDie(this);
        }
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
    public bool HasBuff(string buffname) 
    {
        foreach (var b in buffs)
        {
            if (b.m_name == buffname) return true;
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
