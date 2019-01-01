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

    //装备列表
    public List<Equipment> equipmentList;
    //当前装备着的武器
    public Equipment currentWeapon;
    //当前装备着的护甲
    public Equipment currentArmor;
    //当前装备着的卷轴
    public Equipment currentScroll;

    //当前的技能列表
    public List<Skill> skills = new List<Skill>();


    public TextMeshProUGUI UIHpNum;
    public TextMeshProUGUI UIMpNum;
    //当前攻击的目标
    public GameObject mTarget;
    //护盾
    public int Shield = 0;
    //蓄力列表(控制蓄力时候输出序列)
    public List<int> mChargeList = new List<int>();
    //上一次行动(暂时用)
    public actionType lastAction = actionType.None;
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
    }

    virtual protected void UpdateInput()
    {

    }
    //每次行动之前的初始化:判断蓄力是否断，清空护盾和被击
    virtual public void Initialize()
    {
        if (lastAction == actionType.None)
        {
            ChargeBreak(0);
        }
        if (mTarget == null)
        {
            ChargeBreak(0);
        }
        //Debug.Log(mTarget);
        lastAction = actionType.None;
        Shield = 0;
        getHit = false;



    }
    //蓄力(目前只会在蓄力列表加一个简单的标识)
    virtual public bool Charge()
    {
        if (mChargeList.Count == 0)
        {
            chargeVfx = (GameObject)Instantiate(Resources.Load("VFX/Charge"), transform.position + new Vector3(-1.2f, 0.5f, 0), Quaternion.identity);
        }

        mChargeList.Add(BarController.Instance.nextNoteIndex_main);

        lastAction = actionType.Charge;
        return true;
    }
    //加灵力
    virtual public void AddMp(int dMp)
    {
        if (Mp + dMp <= maxMp)
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
                if (cTarget.Shield > 0)
                {
                    Instantiate(Resources.Load("VFX/Shield"), cTarget.transform.position, Quaternion.identity);
                    return false;
                }
                else
                {
                    Instantiate(Resources.Load("VFX/Slash"), cTarget.transform.position, Quaternion.identity);
                    cTarget.Damage(CalcDmg(getCurrentATK(), cTarget.getCurrentDEF(), dDamage));
                    return true;
                }

            }

        lastAction = actionType.Hit;
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

    //防御
    virtual public void Defense()
    {
        if (mChargeList.Count > 0)
        {
            ChargeBreak(0);
        }
        Shield += 1;
        lastAction = actionType.Defense;
    }
    //死亡
    virtual public void Die()
    {
    }
    //受到伤害(以后名字要改下)
    virtual public void Damage(int dDamage)
    {
        getHit = true;
        if (Hp > dDamage)
        {
            Hp -= dDamage;
            ChargeBreak(0);
        }
        else
        {
            Die();
        }
    }
    //判断蓄力断裂：一种是被打断，一种是使用了招数
    //type:0-fail,1-success 
    virtual public void ChargeBreak(int type)
    {
        Destroy(chargeVfx.gameObject);
        mChargeList.Clear();
    }

    //每个小节结束自动调用
    public virtual void OnTurnEnd()
    {
        ChargeBreak(0);
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
        DMG = ((dATK + dDamage - dDEF) > 1) ? (dATK + dDamage - dDEF) : 1;
        return DMG;
    }
}
