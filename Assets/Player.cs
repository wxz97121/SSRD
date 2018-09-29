using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    private BarController barController;
    private Animator animator;
	// Use this for initialization
	override protected void Start () {
        base.Start();
        barController = GameObject.Find("JudgeBar").GetComponent<BarController>();
        animator = GetComponent<Animator>();
        if (barController!=null){
            //... 
        }
	}

    // Update is called once per frame
    override protected void Update () {
        base.Update();
	}

    protected override void UpdateInput()
    {
        base.UpdateInput();

        if (Input.GetKeyDown(KeyCode.Z)){
            barController.ShowAction(actionType.Charge);
        }
        else if (Input.GetKeyDown(KeyCode.X))
        {
            barController.ShowAction(actionType.Hit);
        }
        else if (Input.GetKeyDown(KeyCode.C))
        {
            barController.ShowAction(actionType.Defense);
        }
    }

    override public void Hit()
    {
        base.Hit();
        animator.Play("player_slash",0);
    }
}
