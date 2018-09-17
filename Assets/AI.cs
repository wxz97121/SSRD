using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AI : Character {
    private BarController barController;
    // Use this for initialization
    override protected void Start()
    {
        base.Start();
        barController = GameObject.Find("PachuliBar").GetComponent<BarController>();
        mTarget = GameObject.Find("Youmu").GetComponent<Player>();
        if (barController != null)
        {
            //... 
        }
    }

    // Update is called once per frame
    override protected void Update()
    {
        base.Update();
        float a = Random.Range(0, 3f);
        if (a<=1f){
            barController.ShowAction(actionType.Charge);
        }
        else if (a<=2f){
            barController.ShowAction(actionType.Hit);
        }
        else {
            barController.ShowAction(actionType.Defense);
        }

    }

    protected override void UpdateInput()
    {
        base.UpdateInput();
    }
}
