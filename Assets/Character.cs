using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour {
    public int maxHp = 3;
    public int Hp = 3;
    public int maxMp = 5;
    public int Mp = 0;
    public TextMeshProUGUI UIHpNum;
    public TextMeshProUGUI UIMpNum;
    public GameObject mTarget;

    public int Shield = 0;

    public List<int> mChargeList = new List<int>();

    public actionType lastAction = actionType.None;

    public bool getHit = false;

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
    virtual public bool Charge()
    {
        if (mChargeList.Count==0){
            chargeVfx = (GameObject)Instantiate(Resources.Load("VFX/Charge"), transform.position + new Vector3(-1.2f, 0.5f, 0), Quaternion.identity);
        }

        mChargeList.Add(1);

        lastAction = actionType.Charge;
        return true;
    }
    virtual public void AddMp(int dMp)
    {
        if (Mp+dMp<=maxMp){
            Mp = maxMp;
        }
        else {
            Mp += dMp;
        }
    }
    virtual public bool Hit()
    {
        if (mChargeList.Count>0){
            if (mTarget != null)
            {
                Character cTarget = mTarget.GetComponent<Character>();
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
            if (mTarget != null)
            {
                Character cTarget = mTarget.GetComponent<Character>();
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
    virtual public void HitFail()
    {

    }

    virtual public void Defense(){
        if (mChargeList.Count > 0)
        {
            ChargeBreak(0);
        }
        Shield += 1;
        lastAction = actionType.Defense;
    }

    virtual public void Die (){

    }

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

    //type:0-fail,1-success 
    virtual public void ChargeBreak (int type){
        Destroy(chargeVfx.gameObject);
        mChargeList.Clear();
    }

}
