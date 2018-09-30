using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Character {

    public int actionID = 0;
    public int[] actionSequence;

    public int enemyID = 0;
    public SpriteRenderer spriteRenderer = null;
    public Vector3 originPosition = new Vector3(5,0,0);

    public bool isDashable = true;
    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        actionID = -1;
        mTarget = GameObject.Find("Player");
        spriteRenderer = GetComponent<SpriteRenderer>();
        StartCoroutine("IdleState");
        originPosition = transform.position;
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
    }

    override public void Die (){
        Player.Instance.enemyList.Remove(gameObject);
        Destroy(UIHpNum.transform.parent.gameObject);
        Destroy(gameObject);
    }

    protected override void UpdateInput()
    {
        base.UpdateInput();
    }

    public void Action (){
        Debug.Log("AI:action");
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
                //prepare
                case 1:
                    {
                        StartCoroutine("PrepareState");
                    }
                    break;
                //attack
                case 2:
                    {
                        if (Hit()){
                            StartCoroutine("AttackState");
                        }
                    }
                    break;
            }
        }


        actionID = (actionID + 1) % actionSequence.Length;
    }

    override public void Damage(int dDamage)
    {
        base.Damage(dDamage);
        StartCoroutine("DamagedState");
    }

    IEnumerator IdleState()
    {
        transform.position = originPosition;
        spriteRenderer.sprite = (Sprite)Resources.Load("Animation/" + enemyID.ToString() + "/spr_idle", typeof(Sprite));
        yield return 0;

    }
    IEnumerator PrepareState()
    {
        float dTime = 0f;
        spriteRenderer.sprite = (Sprite)Resources.Load("Animation/" + enemyID.ToString() + "/spr_prepare", typeof(Sprite));

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
