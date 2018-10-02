using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

//角色基类，控制玩家和敌人的全部属性和行动
public class Character : MonoBehaviour {
    public int maxHp = 3;
    public int Hp = 3;
    public int maxMp = 5;
    public int Mp = 0;
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
    virtual protected void Start () {

    }
	
	// Update is called once per frame
	virtual protected void Update () {
        UIHpNum.SetText(Hp.ToString());
        UIMpNum.SetText(Mp.ToString());
        UpdateInput();
    }

    virtual protected void UpdateInput (){

    }
    //每次行动之前的初始化:判断蓄力是否断，清空护盾和被击
    virtual public void Initialize ()
    {
        if (lastAction == actionType.None){
            ChargeBreak(0);
        }
        if (mTarget == null){
            ChargeBreak(0);
        }
        Debug.Log(mTarget);
        lastAction = actionType.None;
        Shield = 0;
        getHit = false;
    }
    //蓄力(目前只会在蓄力列表加一个简单的标识)
    virtual public bool Charge()
    {
        if (mChargeList.Count==0){
            chargeVfx = (GameObject)Instantiate(Resources.Load("VFX/Charge"), transform.position + new Vector3(-1.2f, 0.5f, 0), Quaternion.identity);
        }

        mChargeList.Add(1);

        lastAction = actionType.Charge;
        return true;
    }
    //加灵力
    virtual public void AddMp(int dMp)
    {
        if (Mp+dMp<=maxMp){
            Mp = maxMp;
        }
        else {
            Mp += dMp;
        }
    }
    //攻击
    virtual public bool Hit()
    {
        //判断是否是蓄力招
        if (mChargeList.Count>0){
            //判断目标
            if (mTarget != null)
            {
                Character cTarget = mTarget.GetComponent<Character>();
                //判断招数(判断顺序应该和判断目标换一下)
                if (mChargeList.Count == 1){
                    Hp = maxHp;
                }
                else {
                    cTarget.Damage(999);
                }
                ChargeBreak(1);
            }
        }
        else {
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
                    cTarget.Damage(1);
                    return true;
                }

            }
        }

        lastAction = actionType.Hit;

        return true;
    }
    //攻击失败
    virtual public void HitFail()
    {

    }
    //防御
    virtual public void Defense(){
        if (mChargeList.Count > 0)
        {
            ChargeBreak(0);
        }
        Shield += 1;
        lastAction = actionType.Defense;
    }
    //死亡
    virtual public void Die (){

    }
    //受到伤害(以后名字要改下)
    virtual public void Damage (int dDamage){
        getHit = true;
        if (Hp>dDamage){
            Hp -= dDamage;
            ChargeBreak(0);
        }
        else {
            Die();
        }
    }
    //判断蓄力断裂：一种是被打断，一种是使用了招数
    //type:0-fail,1-success 
    virtual public void ChargeBreak (int type){
        Destroy(chargeVfx.gameObject);
        mChargeList.Clear();
    }

}
