using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : Character {
    private BarController barController;
	// Use this for initialization
	override protected void Start () {
        base.Start();
        barController = GameObject.Find("YoumuBar").GetComponent<BarController>();
        mTarget = GameObject.Find("Pachuli").GetComponent<AI>();
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
}
