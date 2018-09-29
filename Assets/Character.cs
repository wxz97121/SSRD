using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Character : MonoBehaviour {
    public int mHp = 3;
    public int mMp = 0;
    public TextMeshProUGUI UIHpNum;
    public TextMeshProUGUI UIMpNum;
    public Character mTarget;
    // Use this for initialization
    virtual protected void Start () {

    }
	
	// Update is called once per frame
	virtual protected void Update () {
        UIHpNum.SetText(mHp.ToString());
        UIMpNum.SetText(mMp.ToString());
        UpdateInput();
    }

    virtual protected void UpdateInput (){

    }

    virtual public bool Charge()
    {
        mMp+=1;
        return true;
    }

    virtual public void Hit()
    {
        mTarget.Damage(1);
    }

    virtual public bool Defense(){
        return true;
    }

    virtual public void Damage (int dDam){
        if (mHp>dDam){
            mHp -= dDam;
        }
    }

}
