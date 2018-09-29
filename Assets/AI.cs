using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Character {

    public int actionID = 0;
    public int[] actionSequence;

    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        actionID = -1;
        mTarget = GameObject.Find("Player");

    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        float a = Random.Range(0, 3f);
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
        if (actionID>=0){
            Debug.Log(name + ": " + actionSequence[actionID]);
            switch (actionSequence[actionID])
            {
                case 0:
                    {
                        GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    break;
                case 1:
                    {
                        GetComponent<SpriteRenderer>().color = Color.yellow;
                    }
                    break;
                case 2:
                    {
                        StartCoroutine("AttackColor");
                        Hit();
                    }
                    break;
            }
        }


        actionID = (actionID + 1) % actionSequence.Length;
    }

    IEnumerator AttackColor(){
        GetComponent<SpriteRenderer>().color = Color.red;
        yield return new WaitForSeconds(0.1f);
        GetComponent<SpriteRenderer>().color = Color.white;
    }
}
