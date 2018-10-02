using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//玩家，角色派生类
public class Player : Character {
    static Player _instance;

    private Animator animator;

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
    }

    override public void Defense()
    {
        base.Defense();
        animator.Play("player_defense", 0);
        SoundController.Instance.PlayAudioEffect("HIHAT");

    }

    override public void Damage(int dDamage)
    {
        base.Damage(dDamage);
        animator.Play("player_damaged", 0);
    }
}
