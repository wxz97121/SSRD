using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


//玩家，角色派生类
public class Player : Character {
    static Player _instance;

    private Animator animator;

    //魂点数 魂等级
    public int soulPoint = 0;
    public int soulLevel = 0;
    //魂到达多少升级
    public int soulMaxPoint = 10;
    //魂的UI元素
    public Image soulPointProgress;
    public Image soulLevelLetter;

    //携带金钱数
    public int money=10;
    public TextMeshProUGUI UIMoneyNum;



    public List<GameObject> enemyList = new List<GameObject>();

    private void Awake()
    {
        _instance = this;
    }

    public static Player Instance
    {
        get
        {
            return _instance;
        }
    }

    // Use this for initialization
    override protected void Start () {
        base.Start();
        animator = GetComponent<Animator>();

	}

    // Update is called once per frame
    override protected void Update () {
        base.Update();
        updateSoulUI();
        UIMoneyNum.SetText(money.ToString());

    }
    //控制玩家的输入
    protected override void UpdateInput()
    {
        base.UpdateInput();

        if (Input.GetKeyDown(KeyCode.Z)){
            BarController.Instance.ShowAction(actionType.Charge);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            BarController.Instance.ShowAction(actionType.Hit);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            BarController.Instance.ShowAction(actionType.Defense);
        }
    }

    override public void AddMp(int dMp)
    {
        base.AddMp(dMp);
    }
    //玩家具体的控制
    override public bool Hit()
    {
        animator.Play("player_slash",0);
        SoundController.Instance.PlayAudioEffect("SNARE");
        return base.Hit();
    }

    override public void HitFail (){
        base.HitFail();
        animator.Play("player_fail", 0);
        SoundController.Instance.PlayAudioEffect("ROUND");

    }

    override public void Defense()
    {
        base.Defense();
        animator.Play("player_defense", 0);
        SoundController.Instance.PlayAudioEffect("TOM");

    }

    override public void Damage(int dDamage)
    {
        base.Damage(dDamage);
        animator.Play("player_damaged", 0);
        SoundController.Instance.PlayAudioEffect("HIHAT");

    }

    public void addSoulPoint(int dSoulPoint){
        soulPoint += dSoulPoint;
        if (soulPoint >= soulMaxPoint)
        {
            if(soulLevel>=2){
                soulPoint = soulMaxPoint;
            }else{
                soulLevel++;
                soulPoint = 0;
            }
        }
    }



    public void decreaseSoulLevel(){
        if(soulLevel>0){
            soulLevel--;
        }
        soulPoint = 0;
    }

    public void updateSoulUI(){
        soulPointProgress.fillAmount =((float)soulPoint / (float)soulMaxPoint);
        switch(soulLevel){
            case 0:
                soulLevelLetter.color = Color.black;
                soulPointProgress.color = Color.black;

                break;
            case 1:
                soulLevelLetter.color = Color.yellow;
                soulPointProgress.color = Color.yellow;


                break;
            case 2:
                soulLevelLetter.color = Color.red;
                soulPointProgress.color = Color.red;

                break;
        }
    }
}
