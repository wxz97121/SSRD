using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//角色基类，控制玩家和敌人的全部属性和行动
public class Character : MonoBehaviour
{
    public int maxHp = 3;
    public int Hp = 3;
    public int maxMp = 5;
    public int Mp = 0;
    //基础攻击力
    public int ATK = 2;
    //当前攻击力
    public int currentATK = 1;
    //基础防御力
    public int DEF = 0;
    //当前防御力
    public int currentDEF = 0;
    public bool isUndead = false;
    //装备列表
    public List<Equipment> equipmentList;
    //当前装备着的武器
    public Equipment currentWeapon;
    //当前装备着的护甲
    public Equipment currentArmor;
    //当前装备着的卷轴
    public Equipment currentScroll;


    //当前BUFF列表
    public List<Buff> buffs = new List<Buff>();

    public TextMeshProUGUI UIHpNum;
    public TextMeshProUGUI UIMpNum;

    //当前攻击的目标
    public GameObject mTarget;
    //护盾
    public int Shield = 0;
    //蓄力列表(控制蓄力时候输出序列)
    public List<int> mChargeList = new List<int>();
    //上一次行动(暂时用)
//    public actionType lastAction = actionType.None;
    //这一拍有没有被击中
    public bool getHit = false;
    //蓄力动画
    public GameObject chargeVfx;
    // Use this for initialization
    virtual protected void Start()
    {

    }

    // Update is called once per frame
    virtual protected void Update()
    {
        UIHpNum.SetText(Hp.ToString());
        UIMpNum.SetText(Mp.ToString());
        UpdateInput();
        UpdateBuff();
    }

    virtual protected void UpdateInput()
    {

    }
    //每次行动之前的初始化:判断蓄力是否断，清空护盾和被击
    virtual public void Initialize()
    {


        getHit = false;



    }
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
    //攻击
    virtual public bool Hit(int dDamage)
    {
        //普通攻击目标
        if (mTarget != null)
        {
            Character cTarget = mTarget.GetComponent<Character>();
            //判断对方护盾
            if (cTarget.buffs.Exists(x => x.m_name == "defend"))
            {
                Instantiate(Resources.Load("VFX/Shield"), cTarget.transform.position, Quaternion.identity);
                return false;
            }
            else
            {
                Instantiate(Resources.Load("VFX/Slash"), cTarget.transform.position, Quaternion.identity);
                cTarget.Damage(CalcDmg(getCurrentATK(), cTarget.getCurrentDEF(), dDamage), this);

                //执行BUFF中的攻击结束效果

                List<Buff> tempbuffs = new List<Buff>();
                tempbuffs.AddRange(buffs);
                foreach (Buff b in tempbuffs)
                {

                    b.AfterAttack(this);
                }

                return true;
            }

        }

        //        lastAction = actionType.Hit;
        return true;
    }
    //攻击失败
    virtual public void HitFail()
    {

    }

    virtual public void Heal(int dHeal)
    {
        Hp = Hp + dHeal;
        if (Hp>maxHp)
        {
            Hp = maxHp;
        }
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

        Bubble.AddBubble(BubbleSprType.hp, "-" + dDamage.ToString(), this);

        getHit = true;
        if (Hp > dDamage)
        {
            Hp -= dDamage;
        }
        else
        {
            Hp = 0;
            if (isUndead) { return; }
            Die();
        }
    }



    //计算当前攻击力
    public int getCurrentATK()
    {
        int mATK = ATK;
        if (currentWeapon)
            mATK += currentWeapon.ATK;
        if (currentArmor)
            mATK += currentArmor.ATK;
        if (currentScroll)
            mATK += currentScroll.ATK;
        return mATK;
    }

    //计算当前防御力
    public int getCurrentDEF()
    {
        int mDEF = DEF;
        if (currentWeapon)
            mDEF += currentWeapon.DEF;
        if (currentArmor)
            mDEF += currentArmor.DEF;
        if (currentScroll)
            mDEF += currentScroll.DEF;
        return mDEF;
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

    //更新长时间buff
    public void UpdateBuff()
    {
        foreach (var b in buffs)
        {
            b.PlayerUpdate();
        }
    }
}
