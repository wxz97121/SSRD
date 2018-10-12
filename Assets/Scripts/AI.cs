using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//敌人AI，角色派生类
public class AI : Character {

    //敌人当前的动作序号(在序列的哪一拍)
    public int actionID = 0;
    //敌人的动作序列(控制每一拍的动作)
    public int[] actionSequence;

    //敌人编号(目前控制外观模型)
    public int enemyID = 0;
    //改样子的，不用动
    public SpriteRenderer spriteRenderer = null;
    //敌人的初始位置(基本不用动，多人可能有用)
    public Vector3 originPosition = new Vector3(5,0,0);

    //死亡后掉落金钱数
    public int lootMoney = 1;


    //是否会冲(仅影响动画效果)
    public bool isDashable = true;
    // Use this for initialization
    override protected void Start()
    {
        //默认初始化
        base.Start();
        //初始化动作序号,下一拍到0
        actionID = -1;
        //初始化敌人目标，默认是玩家
        mTarget = GameObject.Find("Player");
        //不用管
        spriteRenderer = GetComponent<SpriteRenderer>();
        //开始默认的待机状态
        StartCoroutine("IdleState");
        //初始化敌人模型位置
        originPosition = transform.position;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }
    //敌人死亡的时候从玩家的目标列表中去除，然后销毁模型和ui
    override public void Die (){
        gameObject.AddComponent<VFX>();
        VFX vfx = gameObject.GetComponent<VFX>();
        Player.Instance.enemyList.Remove(gameObject);
        Destroy(UIHpNum.transform.parent.gameObject);
        vfx.StartCoroutine("FadeOut");
        Player.Instance.money += lootMoney;
    }

    protected override void UpdateInput()
    {
        base.UpdateInput();
    }

    //序列帧处理，每一拍处理当前的怪兽序列
    public void Action (){
        Debug.Log("AI:action"+ actionID);
        if (actionID>=0){
            //Debug.Log(name + ": " + actionSequence[actionID]);
            switch (actionSequence[actionID])
            {
                //idle
                case 0:
                    {
                        StartCoroutine("IdleState");
                    }
                    break;
                //ready
                case 1:
                    {
                        StartCoroutine("ReadyState");
                    }
                    break;
                //attack
                case 2:
                    {
                        Hit();
                        StartCoroutine("AttackState");
                       
                    }
                    break;
            }
        }


    //    actionID = (actionID + 1) % actionSequence.Length;
    }

    override public void Damage(int dDamage)
    {
        base.Damage(dDamage);
        StartCoroutine("DamagedState");
    }

    //下面是几个状态，只影响动画
    IEnumerator IdleState()
    {
        transform.position = originPosition;
        spriteRenderer.sprite = (Sprite)Resources.Load("Animation/" + enemyID.ToString() + "/spr_idle", typeof(Sprite));
        yield return 0;

    }
    IEnumerator ReadyState()
    {
        float dTime = 0f;
        spriteRenderer.sprite = (Sprite)Resources.Load("Animation/" + enemyID.ToString() + "/spr_ready", typeof(Sprite));

        if (isDashable){
            yield return new WaitForSeconds(BarController.Instance.secPerBeat * 0.25f);
            spriteRenderer.sprite = (Sprite)Resources.Load("Animation/" + enemyID.ToString() + "/spr_dash", typeof(Sprite));
            while (dTime < (BarController.Instance.secPerBeat * 0.75f))
            {
                transform.position = originPosition + (mTarget.transform.position - originPosition) * dTime / (BarController.Instance.secPerBeat);
                dTime += Time.deltaTime;
                yield return new WaitForSeconds(Time.deltaTime);
            }
        }
        else {
            yield return 0;
        }
    }
    IEnumerator DamagedState()
    {
        yield return new WaitForSeconds(BarController.Instance.secPerBeat * 0.25f);
    }
    IEnumerator AttackState(){
        spriteRenderer.sprite = (Sprite)Resources.Load("Animation/" + enemyID.ToString() + "/spr_attack", typeof(Sprite));
        yield return 0;
    }


}
